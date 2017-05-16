using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IchigoJamKeyBoard
{
    public partial class GetForm : Form
    {
        #region  ///        フォームの開始と終了          ///
        public GetForm()
        {
            InitializeComponent();
        }

        private void SaveForm_Load(object sender, EventArgs e)
        {
            pgbSave.Visible = false;
            pgbSave.Value = 0;
        }

        private void SaveForm_Shown(object sender, EventArgs e)
        {
            //キーフックを避けます
            MainForm.HookEnable = false;
        }

        /// <summary>
        /// フォームを閉じます
        /// </summary>
        private void closeAll()
        {
            //キーフックを通します
            MainForm.HookEnable = true;

            this.Close();
        }

        private void SaveForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //フォームを閉じます
            closeAll();
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            //フォームを閉じます
            closeAll();
        }

        /// <summary>
        /// 保存ボタンが押されました
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //番号の整合性チェック
            int num = 0;
            if (!int.TryParse(tbxSaveNum.Text.Trim(), out num))
            {
                MessageBox.Show(ValiableList.errSaveNumber, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (num < 0)
            {
                MessageBox.Show(ValiableList.errSaveNumber, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sfdSave.Title = ValiableList.sfdSaveTitleMes;                                               // タイトルバーの文字列

            sfdSave.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);    //ダイアログの開く場所

            sfdSave.Filter = ValiableList.sfdSaveFilterMes;                                             //「ファイルの種類」を指定

            sfdSave.FileName = "";

            if (sfdSave.ShowDialog() == DialogResult.OK)
            {
                //プログレスバーを表示
                pgbSave.Value = 0;
                pgbSave.Visible = true;
                Application.DoEvents();

                //ESCを送信します
                SerialTool.SendComm(27);
                Thread.Sleep(25);
                SerialTool.SendComm(27);
                pgbSave.Value = pgbSave.Value + 3;
                Application.DoEvents();

                //PageDown送信
                SerialTool.SendComm(20);
                Thread.Sleep(25);
                SerialTool.SendComm(20);
                pgbSave.Value = pgbSave.Value + 3;
                Application.DoEvents();

                //改行を送信します
                SerialTool.SendComm(10);
                pgbSave.Value = pgbSave.Value + 3;
                Application.DoEvents();

                //load番号を送信します
                byte[] bins = Encoding.GetEncoding(932).GetBytes("load " + num.ToString());
                foreach (byte cha in bins)
                {
                    SerialTool.SendComm((byte)cha);
                    Thread.Sleep(25);
                    pgbSave.Value = pgbSave.Value + 3;
                    Application.DoEvents();
                }
                //改行を送信します
                SerialTool.SendComm(10);

                //0.5sec待ちます
                Thread.Sleep(500);

                SerialTool.RecvSave.Clear();

                //listを送信します
                bins = Encoding.GetEncoding(932).GetBytes("list");
                foreach (byte cha in bins)
                {
                    SerialTool.SendComm((byte)cha);
                    Thread.Sleep(25);
                    pgbSave.Value = pgbSave.Value + 3;
                    Application.DoEvents();
                }
                //改行を送信します
                SerialTool.SendComm(10);

                double va = (double)pgbSave.Value;
                //2.0sec待ちます
                for (int i = 0; i < 200; i++)
                {
                    Thread.Sleep(10);
                    va += 2;
                    if (va > 100.0)
                    {
                        va = 100;
                    }
                    pgbSave.Value = (int)va;
                    Application.DoEvents();
                }

                pgbSave.Value = 100;

                //不要なコードを削除します
                List<byte> res = new List<byte>();
                for (int i = 0; i < SerialTool.RecvSave.Count; i++)
                {
                    if (SerialTool.RecvSave[i] == 0x15)
                    {
                        i += 2;
                        if (i >= SerialTool.RecvSave.Count)
                        {
                            break;
                        }
                    }
                    {
                        res.Add(SerialTool.RecvSave[i]);
                    }
                }

                //bin配列に入れます
                if (SerialTool.RecvSave.Count > 3)
                {
                    bins = new byte[res.Count - 3];
                    for (int i = 0; i < bins.Length; i++)
                    {
                        bins[i] = res[i];
                    }
                }
                else
                {
                    bins = new byte[0];
                }

                //保存します
                File.WriteAllBytes(sfdSave.FileName, bins);

                pgbSave.Visible = false;
            }
        }
    }
}

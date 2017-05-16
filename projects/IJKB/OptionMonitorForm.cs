using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IchigoJamKeyBoard
{
    public partial class OptionMonitorForm : Form
    {
        class OptionMonitor
        {
            public int FrameRate = 10;
        }

        private OptionMonitor OptMonitor = new OptionMonitor();     //設定するパラメータの内部メモリ
        private TreeNode NowNode;                                   //現在表示しているノード
        private TextBox NowTbx;                                     //現在フォーカスすべきテキストボックス


        #region //********     フォーム 起動と終了 の処理       ********//
        public OptionMonitorForm()
        {
            InitializeComponent();

            //自分自身のフォームの大きさを設定する
            this.Size = new Size(472, 218);
        }

        /// <summary>
        /// フォームロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OptionMonitorForm_Load(object sender, EventArgs e)
        {
            //親画面の中央に表示する
            this.Location = new Point(this.Owner.Location.X + (this.Owner.Width - this.Width) / 2, this.Owner.Location.Y + (this.Owner.Height - this.Height) / 2);

            //ツリーノードを初期化します
            InitTreeNode();

            //グループボックスの初期化
            InitGroupBox();
        }

        /// <summary>
        /// グループボックスの初期化
        /// </summary>
        private void InitGroupBox()
        {
            int x = 237;
            int y = 12;

            //グループボックスの位置を初期化します
            gbxFrame.Location = new Point(x, y);
            //gbxBlind.Location = new Point(x, y);
            //gbxWinGroup.Location = new Point(x, y);
            //gbxSeigyohyouka.Location = new Point(x, y);
            //gbxCameras.Location = new Point(x, y);
            //gbxOthers.Location = new Point(x, y);
            //gbxShikiichi.Location = new Point(x, y);
            //gbxGlare.Location = new Point(x, y);
            //gbxSunRemove.Location = new Point(x, y);
            //gbxWinMen.Location = new Point(x, y);

            //Visibleをfalseにする
            UnSetGbxVisible();

            //ノードのパラメータをセットします
            SetNodeParam(gbxFrame);

            //先頭のグループボックスをtrueにする
            gbxFrame.Visible = true;
        }

        /// <summary>
        /// ノードのパラメータをセットします
        /// </summary>
        /// <param name="gbxFrame"></param>
        private void SetNodeParam(GroupBox gbx)
        {
            if (gbx.Name == gbxFrame.Name)
            {
                this.tbxFrameRate.Text = this.OptMonitor.FrameRate.ToString();
            }
            //else if (gbx.Name == gbxBlind.Name)
            //{
            //    tbxSlatWidth.Text = this.aParam.SlatWidth.ToString();
            //    tbxSlatInterval.Text = this.aParam.SlatInterval.ToString();
            //    tbxSlatRefre1.Text = this.aParam.SlatRefre1.ToString();
            //    tbxSlatRefre2.Text = this.aParam.SlatRefre2.ToString();
            //}

        }

        /// <summary>
        /// Visibleをfalseにする
        /// </summary>
        private void UnSetGbxVisible()
        {
            gbxFrame.Visible = false;
            //gbxBlind.Visible = false;
            //gbxWinGroup.Visible = false;
            //gbxSeigyohyouka.Visible = false;
            //gbxCameras.Visible = false;
            //gbxOthers.Visible = false;
            //gbxShikiichi.Visible = false;
            //gbxGlare.Visible = false;
            //gbxSunRemove.Visible = false;
            //gbxWinMen.Visible = false;
        }

        /// <summary>
        /// ツリーノードを初期化します
        /// </summary>
        private void InitTreeNode()
        {
            //パラメータをコピーします
            this.OptMonitor.FrameRate = MonitorForm.FrameRate;

            TreeNode tnFrame = new TreeNode(ValiableList.MonitorOptFrame);
            tnFrame.Name = gbxFrame.Name;

            TreeNode[] treeNodeRoot = { tnFrame };

            tvwOption.Nodes.Clear();
            tvwOption.Nodes.AddRange(treeNodeRoot);

            //tvwOption.TopNode.Expand();
            tvwOption.ExpandAll();

            //最上位のノードにフォーカスをあてます
            tvwOption.Focus();

            tvwOption.SelectedNode = tnFrame;
            this.NowNode = tnFrame;
        }

        /// <summary>
        /// フォームを閉じる処理です
        /// </summary>
        private void FormClose()
        {
            //フォームを閉じる
            this.Close();

            //フォームの解放
            this.Dispose();
        }

        /// <summary>
        /// フォームの終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OptionMonitorForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        #endregion

        #region //********     ノードの表示処理       ********//
        private void tvwOption_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            TextBox tbx;

            //現在表示されているノードの整合性のチェック
            if (VerifyParam(NowNode, out tbx) == false)
            {
                MessageBox.Show(ValiableList.errInputMiss, ValiableList.errMessage, MessageBoxButtons.OK, MessageBoxIcon.Error);

                //タイマーを使って、間違えたところにフォーカスを戻します
                this.NowTbx = tbx;
                tmrInputErrChk.Enabled = true;
                return;
            }


            //グループボックスをすべて非表示にします
            UnSetGbxVisible();

            if (node.Name == gbxFrame.Name)
            {
                SetNodeParam(gbxFrame);
                gbxFrame.Visible = true;
            }
            //else if (node.Name == gbxBlind.Name)
            //{
            //    SetNodeParam(gbxBlind);
            //    gbxBlind.Visible = true;
            //}

            //ノードをセット
            this.NowNode = node;
        }


        /// <summary>
        /// 現在表示されているノードの整合性のチェック
        /// </summary>
        private bool VerifyParam(TreeNode node, out TextBox tbx)
        {
            tbx = null;

            if (node.Name == gbxFrame.Name)
            {
                if (setTextInteger(this.tbxFrameRate, 1, 30, ref this.OptMonitor.FrameRate, true) == false) { tbx = this.tbxFrameRate; return false; }
            }

            return true;
        }
        #endregion

        #region //********         テキストボックス入力値の有効性チェック処理           ********//
        /// <summary>
        /// テキストボックスの値をチェックします(Int)
        /// </summary>
        /// <param name="tbx"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="dat"></param>
        /// <returns></returns>
        private bool setTextInteger(TextBox tbx, int min, int max, ref int dat, bool MinMaxFlg)
        {
            int valu = dat;

            //空のときは、最小値とする
            if (tbx.Text.Trim() == "")
            {
                return false;
            }
            //整数でないときはエラー
            if (IsInteger(tbx.Text.Trim()) == false)
            {
                return false;
            }

            try
            {
                valu = int.Parse(tbx.Text.Trim());
                if (MinMaxFlg == true)
                {
                    //範囲チェック
                    if (valu < min || valu > max)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.Message);
                return false;
            }

            dat = valu;
            return true;
        }

        /// <summary>
        /// 文字列が整数に変換できるかどうか判断します
        /// </summary>
        /// <param name="stTarget"></param>
        /// <returns></returns>
        public static bool IsInteger(string stTarget)
        {
            int iNullable;
            return int.TryParse(stTarget, NumberStyles.Any, null, out iNullable);
        }

        /// <summary>
        /// 入力ミスを指摘するためのタイマーです
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrInputErrChk_Tick(object sender, EventArgs e)
        {
            this.NowTbx.Focus();
            this.NowTbx.SelectAll();

            tvwOption.SelectedNode = this.NowNode;

            tmrInputErrChk.Enabled = false;
        }
        #endregion

        #region //********         ボタンが押されたときの処理           ********//
        /// <summary>
        ///  登録ボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSet_Click(object sender, EventArgs e)
        {
            //現在表示されているノードの整合性のチェック
            TextBox tbx;
            if (VerifyParam(NowNode, out tbx) == false)
            {
                MessageBox.Show(ValiableList.errInputMiss, ValiableList.errMessage, MessageBoxButtons.OK, MessageBoxIcon.Error);

                //タイマーを使って、間違えたところにフォーカスを戻します
                this.NowTbx = tbx;
                tmrInputErrChk.Enabled = true;
                return;
            }

            btnSet.Enabled = false;

            //パラメータの置き換え
            MonitorForm.FrameRate = OptMonitor.FrameRate;

            //フォームを閉じる
            FormClose();
        }

        /// <summary>
        /// キャンセルボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //フォームを閉じる
            FormClose();
        }
        #endregion
    }
}

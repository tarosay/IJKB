using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using RamGecTools;
using System.Configuration;
using System.Collections;

namespace IchigoJamKeyBoard
{
    public partial class MainForm : Form
    {
        public static bool HookEnable = true;                           //キーボードフックを行うかどうか
        public static MonitorForm Monitor = null;                       //モニタフォームインスタンス

        private TextInputForm Textinputform = null;     //ローマ字入力フォーム
        private ImeMode RomajiIMEMode = ImeMode.On;     //ローマ字入力時のIMEモード
        private TextBox[] FuncKey = new TextBox[10];    //F1～F10の内容

        /// <summary>
        /// Class declarations
        /// </summary>
        RamGecTools.KeyboardHook keyboardHook = new RamGecTools.KeyboardHook();

        private SoftKeyBoard SoftKBForm = null;         //ソフトキーボードフォーム
        //private bool BtnModeUpFlg = true;             //BTNコマンドモードでキーがアップしたフラグ
        private int BtnModeCnt = 0;                     //BTNコマンドモード時のキースキャンカウンタ
        private DateTime BtnModeTime = DateTime.Now;

        private bool BlnBkgSerialPrint = false;         //モニタデータバックグラウンド処理のフラグ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            //センターに表示
            //this.StartPosition = FormStartPosition.CenterScreen;

            //使用言語を調べてメッセージをセットします
            ValiableList.selectLanguage(this.Text);
        }

        #region  ///        フォームの開始と終了          ///
        private void MainForm_Load(object sender, EventArgs e)
        {
            // メインフォームのタイトルをセットします
            SetMainFormTitle("", true);

            //コントロールアイテムの初期化
            InitControlItem();

            // カナキーデータを初期化します
            KanaKey.KanaKeyInit();

            //設定データの読み込み
            ReadAppDataLocalData();

            //キーボードフック処理の初期化
            keyboardhookInit();
        }

        /// <summary>
        /// フォームの表示が終了したあと実行される
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            //bkgSerialPrint.RunWorkerAsync();
        }

        /// <summary>
        /// 設定データの読み込み
        /// </summary>
        private void ReadAppDataLocalData()
        {
            AppSettings apps;

            string[] keys = { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10" };

            for (int i = 0; i < keys.Length; i++)
            {
                apps = new AppSettings();
                //apps.Upgrade();
                apps.SettingsKey = keys[i];

                if (apps.command != null)
                {
                    FKey.fkey[i] = apps.command;
                    FuncKey[i].Text = FKey.fkey[i];
                }
            }

            apps = new AppSettings();
            //apps.Upgrade();
            apps.SettingsKey = "Kana";

            if (apps.command != null)
            {
                cmbKanaRoma.SelectedIndex = int.Parse(apps.command);
            }

            apps = new AppSettings();
            //apps.Upgrade();
            apps.SettingsKey = "KeyAccept";

            if (apps.command != null)
            {
                int num = int.Parse(apps.command);

                if (num == 0)
                {
                    rdiActiveOnly.Checked = true;
                }
                else if (num == 1)
                {
                    rdiFullTime.Checked = true;
                }
            }

            apps = new AppSettings();
            //apps.Upgrade();
            apps.SettingsKey = "MonitorFrameRate";

            if (apps.command != null)
            {
                MonitorForm.FrameRate = int.Parse(apps.command);
            }
        }

        /// <summary>
        /// 設定値を保存します
        /// </summary>
        private void WriteAppDataLocalData()
        {
            AppSettings apps;

            string[] keys = { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10" };

            for (int i = 0; i < keys.Length; i++)
            {
                apps = new AppSettings();
                apps.SettingsKey = keys[i];

                apps.command = FKey.fkey[i];
                apps.Save();
            }

            apps = new AppSettings();
            apps.SettingsKey = "Kana";
            apps.command = cmbKanaRoma.SelectedIndex.ToString();
            apps.Save();

            apps = new AppSettings();
            apps.SettingsKey = "KeyAccept";
            if (rdiActiveOnly.Checked == true)
            {
                apps.command = "0";
            }
            else
            {
                apps.command = "1";
            }
            apps.Save();

            apps = new AppSettings();
            apps.SettingsKey = "MonitorFrameRate";
            apps.command = MonitorForm.FrameRate.ToString();
            apps.Save();
        }

        /// <summary>
        /// キーボードフック処理の初期化
        /// </summary>
        private void keyboardhookInit()
        {
            // register evens
            keyboardHook.KeyDown += new RamGecTools.KeyboardHook.KeyboardHookCallback(keyboardHook_KeyDown);
            keyboardHook.KeyUp += new RamGecTools.KeyboardHook.KeyboardHookCallback(keyboardHook_KeyUp);

            keyboardHook.Install();
        }

        /// <summary>
        /// メインフォームのタイトルをセットします
        /// </summary>
        /// <param name="exportfilename"></param>
        /// <param name="buildpartFlg"></param>
        private void SetMainFormTitle(string exportfilename, bool buildpartFlg)
        {
            //自分自身のバージョン情報を取得する
            FileVersionInfo ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

            //タイトルの表示
            //this.Text = string.Concat(ValiableList.MainFormTtle, " Ver ", ver.FileMajorPart, ".", ver.FileMinorPart, ".", ver.FileBuildPart, " - ", exportfilename);
            if (buildpartFlg == true)
            {
                this.Text = string.Concat(ValiableList.MainFormTtle, " Ver ", ver.FileMajorPart, ".", ver.FileMinorPart.ToString(), ".", ver.FileBuildPart.ToString(), " ", exportfilename);
            }
            else
            {
                this.Text = string.Concat(ValiableList.MainFormTtle, " Ver ", ver.FileMajorPart, ".", ver.FileMinorPart.ToString(), " ", exportfilename);
            }

            this.Text += "  by Tarosay";
        }

        /// <summary>
        /// コントロールアイテムの初期化
        /// </summary>
        private void InitControlItem()
        {
            btnConnect.Enabled = true;
            btnConnectCut.Enabled = false;
            gbxKanaRoma.Enabled = false;
            gbxBtn.Enabled = false;
            tbxKeyChara.Enabled = false;
            chkEchoBak.Enabled = false;
            chkSoftKeyBoard.Enabled = false;



            //シリアルポートをセットする
            string[] portlist = SerialPort.GetPortNames();
            cmbSerialPort.Items.Clear();

            foreach (string portname in portlist)
            {
                cmbSerialPort.Items.Add(portname);
            }
            cmbSerialPort.DropDownStyle = ComboBoxStyle.DropDownList;
            if (cmbSerialPort.Items.Count > 0)
            {
                cmbSerialPort.SelectedIndex = 0;
            }

            //ボーレートをセットする
            string[] baurates = new string[SerialTool.Baurate.Length];
            for (int i = 0; i < baurates.Length; i++)
            {
                baurates[i] = SerialTool.Baurate[i].ToString();
            }
            cmbBaurate.Items.Clear();

            foreach (string baurate in baurates)
            {
                cmbBaurate.Items.Add(baurate);
            }
            cmbBaurate.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBaurate.SelectedIndex = 3;

            //かなモードをセットする
            cmbKanaRoma.Items.Clear();
            cmbKanaRoma.Items.Add(ValiableList.cmbKanaRomaMes[0]);
            cmbKanaRoma.Items.Add(ValiableList.cmbKanaRomaMes[1]);
            cmbKanaRoma.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbKanaRoma.SelectedIndex = KanaKey.KanaMode;

            tbxKeyChara.Text = "";
            for (int i = 0; i < 8; i++)
            {
                tbxKeyChara.Text += "\r\n";
            }
            tbxKeyChara.Text += ValiableList.tbxKeyCharaMes[0] + "\r\n\r\n";
            tbxKeyChara.Text += ValiableList.tbxKeyCharaMes[1] + "\r\n";

            FuncKey[0] = tbxF1;
            FuncKey[1] = tbxF2;
            FuncKey[2] = tbxF3;
            FuncKey[3] = tbxF4;
            FuncKey[4] = tbxF5;
            FuncKey[5] = tbxF6;
            FuncKey[6] = tbxF7;
            FuncKey[7] = tbxF8;
            FuncKey[8] = tbxF9;
            FuncKey[9] = tbxF10;

            for (int i = 0; i < FuncKey.Length; i++)
            {
                FuncKey[i].Text = FKey.fkey[i].Replace("\n", "\\n");
                FuncKey[i].Tag = (int)i;
                FuncKey[i].TextChanged += new System.EventHandler(FuncKey_TextChanged);
                FuncKey[i].Click += new System.EventHandler(FuncKey_Click);
                FuncKey[i].Leave += new System.EventHandler(FuncKey_Leave);
            }

            //仮想スクリーンの初期化
            for (int y = 0; y < Scrn.Height; y++)
            {
                for (int x = 0; x < Scrn.Width; x++)
                {
                    Scrn.Vram[x, y] = 0;
                }
            }
        }

        private void FuncKey_Leave(object sender, EventArgs e)
        {
            HookEnable = true;
        }

        private void FuncKey_Click(object sender, EventArgs e)
        {
            HookEnable = false;
        }

        /// <summary>
        /// ファンクションキー内容変更割り込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FuncKey_TextChanged(object sender, EventArgs e)
        {
            int num = (int)(((TextBox)sender).Tag);
            FKey.fkey[num] = FuncKey[num].Text;
        }

        /// <summary>
        /// フォームを閉じます
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //モニタ処理のスレッドを停止させる
            bkgSerialPrint.CancelAsync();

            // there's no harm to call Uninstall method repeatedly even if hooks aren't installed
            keyboardHook.Uninstall();

            for (int i = 0; i < FuncKey.Length; i++)
            {
                FuncKey[i].TextChanged -= new System.EventHandler(FuncKey_TextChanged);
                FuncKey[i].Click -= new System.EventHandler(FuncKey_Click);
                FuncKey[i].Leave -= new System.EventHandler(FuncKey_Leave);
            }

            //シリアル接続を切ります
            SerialTool.CommPortClose();
        }

        /// <summary>
        /// フォームが閉じられるときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //設定値を保存します
            WriteAppDataLocalData();
        }

        /// <summary>
        /// フォームからフォーカスが外れたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            HookEnable = true;
        }
        #endregion

        #region  ///        ボタンのクリック処理          ///
        /// <summary>
        /// 接続ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(cmbSerialPort.SelectedItem.ToString());
            if (SerialTool.CommPortIni(cmbSerialPort.SelectedItem.ToString(), SerialTool.Baurate[cmbBaurate.SelectedIndex]) == true)
            {
                cmbSerialPort.Enabled = false;
                cmbBaurate.Enabled = false;
                btnConnect.Enabled = false;
                btnConnectCut.Enabled = true;
                //gbxKanaRoma.Enabled = true;
                //tbxKeyChara.Enabled = true;

                tbxKeyChara.Focus();
                MoveBottom();
            }
        }

        /// <summary>
        /// 切断ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnectCut_Click(object sender, EventArgs e)
        {
            SerialTool.CommPortClose();
            cmbSerialPort.Enabled = true;
            cmbBaurate.Enabled = true;
            btnConnect.Enabled = true;
            btnConnectCut.Enabled = false;
            //gbxKanaRoma.Enabled = false;
            //tbxKeyChara.Enabled = false;
        }

        /// <summary>
        /// インサートキーが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIns_Click(object sender, EventArgs e)
        {
            if (PressBlock == true)
            {
                PressBlock = false;
                return;
            }

            SerialTool.SendComm(17);
        }

        /// <summary>
        /// 貼り付けキーが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPaste_Click(object sender, EventArgs e)
        {
            IDataObject data = Clipboard.GetDataObject();

            if (data.GetDataPresent(DataFormats.Text))
            {
                string clp = (string)data.GetData(DataFormats.Text);

                string[] clps = clp.Replace("\r\n", "\n").Split('\n');

                for (int i = 0; i < clps.Length; i++)
                {
                    if (i == clps.Length - 1)
                    {
                        TextUpdate(clps[i]);
                        SendText(clps[i]);         //文字列を送信します
                    }
                    else
                    {
                        TextUpdate(clps[i] + "\r\n");
                        SendText(clps[i] + "\n");         //文字列を送信します
                    }
                    btnConnectCut.Focus();
                }

                tbxKeyChara.Focus();

                //TextUpdate(clp);
                //SendText(clp);      //文字列を送信します
            }
        }

        /// <summary>
        /// ファイル保存ボタンが押された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (chkMonitor.Checked == true)
            {
                //画面を消去します
                CLS_Command();

                //モニタが使えない状態ということをセットする
                Scrn.IsReady = false;
            }

            //シリアル受信メソッドを切り替えます
            SerialTool.ChangeDataReceivedMethod(1);

            GetForm form = new GetForm();

            form.StartPosition = FormStartPosition.CenterParent;

            //オーナーウィンドウにthisを指定する
            form.ShowDialog(this);

            //フォームが必要なくなったところで、Disposeを呼び出す
            form.Dispose();

            tbxKeyChara.Focus();

            //シリアル受信メソッドを切り替えます
            SerialTool.ChangeDataReceivedMethod(0);

            if (chkMonitor.Checked == true)
            {
                //モニタが使えない状態ということをセットする
                Scrn.IsReady = true;

                //画面を消去します
                CLS_Command();
            }
        }

        /// <summary>
        /// 接続ボタンのEnableが変化したときのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_EnabledChanged(object sender, EventArgs e)
        {
            if (btnConnect.Enabled == false)
            {
                chkEchoBak.Enabled = true;
                chkSoftKeyBoard.Enabled = true;
                if (chkSoftKeyBoard.Checked == true)
                {
                    if (SoftKBForm != null)
                    {
                        SoftKBForm.Visible = true;
                    }
                }

                gbxKanaRoma.Enabled = true;
                gbxBtn.Enabled = true;
                tbxKeyChara.Enabled = true;

                ////モニタ画面を表示します
                //chkMonitor.Checked = true;
            }
            else
            {
                chkEchoBak.Enabled = false;
                chkSoftKeyBoard.Enabled = false;

                if (chkSoftKeyBoard.Checked == true)
                {
                    if (SoftKBForm != null)
                    {
                        SoftKBForm.Visible = false;
                    }
                }

                gbxKanaRoma.Enabled = false;
                gbxBtn.Enabled = false;
                tbxKeyChara.Enabled = false;
            }
        }

        /// <summary>
        /// ソフトキーボードが選択されたときのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSoftKeyBoard_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSoftKeyBoard.Checked == true)
            {
                //ソフトキーボードを開きます
                SetSoftKeyBoardForm();
            }
            else
            {
                //ソフトキーボードを閉じます
                SoftKeyBoard_Closed(null, null);
            }
        }

        /// <summary>
        /// モニタが選択されたときのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkMonitor_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMonitor.Checked == true)
            {
                if (BlnBkgSerialPrint == false)
                {
                    //モニタデータ処理スレッド動作開始
                    bkgSerialPrint.RunWorkerAsync();
                }

                //モニタを表示します
                MonitorLoad();
            }
            else
            {
                //モニタを閉じます
                Monitor_Closed(null, null);

                if (BlnBkgSerialPrint == true)
                {
                    //モニタ処理のスレッドを停止させる
                    bkgSerialPrint.CancelAsync();
                }
            }
        }

        /// <summary>
        /// ソフトキーボードを開きます
        /// </summary>
        private void SetSoftKeyBoardForm()
        {
            if (SoftKBForm != null)
            {
                return;
            }

            //フォームのインスタンスを作ります
            SoftKBForm = new SoftKeyBoard(this.Location.X, this.Location.Y, this.Width, this.Height);

            SoftKBForm.StartPosition = FormStartPosition.CenterParent;

            ////表示フォームを所有する。これでメインフォームの裏に表示フォームは回ることができない。
            //this.AddOwnedForm(form);

            //表示フォームが閉じたときのイベントを生成する
            SoftKBForm.Closed += new EventHandler(SoftKeyBoard_Closed);

            //表示フォームをモードレスで表示する
            SoftKBForm.Show();
        }

        /// <summary>
        /// ソフトキーボードが閉じられたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoftKeyBoard_Closed(object sender, EventArgs e)
        {
            if (SoftKBForm == null) { return; }


            //ViewForm form = (ViewForm)sender;

            //表示フォームが閉じたときのイベントを削除する
            SoftKBForm.Closed -= new EventHandler(SoftKeyBoard_Closed);

            //フォームが必要なくなったところで、Disposeを呼び出す
            SoftKBForm.Dispose();
            SoftKBForm = null;

            chkSoftKeyBoard.Checked = false;
        }

        /// <summary>
        /// ボタンモードの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBTNMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBTNMode.Checked == true)
            {
                btnIns.Enabled = false;
                chkSoftKeyBoard.Enabled = false;
                gbxKanaRoma.Enabled = false;
                gbxSerial.Enabled = false;
                gbxFunctionKey.Enabled = false;
            }
            else
            {
                btnIns.Enabled = true;
                chkSoftKeyBoard.Enabled = true;
                gbxKanaRoma.Enabled = true;
                gbxSerial.Enabled = true;
                gbxFunctionKey.Enabled = true;
            }
        }

        /// <summary>
        /// モニタを表示します
        /// </summary>
        private void MonitorLoad()
        {
            if (Monitor != null)
            {
                return;
            }

            //フォームのインスタンスを作ります
            Monitor = new MonitorForm(this.Location.X, this.Location.Y, this.Width, this.Height);

            Monitor.StartPosition = FormStartPosition.CenterParent;

            ////表示フォームを所有する。これでメインフォームの裏に表示フォームは回ることができない。
            //this.AddOwnedForm(form);

            //表示フォームが閉じたときのイベントを生成する
            Monitor.Closed += new EventHandler(Monitor_Closed);

            //表示フォームをモードレスで表示する
            Monitor.Show();
        }

        /// <summary>
        /// モニタを閉じます
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Monitor_Closed(object sender, EventArgs e)
        {
            if (Monitor == null) { return; }

            //スクリーンが使えない状態ということをセットする
            Scrn.IsReady = false;

            //表示フォームが閉じたときのイベントを削除する
            Monitor.Closed -= new EventHandler(Monitor_Closed);

            //フォームが必要なくなったところで、Disposeを呼び出す
            Monitor.Dispose();
            Monitor = null;

            chkMonitor.Checked = false;
        }

        #endregion

        #region  ///        キーボードのキーが押された処理          ///
        private static bool PressBlock = false;

        /// <summary>
        /// かな文字を変換出力します
        /// </summary>
        /// <param name="ekeychar"></param>
        private void KanaChange(char ekeychar)
        {
            if (KanaKey.KanaMode == 0)
            {
                KanaChara kc = null;
                if (KanaKey.KanaRO > 0)
                {
                    kc = KanaKey.getKanaChara((char)KanaKey.KanaRO);
                    KanaKey.KanaRO = 0;
                }
                else
                {
                    kc = KanaKey.getKanaChara(ekeychar);
                }
                SerialTool.SendComm((byte)kc.code);
                TextUpdate(kc.kana);
            }
        }

        /// <summary>
        /// 入力文字の更新
        /// </summary>
        /// <param name="str"></param>
        private void TextUpdate(string str)
        {
            if (chkEchoBak.Checked == false)
            {
                return;
            }

            if (tbxKeyChara.Text.Length > 30000)
            {
                string txt = tbxKeyChara.Text.Substring(tbxKeyChara.Text.Length - 10000) + str;
                tbxKeyChara.Clear();
                tbxKeyChara.AppendText(txt);
            }
            else
            {
                tbxKeyChara.AppendText(str);
            }

            ////カーソルを一番下に移動します
            //MoveBottom();
        }

        /// <summary>
        /// カーソルを一番下に移動します
        /// </summary>
        private void MoveBottom()
        {
            //カレット位置を末尾に移動
            tbxKeyChara.SelectionStart = tbxKeyChara.Text.Length;
            ////テキストボックスにフォーカスを移動
            //tbxKeyChara.Focus();
            //カレット位置までスクロール
            tbxKeyChara.ScrollToCaret();
        }

        void keyboardHook_KeyUp(RamGecTools.KeyboardHook.VKeys key)
        {
            if (SerialTool.IsCommPort() == false) { return; }

            if (rdiActiveOnly.Checked == true && Form.ActiveForm != this && MonitorForm.BlnMonitorActive == false) { return; }

            switch (key)
            {
                case KeyboardHook.VKeys.LSHIFT: //左のシフトキー
                case KeyboardHook.VKeys.RSHIFT: //右のシフトキー
                    Keyboards.ShiftKey = false;
                    break;

                case KeyboardHook.VKeys.LCONTROL:   //左のコントロールキー
                case KeyboardHook.VKeys.RCONTROL:   //右のコントロールキー
                    Keyboards.CtrlKey = false;
                    break;
            }

            if (chkBTNMode.Checked == true)
            {
                switch (key)
                {
                    case KeyboardHook.VKeys.LEFT:    //カーソル左
                        BtnMode(28, false);
                        break;

                    case KeyboardHook.VKeys.UP:    //カーソル上
                        BtnMode(30, false);
                        break;

                    case KeyboardHook.VKeys.RIGHT:    //カーソル右
                        BtnMode(29, false);
                        break;

                    case KeyboardHook.VKeys.DOWN:    //カーソル下
                        BtnMode(31, false);
                        break;

                    case KeyboardHook.VKeys.SPACE:  //スペース
                        BtnMode(32, false);
                        break;
                }
            }

            //Debug.Write("Up:");
            //Debug.WriteLine(key.ToString());
        }

        void keyboardHook_KeyDown(RamGecTools.KeyboardHook.VKeys key)
        {
            if (SerialTool.IsCommPort() == false) { return; }

            //Debug.Write("Down:");
            //Debug.WriteLine(key.ToString());

            if (Keyboards.CtrlKey == true) { return; }  //コントロールキーが押されているときは処理しない

            if (rdiActiveOnly.Checked == true && Form.ActiveForm != this && MonitorForm.BlnMonitorActive == false) { return; }

            //this.Activate();
            //tbxKeyChara.Focus();

            if ((int)key == 240 || (int)key == 242)
            {
                SerialTool.SendComm((byte)IchigoJamKey.VKeys.KANA);
                PressBlock = true;
                if (rdiAlpha.Checked == true)
                {
                    RomajiIMEMode = ImeMode.Alpha;
                    rdiKana.Checked = true;
                }
                else if (rdiKana.Checked == true)
                {
                    rdiAlpha.Checked = true;
                }
            }

            if (HookEnable == false) { return; }

            switch (key)
            {
                case KeyboardHook.VKeys.BACK:    //BS
                    SerialTool.SendComm((byte)IchigoJamKey.VKeys.BS);
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    break;

                case KeyboardHook.VKeys.RETURN:    //改行
                    if (Keyboards.ShiftKey == true)
                    {
                        SerialTool.SendComm((byte)IchigoJamKey.VKeys.S_ENTER);
                    }
                    else
                    {
                        SerialTool.SendComm((byte)IchigoJamKey.VKeys.ENTER);
                    }
                    PressBlock = true;
                    TextUpdate("\r\n");
                    break;

                case KeyboardHook.VKeys.ESCAPE:    //ESC
                    SerialTool.SendComm((byte)IchigoJamKey.VKeys.ESC);
                    PressBlock = true;
                    break;

                case KeyboardHook.VKeys.PRIOR:    //Page UP
                    SerialTool.SendComm((byte)IchigoJamKey.VKeys.P_UP);
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    break;

                case KeyboardHook.VKeys.NEXT:    //Page DOWN
                    SerialTool.SendComm((byte)IchigoJamKey.VKeys.P_DOWN);
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    break;

                case KeyboardHook.VKeys.END:    //END
                    SerialTool.SendComm((byte)IchigoJamKey.VKeys.END);
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    break;

                case KeyboardHook.VKeys.HOME:    //HOME
                    SerialTool.SendComm((byte)IchigoJamKey.VKeys.HOME);
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    break;

                case KeyboardHook.VKeys.LEFT:    //カーソル左
                    if (chkBTNMode.Checked == true)
                    {
                        BtnMode((byte)IchigoJamKey.VKeys.LEFT, true);
                    }
                    else
                    {
                        SerialTool.SendComm((byte)IchigoJamKey.VKeys.LEFT);
                    }
                    PressBlock = true;
                    break;

                case KeyboardHook.VKeys.UP:    //カーソル上
                    if (chkBTNMode.Checked == true)
                    {
                        BtnMode((byte)IchigoJamKey.VKeys.UP, true);
                    }
                    else
                    {
                        SerialTool.SendComm((byte)IchigoJamKey.VKeys.UP);
                    }
                    PressBlock = true;
                    break;

                case KeyboardHook.VKeys.RIGHT:    //カーソル右
                    if (chkBTNMode.Checked == true)
                    {
                        BtnMode((byte)IchigoJamKey.VKeys.RIGHT, true);
                    }
                    else
                    {
                        SerialTool.SendComm((byte)IchigoJamKey.VKeys.RIGHT);
                    }
                    PressBlock = true;
                    break;

                case KeyboardHook.VKeys.DOWN:    //カーソル下
                    if (chkBTNMode.Checked == true)
                    {
                        BtnMode((byte)IchigoJamKey.VKeys.DOWN, true);
                    }
                    else
                    {
                        SerialTool.SendComm((byte)IchigoJamKey.VKeys.DOWN);
                    }
                    PressBlock = true;
                    break;

                case KeyboardHook.VKeys.SPACE:    //スペース
                    if (chkBTNMode.Checked == true)
                    {
                        BtnMode((byte)IchigoJamKey.VKeys.SPACE, true);
                    }
                    else
                    {
                        if (Keyboards.ShiftKey == true)
                        {
                            SerialTool.SendComm((byte)IchigoJamKey.VKeys.I_SPACE);
                        }
                        else
                        {
                            SerialTool.SendComm((byte)IchigoJamKey.VKeys.SPACE);
                        }
                    }
                    PressBlock = true;
                    break;

                case KeyboardHook.VKeys.INSERT:    //挿入
                    SerialTool.SendComm((byte)IchigoJamKey.VKeys.INSERT);
                    PressBlock = true;
                    break;

                case KeyboardHook.VKeys.RMENU:      //シフト+右ALTで挿入
                    if (Keyboards.ShiftKey == true)
                    {
                        SerialTool.SendComm((byte)IchigoJamKey.VKeys.INSERT);
                        PressBlock = true;
                    }
                    break;

                case KeyboardHook.VKeys.DELETE:    //Delete
                    if (Keyboards.ShiftKey == true)
                    {
                        SerialTool.SendComm((byte)IchigoJamKey.VKeys.L_DELETE);
                    }
                    else
                    {
                        SerialTool.SendComm((byte)IchigoJamKey.VKeys.DELETE);
                    }
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    break;

                case KeyboardHook.VKeys.TAB:    //TAB
                    SerialTool.SendComm((byte)IchigoJamKey.VKeys.TAB);
                    PressBlock = true;
                    ////カーソルを一番下に移動します
                    //MoveBottom();
                    break;

                case KeyboardHook.VKeys.LSHIFT: //左のシフトキー
                case KeyboardHook.VKeys.RSHIFT: //右のシフトキー
                    PressBlock = true;
                    Keyboards.ShiftKey = true;
                    break;

                case KeyboardHook.VKeys.LCONTROL: //左のコントロール
                case KeyboardHook.VKeys.RCONTROL: //右のコントロール
                    PressBlock = true;
                    Keyboards.CtrlKey = true;
                    break;

                case KeyboardHook.VKeys.F1:     //F1
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    TextUpdate(FKey.fkey[0].Replace("\\n", "\r\n"));
                    SendText(FKey.fkey[0].Replace("\\n", "\n"));
                    break;

                case KeyboardHook.VKeys.F2:    //F2
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    TextUpdate(FKey.fkey[1].Replace("\\n", "\r\n"));
                    SendText(FKey.fkey[1].Replace("\\n", "\n"));
                    break;

                case KeyboardHook.VKeys.F3:    //F3
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    TextUpdate(FKey.fkey[2].Replace("\\n", "\r\n"));
                    SendText(FKey.fkey[2].Replace("\\n", "\n"));
                    break;

                case KeyboardHook.VKeys.F4:   //F4
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    TextUpdate(FKey.fkey[3].Replace("\\n", "\r\n"));
                    SendText(FKey.fkey[3].Replace("\\n", "\n"));
                    break;

                case KeyboardHook.VKeys.F5:   //F5
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    TextUpdate(FKey.fkey[4].Replace("\\n", "\r\n"));
                    SendText(FKey.fkey[4].Replace("\\n", "\n"));
                    break;

                case KeyboardHook.VKeys.F6:   //F6
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    TextUpdate(FKey.fkey[5].Replace("\\n", "\r\n"));
                    SendText(FKey.fkey[5].Replace("\\n", "\n"));
                    break;

                case KeyboardHook.VKeys.F7:   //F7
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    TextUpdate(FKey.fkey[6].Replace("\\n", "\r\n"));
                    SendText(FKey.fkey[6].Replace("\\n", "\n"));
                    break;

                case KeyboardHook.VKeys.F8:   //F8
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    TextUpdate(FKey.fkey[7].Replace("\\n", "\r\n"));
                    SendText(FKey.fkey[7].Replace("\\n", "\n"));
                    break;

                case KeyboardHook.VKeys.F9:   //F9
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    TextUpdate(FKey.fkey[8].Replace("\\n", "\r\n"));
                    SendText(FKey.fkey[8].Replace("\\n", "\n"));
                    break;

                case KeyboardHook.VKeys.F10:   //F10
                    PressBlock = true;
                    //カーソルを一番下に移動します
                    MoveBottom();
                    TextUpdate(FKey.fkey[9].Replace("\\n", "\r\n"));
                    SendText(FKey.fkey[9].Replace("\\n", "\n"));
                    break;

                case KeyboardHook.VKeys.KEY_0:    //カタカナの"ヲ"
                    //かな文字モードのとき
                    if (KanaKey.EisuuInputFlg == false)
                    {
                        if (Keyboards.ShiftKey == true)
                        {
                            KanaKey.KanaRO = 166;
                            KanaChange((char)KanaKey.KanaRO);
                        }
                    }
                    break;

                case KeyboardHook.VKeys.OEM_5:      //半角かなの'ｰ'
                    KanaKey.KanaRO = 220;
                    break;

                case KeyboardHook.VKeys.OEM_102:    //半角かなの'ﾛ'
                    KanaKey.KanaRO = 226;
                    break;
            }

            if (PressBlock == true)
            {
                PressBlock = false;
                return;
            }

            //キーコードを取得しています
            //char ekeychar = (char)key;
            char ekeychar = KanaKey.getEisuChar((char)key, Keyboards.ShiftKey);

            if (KanaKey.EisuuInputFlg == false)
            {
                KanaChange(ekeychar);
            }
            else
            {
                SerialTool.SendComm((byte)ekeychar);
                TextUpdate(ekeychar.ToString());
            }
        }

        /// <summary>
        /// ボタンモードの処理を行います
        /// </summary>
        private void BtnMode(byte code, bool udflg)
        {
            if (udflg == true)
            {
                if (BtnModeCnt == 0)
                {
                    SerialTool.SendComm(code);
                    BtnModeTime = DateTime.Now;
                    BtnModeCnt = 1;
                }
            }
            else
            {
                TimeSpan diff = DateTime.Now - BtnModeTime;
                if (BtnModeCnt > 1 || diff.Milliseconds < 120)
                {
                    SerialTool.SendComm(code);
                    BtnModeCnt = 0;
                }
                else
                {
                    BtnModeCnt = 2;
                }
            }
        }

        #endregion

        #region  ///        タイマーの処理          ///
        /// <summary>
        /// 受信タイマ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timRecv_Tick(object sender, EventArgs e)
        {
            timRecv.Enabled = false;

            if (SerialTool.enmReceiveStats == ReceiveStats.NewData)
            {
                SerialTool.enmReceiveStats = ReceiveStats.MainformProcessed;

                if (tbxKeyChara.Text.Length > 30000)
                {
                    string txt = tbxKeyChara.Text.Substring(tbxKeyChara.Text.Length - 10000) + SerialTool.RecvDataText;
                    tbxKeyChara.Clear();
                    tbxKeyChara.AppendText(txt);
                }
                else
                {
                    tbxKeyChara.AppendText(SerialTool.RecvDataText);
                }

                //if (tbxKeyChara.Text.Length > 30000)
                //{
                //    tbxKeyChara.Text = tbxKeyChara.Text.Substring(tbxKeyChara.Text.Length - 30000);
                //}

                ////カレット位置を末尾に移動
                //tbxKeyChara.SelectionStart = tbxKeyChara.Text.Length;
                ////テキストボックスにフォーカスを移動
                //tbxKeyChara.Focus();
                ////カレット位置までスクロール
                //tbxKeyChara.ScrollToCaret();

                SerialTool.enmReceiveStats = ReceiveStats.MainformFinish;
            }
            timRecv.Enabled = true;
        }

        #endregion

        #region  ///        コンボボックス/ラジオボタンの処理          ///
        /// <summary>
        /// カナ選択時の入力モード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbKanaRoma_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdiKana.Checked == true)
            {
                if (cmbKanaRoma.SelectedIndex == 1)
                {
                    //ローマ字入力ウィンドウを出します
                    StartRomajiInput(ImeMode.On);
                }
            }

            KanaKey.KanaMode = cmbKanaRoma.SelectedIndex;
        }

        /// <summary>
        /// 英数/かなの選択ラジオボタンが変わった
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdiAlpha_CheckedChanged(object sender, EventArgs e)
        {
            KanaKey.EisuuInputFlg = rdiAlpha.Checked;

            if (KanaKey.EisuuInputFlg == false && KanaKey.KanaMode == 1)
            {
                //ローマ字入力ウィンドウを出します
                StartRomajiInput(RomajiIMEMode);
                RomajiIMEMode = ImeMode.On;
            }
            else if (KanaKey.EisuuInputFlg == true && Textinputform != null)
            {
                //ローマ字入力ウィンドウを閉じます
                TextInputForm_Closed(null, null);
            }
        }

        /// <summary>
        /// ローマ字入力ウィンドウを出します
        /// </summary>
        private void StartRomajiInput(ImeMode ime)
        {
            if (this.Textinputform != null)
            {
                return;
            }

            cmbKanaRoma.Enabled = false;

            //フォームのインスタンスを作ります
            this.Textinputform = new TextInputForm(this.Location.X, this.Location.Y, this.Width, this.Height, ime);

            //form.StartPosition = FormStartPosition.CenterParent;

            //ローマ字入力ウィンドウを所有する。これでメインフォームの裏に表示フォームは回ることができない。
            this.AddOwnedForm(this.Textinputform);

            //ローマ字入力ウィンドウが閉じたときのイベントを生成する
            this.Textinputform.Closed += new EventHandler(TextInputForm_Closed);

            //ローマ字入力ウィンドウをモードレスで表示する
            this.Textinputform.Show();

            //キーボードフック処理を利かなくします
            HookEnable = false;
        }

        /// <summary>
        /// ローマ字入力ウィンドウが閉じられたときの処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextInputForm_Closed(object sender, EventArgs e)
        {
            //ローマ字入力ウィンドウが閉じたときのイベントを削除する
            this.Textinputform.Closed -= new EventHandler(TextInputForm_Closed);

            //フォームが必要なくなったところで、Disposeを呼び出す
            this.Textinputform.Dispose();
            this.Textinputform = null;

            cmbKanaRoma.Enabled = true;
            rdiAlpha.Checked = true;

            if (TextInputForm.RomajiText != "" && TextInputForm.RomajiOutputFlg == true)
            {
                //ひらがなをカタカナに変換して全角を半角に変換して出力する
                string str = Strings.StrConv(TextInputForm.RomajiText, VbStrConv.Katakana | VbStrConv.Narrow, 0x411);   //LocalID 0x411は日本語のID

                TextUpdate(str);    //文字列を表示します

                SendText(str);      //文字列を送信します

                TextInputForm.RomajiText = "";
                TextInputForm.RomajiOutputFlg = false;
            }

            //キーボードフック処理を利くようにします
            HookEnable = true;
        }

        /// <summary>
        /// 文字列を送信します
        /// </summary>
        /// <param name="str"></param>
        private static void SendText(string str)
        {
            byte[] sjis = Encoding.GetEncoding(932).GetBytes(str);
            foreach (byte cha in sjis)
            {
                SerialTool.SendComm((byte)cha);
                Thread.Sleep(25);
                Application.DoEvents();
                if (SerialTool.IsCommPort() == false) { break; }
            }
        }
        #endregion

        #region  ///        ドラッグ・ドロップの処理          ///
        private void tbxKeyChara_DragDrop(object sender, DragEventArgs e)
        {
            // ドラッグ＆ドロップされたファイル
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            SendFileData(files[0]);
        }

        private void tbxKeyChara_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        /// <summary>
        /// プログラムを送信します
        /// </summary>
        /// <param name="filename"></param>
        private void SendFileData(string filename)
        {
            //ファイルであるか
            if (!File.Exists(filename))
            {
                MessageBox.Show(ValiableList.errFilename, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (chkMonitor.Checked == true)
            {
                //モニタが使えない状態ということをセットする
                Scrn.IsReady = false;
            }

            try
            {
                HookEnable = false;
                string txtData = "";
                byte[] data = File.ReadAllBytes(filename); // 読み込み
                byte[] byteData = new byte[2];

                //画面を消去します
                CLS_Command();

                //NEWコマンドの送信
                byte[] cmd = Encoding.ASCII.GetBytes("NEW\n");
                for (int k = 0; k < cmd.Length; k++)
                {
                    SerialTool.SendComm(cmd[k]);
                    for (int i = 0; i < 4; i++)
                    {
                        Thread.Sleep(5);
                        Application.DoEvents();
                    }
                }

                for (int i = 0; i < data.Length; i++)
                {
                    byteData[0] = data[i];

                    //Shift JISとして文字列に変換
                    txtData = Encoding.GetEncoding(932).GetString(byteData);
                    TextUpdate(txtData);

                    if (data[i] != 0x0D)
                    {
                        SerialTool.SendComm(data[i]);
                    }

                    btnConnectCut.Focus();
                    Thread.Sleep(25);
                    Application.DoEvents();

                    if (SerialTool.IsCommPort() == false) { break; }
                }
                HookEnable = true;

            }
            catch (Exception e)
            {
                MessageBox.Show(ValiableList.errFileRead, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine(e.Message);
            }

            if (chkMonitor.Checked == true)
            {
                //モニタが使える状態にする
                Scrn.IsReady = true;

                //画面を消去します
                CLS_Command();

                //LISTコマンドの送信
                byte[] cmd = Encoding.ASCII.GetBytes("LIST\n");
                for (int k = 0; k < cmd.Length; k++)
                {
                    SerialTool.SendComm(cmd[k]);
                    for (int i = 0; i < 4; i++)
                    {
                        Thread.Sleep(5);
                        Application.DoEvents();
                    }
                }
            }
            
            tbxKeyChara.Focus();
        }

        /// <summary>
        /// 画面を消去する CLSコマンドです。
        /// </summary>
        private static void CLS_Command()
        {
            //ESCキーコードを送ります
            SerialTool.SendComm((byte)IchigoJamKey.VKeys.ESC);
            for (int i = 0; i < 120; i++)
            {
                Thread.Sleep(2);
                Application.DoEvents();
            }

            //カーソルを画面の一番上に持っていく
            SerialTool.SendComm((byte)IchigoJamKey.VKeys.P_UP);
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(5);
                Application.DoEvents();
            }

            //カーソル以下を全て削除します
            SerialTool.SendComm((byte)IchigoJamKey.VKeys.D_DELETE);
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(5);
                Application.DoEvents();
            }
        }
        #endregion

        #region  ///        仮想画面関連の処理          ///
        /// <summary>
        /// モニタ処理のスレッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bkgSerialPrint_DoWork(object sender, DoWorkEventArgs e)
        {
            int dat;
            BackgroundWorker bg = (BackgroundWorker)sender;
            BlnBkgSerialPrint = true;

            Debug.WriteLine("bgwSerialPrintスレッドのスタート！");

            while (!bg.CancellationPending)
            {
                if (SerialTool.SerialDataQueue.Count > 0)
                {
                    SerialTool.QueueLock.WaitOne();
                    {
                        dat = (int)SerialTool.SerialDataQueue.Dequeue();
                    }
                    SerialTool.QueueLock.Set();

                    bg.ReportProgress(100, (int)dat);
                }
                //Application.DoEvents();
            }

            Debug.WriteLine("bgwSerialPrintスレッドの終了！");
        }

        /// <summary>
        /// モニタのバックグラウンドスレッドから呼び出されるスレッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bkgSerialPrint_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (Scrn.IsReady == true)
            {
                Monitor.Print((int)e.UserState);
            }
        }

        /// <summary>
        /// モニタのバックグラウンドスレッドが終了したときに呼び出されるスレッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bkgSerialPrint_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BlnBkgSerialPrint = false;
        }
        #endregion
    }
}
namespace IchigoJamKeyBoard
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tbxKeyChara = new System.Windows.Forms.TextBox();
            this.btnConnectCut = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblSerial = new System.Windows.Forms.Label();
            this.cmbSerialPort = new System.Windows.Forms.ComboBox();
            this.gbxSerial = new System.Windows.Forms.GroupBox();
            this.chkEchoBak = new System.Windows.Forms.CheckBox();
            this.lblBaurate = new System.Windows.Forms.Label();
            this.cmbBaurate = new System.Windows.Forms.ComboBox();
            this.chkSoftKeyBoard = new System.Windows.Forms.CheckBox();
            this.rdiAlpha = new System.Windows.Forms.RadioButton();
            this.rdiKana = new System.Windows.Forms.RadioButton();
            this.gbxKanaRoma = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.lblKanamode = new System.Windows.Forms.Label();
            this.cmbKanaRoma = new System.Windows.Forms.ComboBox();
            this.timRecv = new System.Windows.Forms.Timer(this.components);
            this.gbxFunctionKey = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxF10 = new System.Windows.Forms.TextBox();
            this.tbxF9 = new System.Windows.Forms.TextBox();
            this.tbxF8 = new System.Windows.Forms.TextBox();
            this.lblF8 = new System.Windows.Forms.Label();
            this.tbxF7 = new System.Windows.Forms.TextBox();
            this.lblF7 = new System.Windows.Forms.Label();
            this.tbxF6 = new System.Windows.Forms.TextBox();
            this.lblF6 = new System.Windows.Forms.Label();
            this.tbxF5 = new System.Windows.Forms.TextBox();
            this.lblF5 = new System.Windows.Forms.Label();
            this.tbxF4 = new System.Windows.Forms.TextBox();
            this.lblF4 = new System.Windows.Forms.Label();
            this.tbxF3 = new System.Windows.Forms.TextBox();
            this.lblF3 = new System.Windows.Forms.Label();
            this.tbxF2 = new System.Windows.Forms.TextBox();
            this.lblF2 = new System.Windows.Forms.Label();
            this.tbxF1 = new System.Windows.Forms.TextBox();
            this.lblF1 = new System.Windows.Forms.Label();
            this.gbxBtn = new System.Windows.Forms.GroupBox();
            this.chkMonitor = new System.Windows.Forms.CheckBox();
            this.gbxKeyAccept = new System.Windows.Forms.GroupBox();
            this.rdiActiveOnly = new System.Windows.Forms.RadioButton();
            this.rdiFullTime = new System.Windows.Forms.RadioButton();
            this.btnIns = new System.Windows.Forms.Button();
            this.chkBTNMode = new System.Windows.Forms.CheckBox();
            this.bkgSerialPrint = new System.ComponentModel.BackgroundWorker();
            this.gbxSerial.SuspendLayout();
            this.gbxKanaRoma.SuspendLayout();
            this.gbxFunctionKey.SuspendLayout();
            this.gbxBtn.SuspendLayout();
            this.gbxKeyAccept.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxKeyChara
            // 
            this.tbxKeyChara.AllowDrop = true;
            resources.ApplyResources(this.tbxKeyChara, "tbxKeyChara");
            this.tbxKeyChara.Name = "tbxKeyChara";
            this.tbxKeyChara.ReadOnly = true;
            this.tbxKeyChara.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbxKeyChara_DragDrop);
            this.tbxKeyChara.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbxKeyChara_DragEnter);
            // 
            // btnConnectCut
            // 
            resources.ApplyResources(this.btnConnectCut, "btnConnectCut");
            this.btnConnectCut.Name = "btnConnectCut";
            this.btnConnectCut.UseVisualStyleBackColor = true;
            this.btnConnectCut.Click += new System.EventHandler(this.btnConnectCut_Click);
            // 
            // btnConnect
            // 
            resources.ApplyResources(this.btnConnect, "btnConnect");
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.EnabledChanged += new System.EventHandler(this.btnConnect_EnabledChanged);
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblSerial
            // 
            resources.ApplyResources(this.lblSerial, "lblSerial");
            this.lblSerial.Name = "lblSerial";
            // 
            // cmbSerialPort
            // 
            this.cmbSerialPort.FormattingEnabled = true;
            resources.ApplyResources(this.cmbSerialPort, "cmbSerialPort");
            this.cmbSerialPort.Name = "cmbSerialPort";
            // 
            // gbxSerial
            // 
            this.gbxSerial.Controls.Add(this.chkEchoBak);
            this.gbxSerial.Controls.Add(this.lblBaurate);
            this.gbxSerial.Controls.Add(this.cmbBaurate);
            this.gbxSerial.Controls.Add(this.lblSerial);
            this.gbxSerial.Controls.Add(this.btnConnectCut);
            this.gbxSerial.Controls.Add(this.cmbSerialPort);
            this.gbxSerial.Controls.Add(this.btnConnect);
            resources.ApplyResources(this.gbxSerial, "gbxSerial");
            this.gbxSerial.Name = "gbxSerial";
            this.gbxSerial.TabStop = false;
            // 
            // chkEchoBak
            // 
            resources.ApplyResources(this.chkEchoBak, "chkEchoBak");
            this.chkEchoBak.Checked = true;
            this.chkEchoBak.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEchoBak.Name = "chkEchoBak";
            this.chkEchoBak.UseVisualStyleBackColor = true;
            // 
            // lblBaurate
            // 
            resources.ApplyResources(this.lblBaurate, "lblBaurate");
            this.lblBaurate.Name = "lblBaurate";
            // 
            // cmbBaurate
            // 
            this.cmbBaurate.FormattingEnabled = true;
            resources.ApplyResources(this.cmbBaurate, "cmbBaurate");
            this.cmbBaurate.Name = "cmbBaurate";
            // 
            // chkSoftKeyBoard
            // 
            resources.ApplyResources(this.chkSoftKeyBoard, "chkSoftKeyBoard");
            this.chkSoftKeyBoard.Name = "chkSoftKeyBoard";
            this.chkSoftKeyBoard.UseVisualStyleBackColor = true;
            this.chkSoftKeyBoard.CheckedChanged += new System.EventHandler(this.chkSoftKeyBoard_CheckedChanged);
            // 
            // rdiAlpha
            // 
            resources.ApplyResources(this.rdiAlpha, "rdiAlpha");
            this.rdiAlpha.Checked = true;
            this.rdiAlpha.Name = "rdiAlpha";
            this.rdiAlpha.TabStop = true;
            this.rdiAlpha.UseVisualStyleBackColor = true;
            this.rdiAlpha.CheckedChanged += new System.EventHandler(this.rdiAlpha_CheckedChanged);
            // 
            // rdiKana
            // 
            resources.ApplyResources(this.rdiKana, "rdiKana");
            this.rdiKana.Name = "rdiKana";
            this.rdiKana.UseVisualStyleBackColor = true;
            // 
            // gbxKanaRoma
            // 
            this.gbxKanaRoma.Controls.Add(this.btnSave);
            this.gbxKanaRoma.Controls.Add(this.btnPaste);
            this.gbxKanaRoma.Controls.Add(this.lblKanamode);
            this.gbxKanaRoma.Controls.Add(this.cmbKanaRoma);
            this.gbxKanaRoma.Controls.Add(this.rdiAlpha);
            this.gbxKanaRoma.Controls.Add(this.rdiKana);
            resources.ApplyResources(this.gbxKanaRoma, "gbxKanaRoma");
            this.gbxKanaRoma.Name = "gbxKanaRoma";
            this.gbxKanaRoma.TabStop = false;
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPaste
            // 
            resources.ApplyResources(this.btnPaste, "btnPaste");
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // lblKanamode
            // 
            resources.ApplyResources(this.lblKanamode, "lblKanamode");
            this.lblKanamode.Name = "lblKanamode";
            // 
            // cmbKanaRoma
            // 
            this.cmbKanaRoma.FormattingEnabled = true;
            resources.ApplyResources(this.cmbKanaRoma, "cmbKanaRoma");
            this.cmbKanaRoma.Name = "cmbKanaRoma";
            this.cmbKanaRoma.SelectedIndexChanged += new System.EventHandler(this.cmbKanaRoma_SelectedIndexChanged);
            // 
            // timRecv
            // 
            this.timRecv.Enabled = true;
            this.timRecv.Interval = 10;
            this.timRecv.Tick += new System.EventHandler(this.timRecv_Tick);
            // 
            // gbxFunctionKey
            // 
            this.gbxFunctionKey.Controls.Add(this.label2);
            this.gbxFunctionKey.Controls.Add(this.label1);
            this.gbxFunctionKey.Controls.Add(this.tbxF10);
            this.gbxFunctionKey.Controls.Add(this.tbxF9);
            this.gbxFunctionKey.Controls.Add(this.tbxF8);
            this.gbxFunctionKey.Controls.Add(this.lblF8);
            this.gbxFunctionKey.Controls.Add(this.tbxF7);
            this.gbxFunctionKey.Controls.Add(this.lblF7);
            this.gbxFunctionKey.Controls.Add(this.tbxF6);
            this.gbxFunctionKey.Controls.Add(this.lblF6);
            this.gbxFunctionKey.Controls.Add(this.tbxF5);
            this.gbxFunctionKey.Controls.Add(this.lblF5);
            this.gbxFunctionKey.Controls.Add(this.tbxF4);
            this.gbxFunctionKey.Controls.Add(this.lblF4);
            this.gbxFunctionKey.Controls.Add(this.tbxF3);
            this.gbxFunctionKey.Controls.Add(this.lblF3);
            this.gbxFunctionKey.Controls.Add(this.tbxF2);
            this.gbxFunctionKey.Controls.Add(this.lblF2);
            this.gbxFunctionKey.Controls.Add(this.tbxF1);
            this.gbxFunctionKey.Controls.Add(this.lblF1);
            resources.ApplyResources(this.gbxFunctionKey, "gbxFunctionKey");
            this.gbxFunctionKey.Name = "gbxFunctionKey";
            this.gbxFunctionKey.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbxF10
            // 
            resources.ApplyResources(this.tbxF10, "tbxF10");
            this.tbxF10.Name = "tbxF10";
            // 
            // tbxF9
            // 
            resources.ApplyResources(this.tbxF9, "tbxF9");
            this.tbxF9.Name = "tbxF9";
            // 
            // tbxF8
            // 
            resources.ApplyResources(this.tbxF8, "tbxF8");
            this.tbxF8.Name = "tbxF8";
            // 
            // lblF8
            // 
            resources.ApplyResources(this.lblF8, "lblF8");
            this.lblF8.Name = "lblF8";
            // 
            // tbxF7
            // 
            resources.ApplyResources(this.tbxF7, "tbxF7");
            this.tbxF7.Name = "tbxF7";
            // 
            // lblF7
            // 
            resources.ApplyResources(this.lblF7, "lblF7");
            this.lblF7.Name = "lblF7";
            // 
            // tbxF6
            // 
            resources.ApplyResources(this.tbxF6, "tbxF6");
            this.tbxF6.Name = "tbxF6";
            // 
            // lblF6
            // 
            resources.ApplyResources(this.lblF6, "lblF6");
            this.lblF6.Name = "lblF6";
            // 
            // tbxF5
            // 
            resources.ApplyResources(this.tbxF5, "tbxF5");
            this.tbxF5.Name = "tbxF5";
            // 
            // lblF5
            // 
            resources.ApplyResources(this.lblF5, "lblF5");
            this.lblF5.Name = "lblF5";
            // 
            // tbxF4
            // 
            resources.ApplyResources(this.tbxF4, "tbxF4");
            this.tbxF4.Name = "tbxF4";
            // 
            // lblF4
            // 
            resources.ApplyResources(this.lblF4, "lblF4");
            this.lblF4.Name = "lblF4";
            // 
            // tbxF3
            // 
            resources.ApplyResources(this.tbxF3, "tbxF3");
            this.tbxF3.Name = "tbxF3";
            this.tbxF3.Tag = "2";
            // 
            // lblF3
            // 
            resources.ApplyResources(this.lblF3, "lblF3");
            this.lblF3.Name = "lblF3";
            // 
            // tbxF2
            // 
            resources.ApplyResources(this.tbxF2, "tbxF2");
            this.tbxF2.Name = "tbxF2";
            this.tbxF2.Tag = "1";
            // 
            // lblF2
            // 
            resources.ApplyResources(this.lblF2, "lblF2");
            this.lblF2.Name = "lblF2";
            // 
            // tbxF1
            // 
            resources.ApplyResources(this.tbxF1, "tbxF1");
            this.tbxF1.Name = "tbxF1";
            this.tbxF1.Tag = "0";
            // 
            // lblF1
            // 
            resources.ApplyResources(this.lblF1, "lblF1");
            this.lblF1.Name = "lblF1";
            // 
            // gbxBtn
            // 
            this.gbxBtn.Controls.Add(this.chkMonitor);
            this.gbxBtn.Controls.Add(this.gbxKeyAccept);
            this.gbxBtn.Controls.Add(this.btnIns);
            this.gbxBtn.Controls.Add(this.chkSoftKeyBoard);
            this.gbxBtn.Controls.Add(this.chkBTNMode);
            resources.ApplyResources(this.gbxBtn, "gbxBtn");
            this.gbxBtn.Name = "gbxBtn";
            this.gbxBtn.TabStop = false;
            // 
            // chkMonitor
            // 
            resources.ApplyResources(this.chkMonitor, "chkMonitor");
            this.chkMonitor.Name = "chkMonitor";
            this.chkMonitor.UseVisualStyleBackColor = true;
            this.chkMonitor.CheckedChanged += new System.EventHandler(this.chkMonitor_CheckedChanged);
            // 
            // gbxKeyAccept
            // 
            this.gbxKeyAccept.Controls.Add(this.rdiActiveOnly);
            this.gbxKeyAccept.Controls.Add(this.rdiFullTime);
            resources.ApplyResources(this.gbxKeyAccept, "gbxKeyAccept");
            this.gbxKeyAccept.Name = "gbxKeyAccept";
            this.gbxKeyAccept.TabStop = false;
            // 
            // rdiActiveOnly
            // 
            resources.ApplyResources(this.rdiActiveOnly, "rdiActiveOnly");
            this.rdiActiveOnly.Checked = true;
            this.rdiActiveOnly.Name = "rdiActiveOnly";
            this.rdiActiveOnly.TabStop = true;
            this.rdiActiveOnly.UseVisualStyleBackColor = true;
            // 
            // rdiFullTime
            // 
            resources.ApplyResources(this.rdiFullTime, "rdiFullTime");
            this.rdiFullTime.Name = "rdiFullTime";
            this.rdiFullTime.UseVisualStyleBackColor = true;
            // 
            // btnIns
            // 
            resources.ApplyResources(this.btnIns, "btnIns");
            this.btnIns.Name = "btnIns";
            this.btnIns.UseVisualStyleBackColor = true;
            this.btnIns.Click += new System.EventHandler(this.btnIns_Click);
            // 
            // chkBTNMode
            // 
            resources.ApplyResources(this.chkBTNMode, "chkBTNMode");
            this.chkBTNMode.Name = "chkBTNMode";
            this.chkBTNMode.UseVisualStyleBackColor = true;
            this.chkBTNMode.CheckedChanged += new System.EventHandler(this.chkBTNMode_CheckedChanged);
            // 
            // bkgSerialPrint
            // 
            this.bkgSerialPrint.WorkerReportsProgress = true;
            this.bkgSerialPrint.WorkerSupportsCancellation = true;
            this.bkgSerialPrint.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bkgSerialPrint_DoWork);
            this.bkgSerialPrint.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bkgSerialPrint_ProgressChanged);
            this.bkgSerialPrint.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bkgSerialPrint_RunWorkerCompleted);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxBtn);
            this.Controls.Add(this.gbxFunctionKey);
            this.Controls.Add(this.gbxKanaRoma);
            this.Controls.Add(this.gbxSerial);
            this.Controls.Add(this.tbxKeyChara);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.gbxSerial.ResumeLayout(false);
            this.gbxSerial.PerformLayout();
            this.gbxKanaRoma.ResumeLayout(false);
            this.gbxKanaRoma.PerformLayout();
            this.gbxFunctionKey.ResumeLayout(false);
            this.gbxFunctionKey.PerformLayout();
            this.gbxBtn.ResumeLayout(false);
            this.gbxKeyAccept.ResumeLayout(false);
            this.gbxKeyAccept.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxKeyChara;
        private System.Windows.Forms.Button btnConnectCut;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblSerial;
        private System.Windows.Forms.ComboBox cmbSerialPort;
        private System.Windows.Forms.GroupBox gbxSerial;
        private System.Windows.Forms.Label lblBaurate;
        private System.Windows.Forms.ComboBox cmbBaurate;
        private System.Windows.Forms.RadioButton rdiAlpha;
        private System.Windows.Forms.RadioButton rdiKana;
        private System.Windows.Forms.GroupBox gbxKanaRoma;
        private System.Windows.Forms.Timer timRecv;
        private System.Windows.Forms.ComboBox cmbKanaRoma;
        private System.Windows.Forms.Label lblKanamode;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.CheckBox chkEchoBak;
        private System.Windows.Forms.GroupBox gbxFunctionKey;
        private System.Windows.Forms.Label lblF1;
        private System.Windows.Forms.TextBox tbxF1;
        private System.Windows.Forms.TextBox tbxF3;
        private System.Windows.Forms.Label lblF3;
        private System.Windows.Forms.TextBox tbxF2;
        private System.Windows.Forms.Label lblF2;
        private System.Windows.Forms.TextBox tbxF8;
        private System.Windows.Forms.Label lblF8;
        private System.Windows.Forms.TextBox tbxF7;
        private System.Windows.Forms.Label lblF7;
        private System.Windows.Forms.TextBox tbxF6;
        private System.Windows.Forms.Label lblF6;
        private System.Windows.Forms.TextBox tbxF5;
        private System.Windows.Forms.Label lblF5;
        private System.Windows.Forms.TextBox tbxF4;
        private System.Windows.Forms.Label lblF4;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkSoftKeyBoard;
        private System.Windows.Forms.GroupBox gbxBtn;
        private System.Windows.Forms.CheckBox chkBTNMode;
        private System.Windows.Forms.Button btnIns;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxF10;
        private System.Windows.Forms.TextBox tbxF9;
        private System.Windows.Forms.GroupBox gbxKeyAccept;
        private System.Windows.Forms.RadioButton rdiActiveOnly;
        private System.Windows.Forms.RadioButton rdiFullTime;
        private System.Windows.Forms.CheckBox chkMonitor;
        private System.ComponentModel.BackgroundWorker bkgSerialPrint;
    }
}


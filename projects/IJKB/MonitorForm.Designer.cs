namespace IchigoJamKeyBoard
{
    partial class MonitorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitorForm));
            this.pbxMonitor = new System.Windows.Forms.PictureBox();
            this.MnuRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MnuRCGamenSize = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuRC1bai = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuRC2bai = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuRC3bai = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuRC4bai = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuResetScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuOption = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuCursor = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuCurInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuCurOverWrite = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuCC = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuTEST = new System.Windows.Forms.ToolStripMenuItem();
            this.pbx256x192 = new System.Windows.Forms.PictureBox();
            this.pbxIchigoCharas = new System.Windows.Forms.PictureBox();
            this.timRefresh = new System.Windows.Forms.Timer(this.components);
            this.timCursor = new System.Windows.Forms.Timer(this.components);
            this.pnlButton = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.Button();
            this.pnlSpeacer3 = new System.Windows.Forms.Panel();
            this.chkMouse = new System.Windows.Forms.CheckBox();
            this.pnlSpeacer2 = new System.Windows.Forms.Panel();
            this.chkCursor = new System.Windows.Forms.CheckBox();
            this.pnlSpeacer1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMonitor)).BeginInit();
            this.MnuRightClick.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx256x192)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIchigoCharas)).BeginInit();
            this.pnlButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbxMonitor
            // 
            this.pbxMonitor.ContextMenuStrip = this.MnuRightClick;
            resources.ApplyResources(this.pbxMonitor, "pbxMonitor");
            this.pbxMonitor.Name = "pbxMonitor";
            this.pbxMonitor.TabStop = false;
            this.pbxMonitor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbxMonitor_MouseClick);
            // 
            // MnuRightClick
            // 
            this.MnuRightClick.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MnuRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuRCGamenSize,
            this.MnuResetScreen,
            this.MnuOption,
            this.MnuCursor,
            this.MnuCC,
            this.MnuTEST});
            this.MnuRightClick.Name = "MnuRightClick";
            resources.ApplyResources(this.MnuRightClick, "MnuRightClick");
            // 
            // MnuRCGamenSize
            // 
            this.MnuRCGamenSize.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuRC1bai,
            this.MnuRC2bai,
            this.MnuRC3bai,
            this.MnuRC4bai});
            this.MnuRCGamenSize.Name = "MnuRCGamenSize";
            resources.ApplyResources(this.MnuRCGamenSize, "MnuRCGamenSize");
            // 
            // MnuRC1bai
            // 
            this.MnuRC1bai.Name = "MnuRC1bai";
            resources.ApplyResources(this.MnuRC1bai, "MnuRC1bai");
            this.MnuRC1bai.Click += new System.EventHandler(this.MnuRC1bai_Click);
            // 
            // MnuRC2bai
            // 
            this.MnuRC2bai.Name = "MnuRC2bai";
            resources.ApplyResources(this.MnuRC2bai, "MnuRC2bai");
            this.MnuRC2bai.Click += new System.EventHandler(this.MnuRC2bai_Click);
            // 
            // MnuRC3bai
            // 
            this.MnuRC3bai.Name = "MnuRC3bai";
            resources.ApplyResources(this.MnuRC3bai, "MnuRC3bai");
            this.MnuRC3bai.Click += new System.EventHandler(this.MnuRC3bai_Click);
            // 
            // MnuRC4bai
            // 
            this.MnuRC4bai.Name = "MnuRC4bai";
            resources.ApplyResources(this.MnuRC4bai, "MnuRC4bai");
            this.MnuRC4bai.Click += new System.EventHandler(this.MnuRC4bai_Click);
            // 
            // MnuResetScreen
            // 
            this.MnuResetScreen.Name = "MnuResetScreen";
            resources.ApplyResources(this.MnuResetScreen, "MnuResetScreen");
            this.MnuResetScreen.Click += new System.EventHandler(this.MnuResetScreen_Click);
            // 
            // MnuOption
            // 
            this.MnuOption.Name = "MnuOption";
            resources.ApplyResources(this.MnuOption, "MnuOption");
            this.MnuOption.Click += new System.EventHandler(this.MnuOption_Click);
            // 
            // MnuCursor
            // 
            this.MnuCursor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuCurInsert,
            this.MnuCurOverWrite});
            this.MnuCursor.Name = "MnuCursor";
            resources.ApplyResources(this.MnuCursor, "MnuCursor");
            // 
            // MnuCurInsert
            // 
            this.MnuCurInsert.Name = "MnuCurInsert";
            resources.ApplyResources(this.MnuCurInsert, "MnuCurInsert");
            this.MnuCurInsert.Click += new System.EventHandler(this.MnuCurInsert_Click);
            // 
            // MnuCurOverWrite
            // 
            this.MnuCurOverWrite.Name = "MnuCurOverWrite";
            resources.ApplyResources(this.MnuCurOverWrite, "MnuCurOverWrite");
            this.MnuCurOverWrite.Click += new System.EventHandler(this.MnuCurOverWrite_Click);
            // 
            // MnuCC
            // 
            this.MnuCC.Name = "MnuCC";
            resources.ApplyResources(this.MnuCC, "MnuCC");
            this.MnuCC.Click += new System.EventHandler(this.MnuCC_Click);
            // 
            // MnuTEST
            // 
            this.MnuTEST.Name = "MnuTEST";
            resources.ApplyResources(this.MnuTEST, "MnuTEST");
            this.MnuTEST.Click += new System.EventHandler(this.MnuTEST_Click);
            // 
            // pbx256x192
            // 
            resources.ApplyResources(this.pbx256x192, "pbx256x192");
            this.pbx256x192.Name = "pbx256x192";
            this.pbx256x192.TabStop = false;
            // 
            // pbxIchigoCharas
            // 
            resources.ApplyResources(this.pbxIchigoCharas, "pbxIchigoCharas");
            this.pbxIchigoCharas.Name = "pbxIchigoCharas";
            this.pbxIchigoCharas.TabStop = false;
            // 
            // timRefresh
            // 
            this.timRefresh.Tick += new System.EventHandler(this.timRefresh_Tick);
            // 
            // timCursor
            // 
            this.timCursor.Enabled = true;
            this.timCursor.Interval = 250;
            this.timCursor.Tick += new System.EventHandler(this.timCursor_Tick);
            // 
            // pnlButton
            // 
            this.pnlButton.Controls.Add(this.btnReset);
            this.pnlButton.Controls.Add(this.pnlSpeacer3);
            this.pnlButton.Controls.Add(this.chkMouse);
            this.pnlButton.Controls.Add(this.pnlSpeacer2);
            this.pnlButton.Controls.Add(this.chkCursor);
            this.pnlButton.Controls.Add(this.pnlSpeacer1);
            resources.ApplyResources(this.pnlButton, "pnlButton");
            this.pnlButton.Name = "pnlButton";
            // 
            // btnReset
            // 
            resources.ApplyResources(this.btnReset, "btnReset");
            this.btnReset.Name = "btnReset";
            this.btnReset.TabStop = false;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnReset_MouseClick);
            // 
            // pnlSpeacer3
            // 
            resources.ApplyResources(this.pnlSpeacer3, "pnlSpeacer3");
            this.pnlSpeacer3.Name = "pnlSpeacer3";
            // 
            // chkMouse
            // 
            resources.ApplyResources(this.chkMouse, "chkMouse");
            this.chkMouse.Name = "chkMouse";
            this.chkMouse.TabStop = false;
            this.chkMouse.UseVisualStyleBackColor = true;
            // 
            // pnlSpeacer2
            // 
            resources.ApplyResources(this.pnlSpeacer2, "pnlSpeacer2");
            this.pnlSpeacer2.Name = "pnlSpeacer2";
            // 
            // chkCursor
            // 
            resources.ApplyResources(this.chkCursor, "chkCursor");
            this.chkCursor.Checked = true;
            this.chkCursor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCursor.Name = "chkCursor";
            this.chkCursor.TabStop = false;
            this.chkCursor.UseVisualStyleBackColor = true;
            this.chkCursor.CheckedChanged += new System.EventHandler(this.chkCursor_CheckedChanged);
            // 
            // pnlSpeacer1
            // 
            resources.ApplyResources(this.pnlSpeacer1, "pnlSpeacer1");
            this.pnlSpeacer1.Name = "pnlSpeacer1";
            // 
            // MonitorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbx256x192);
            this.Controls.Add(this.pbxMonitor);
            this.Controls.Add(this.pbxIchigoCharas);
            this.Controls.Add(this.pnlButton);
            this.Name = "MonitorForm";
            this.Activated += new System.EventHandler(this.MonitorForm_Activated);
            this.Deactivate += new System.EventHandler(this.MonitorForm_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MonitorForm_FormClosed);
            this.Load += new System.EventHandler(this.MonitorForm_Load);
            this.Shown += new System.EventHandler(this.MonitorForm_Shown);
            this.SizeChanged += new System.EventHandler(this.MonitorForm_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MonitorForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbxMonitor)).EndInit();
            this.MnuRightClick.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbx256x192)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxIchigoCharas)).EndInit();
            this.pnlButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxMonitor;
        private System.Windows.Forms.PictureBox pbx256x192;
        private System.Windows.Forms.PictureBox pbxIchigoCharas;
        private System.Windows.Forms.ContextMenuStrip MnuRightClick;
        private System.Windows.Forms.ToolStripMenuItem MnuRCGamenSize;
        private System.Windows.Forms.ToolStripMenuItem MnuRC1bai;
        private System.Windows.Forms.ToolStripMenuItem MnuRC2bai;
        private System.Windows.Forms.ToolStripMenuItem MnuRC3bai;
        private System.Windows.Forms.ToolStripMenuItem MnuRC4bai;
        private System.Windows.Forms.ToolStripMenuItem MnuTEST;
        private System.Windows.Forms.ToolStripMenuItem MnuResetScreen;
        private System.Windows.Forms.ToolStripMenuItem MnuCC;
        private System.Windows.Forms.ToolStripMenuItem MnuOption;
        private System.Windows.Forms.Timer timRefresh;
        private System.Windows.Forms.Timer timCursor;
        private System.Windows.Forms.Panel pnlButton;
        private System.Windows.Forms.CheckBox chkCursor;
        private System.Windows.Forms.Panel pnlSpeacer1;
        private System.Windows.Forms.CheckBox chkMouse;
        private System.Windows.Forms.Panel pnlSpeacer2;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Panel pnlSpeacer3;
        private System.Windows.Forms.ToolStripMenuItem MnuCursor;
        private System.Windows.Forms.ToolStripMenuItem MnuCurInsert;
        private System.Windows.Forms.ToolStripMenuItem MnuCurOverWrite;
    }
}
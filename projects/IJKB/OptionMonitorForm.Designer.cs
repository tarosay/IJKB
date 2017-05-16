namespace IchigoJamKeyBoard
{
    partial class OptionMonitorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionMonitorForm));
            this.tvwOption = new System.Windows.Forms.TreeView();
            this.gbxFrame = new System.Windows.Forms.GroupBox();
            this.lblFrameRateTani = new System.Windows.Forms.Label();
            this.tbxFrameRate = new System.Windows.Forms.TextBox();
            this.lblFrameRate = new System.Windows.Forms.Label();
            this.tmrInputErrChk = new System.Windows.Forms.Timer(this.components);
            this.pnlButton = new System.Windows.Forms.Panel();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbxFrame.SuspendLayout();
            this.pnlButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvwOption
            // 
            resources.ApplyResources(this.tvwOption, "tvwOption");
            this.tvwOption.Name = "tvwOption";
            this.tvwOption.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwOption_NodeMouseClick);
            // 
            // gbxFrame
            // 
            this.gbxFrame.Controls.Add(this.lblFrameRateTani);
            this.gbxFrame.Controls.Add(this.tbxFrameRate);
            this.gbxFrame.Controls.Add(this.lblFrameRate);
            resources.ApplyResources(this.gbxFrame, "gbxFrame");
            this.gbxFrame.Name = "gbxFrame";
            this.gbxFrame.TabStop = false;
            // 
            // lblFrameRateTani
            // 
            resources.ApplyResources(this.lblFrameRateTani, "lblFrameRateTani");
            this.lblFrameRateTani.Name = "lblFrameRateTani";
            // 
            // tbxFrameRate
            // 
            resources.ApplyResources(this.tbxFrameRate, "tbxFrameRate");
            this.tbxFrameRate.Name = "tbxFrameRate";
            // 
            // lblFrameRate
            // 
            resources.ApplyResources(this.lblFrameRate, "lblFrameRate");
            this.lblFrameRate.Name = "lblFrameRate";
            // 
            // tmrInputErrChk
            // 
            this.tmrInputErrChk.Interval = 50;
            this.tmrInputErrChk.Tick += new System.EventHandler(this.tmrInputErrChk_Tick);
            // 
            // pnlButton
            // 
            this.pnlButton.Controls.Add(this.btnSet);
            this.pnlButton.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.pnlButton, "pnlButton");
            this.pnlButton.Name = "pnlButton";
            // 
            // btnSet
            // 
            resources.ApplyResources(this.btnSet, "btnSet");
            this.btnSet.Name = "btnSet";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // OptionMonitorForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlButton);
            this.Controls.Add(this.gbxFrame);
            this.Controls.Add(this.tvwOption);
            this.Name = "OptionMonitorForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OptionMonitorForm_FormClosed);
            this.Load += new System.EventHandler(this.OptionMonitorForm_Load);
            this.gbxFrame.ResumeLayout(false);
            this.gbxFrame.PerformLayout();
            this.pnlButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvwOption;
        private System.Windows.Forms.GroupBox gbxFrame;
        private System.Windows.Forms.Label lblFrameRateTani;
        private System.Windows.Forms.TextBox tbxFrameRate;
        private System.Windows.Forms.Label lblFrameRate;
        private System.Windows.Forms.Timer tmrInputErrChk;
        private System.Windows.Forms.Panel pnlButton;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnCancel;
    }
}
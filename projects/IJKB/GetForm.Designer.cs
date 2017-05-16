namespace IchigoJamKeyBoard
{
    partial class GetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetForm));
            this.btnClose = new System.Windows.Forms.Button();
            this.tbxSaveNum = new System.Windows.Forms.TextBox();
            this.lblsave = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.sfdSave = new System.Windows.Forms.SaveFileDialog();
            this.pgbSave = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tbxSaveNum
            // 
            resources.ApplyResources(this.tbxSaveNum, "tbxSaveNum");
            this.tbxSaveNum.Name = "tbxSaveNum";
            // 
            // lblsave
            // 
            resources.ApplyResources(this.lblsave, "lblsave");
            this.lblsave.Name = "lblsave";
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pgbSave
            // 
            resources.ApplyResources(this.pgbSave, "pgbSave");
            this.pgbSave.Name = "pgbSave";
            this.pgbSave.Step = 20;
            // 
            // GetForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pgbSave);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblsave);
            this.Controls.Add(this.tbxSaveNum);
            this.Controls.Add(this.btnClose);
            this.Name = "GetForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SaveForm_FormClosed);
            this.Load += new System.EventHandler(this.SaveForm_Load);
            this.Shown += new System.EventHandler(this.SaveForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox tbxSaveNum;
        private System.Windows.Forms.Label lblsave;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog sfdSave;
        private System.Windows.Forms.ProgressBar pgbSave;
    }
}
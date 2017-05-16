namespace IchigoJamKeyBoard
{
    partial class SoftKeyBoard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SoftKeyBoard));
            this.pbxMain = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkIchigoJam = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMain)).BeginInit();
            this.SuspendLayout();
            // 
            // pbxMain
            // 
            this.pbxMain.Image = global::IchigoJamKeyBoard.Properties.Resources.IchogoChara1;
            resources.ApplyResources(this.pbxMain, "pbxMain");
            this.pbxMain.Name = "pbxMain";
            this.pbxMain.TabStop = false;
            this.pbxMain.Click += new System.EventHandler(this.pbxMain_Click);
            this.pbxMain.MouseLeave += new System.EventHandler(this.pbxMain_MouseLeave);
            this.pbxMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbxMain_MouseMove);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lnkIchigoJam
            // 
            resources.ApplyResources(this.lnkIchigoJam, "lnkIchigoJam");
            this.lnkIchigoJam.Name = "lnkIchigoJam";
            this.lnkIchigoJam.TabStop = true;
            this.lnkIchigoJam.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkIchigoJam_LinkClicked);
            this.lnkIchigoJam.Click += new System.EventHandler(this.lnkIchigoJam_Click);
            // 
            // SoftKeyBoard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lnkIchigoJam);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbxMain);
            this.Name = "SoftKeyBoard";
            this.Load += new System.EventHandler(this.SoftKeyBoard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbxMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lnkIchigoJam;

    }
}
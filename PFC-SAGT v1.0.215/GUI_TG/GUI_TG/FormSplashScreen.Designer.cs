namespace GUI_GT
{
    partial class FormSplashScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSplashScreen));
            this.timerSplashSecreen = new System.Windows.Forms.Timer(this.components);
            this.nameStudent = new System.Windows.Forms.Label();
            this.nameDirector1 = new System.Windows.Forms.Label();
            this.nameDirector2 = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timerSplashSecreen
            // 
            this.timerSplashSecreen.Enabled = true;
            this.timerSplashSecreen.Interval = 3000;
            this.timerSplashSecreen.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // nameStudent
            // 
            resources.ApplyResources(this.nameStudent, "nameStudent");
            this.nameStudent.BackColor = System.Drawing.Color.Transparent;
            this.nameStudent.ForeColor = System.Drawing.SystemColors.ControlText;
            this.nameStudent.Name = "nameStudent";
            // 
            // nameDirector1
            // 
            resources.ApplyResources(this.nameDirector1, "nameDirector1");
            this.nameDirector1.BackColor = System.Drawing.Color.Transparent;
            this.nameDirector1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.nameDirector1.Name = "nameDirector1";
            // 
            // nameDirector2
            // 
            resources.ApplyResources(this.nameDirector2, "nameDirector2");
            this.nameDirector2.BackColor = System.Drawing.Color.Transparent;
            this.nameDirector2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.nameDirector2.Name = "nameDirector2";
            // 
            // lbVersion
            // 
            resources.ApplyResources(this.lbVersion, "lbVersion");
            this.lbVersion.BackColor = System.Drawing.Color.Transparent;
            this.lbVersion.Name = "lbVersion";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Name = "label2";
            // 
            // FormSplashScreen
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GUI_GT.Properties.Resources.splashScreen3;
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.nameDirector2);
            this.Controls.Add(this.nameDirector1);
            this.Controls.Add(this.nameStudent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSplashScreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerSplashSecreen;
        private System.Windows.Forms.Label nameStudent;
        private System.Windows.Forms.Label nameDirector1;
        private System.Windows.Forms.Label nameDirector2;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
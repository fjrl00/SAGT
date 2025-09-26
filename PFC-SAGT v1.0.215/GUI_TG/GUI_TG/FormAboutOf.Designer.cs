namespace GUI_GT
{
    partial class FormAboutOf
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
            this.pictureBoxSagt_Uma = new System.Windows.Forms.PictureBox();
            this.lbSagtName = new System.Windows.Forms.Label();
            this.lbAlumName = new System.Windows.Forms.Label();
            this.lbProjectDirector = new System.Windows.Forms.Label();
            this.lbAcademicDirector = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.btAccept = new System.Windows.Forms.Button();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.lbMethodologicalAdviser = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSagt_Uma)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxSagt_Uma
            // 
            this.pictureBoxSagt_Uma.Image = global::GUI_GT.Properties.Resources.Barra_de_about_of;
            this.pictureBoxSagt_Uma.Location = new System.Drawing.Point(0, -1);
            this.pictureBoxSagt_Uma.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBoxSagt_Uma.Name = "pictureBoxSagt_Uma";
            this.pictureBoxSagt_Uma.Size = new System.Drawing.Size(648, 111);
            this.pictureBoxSagt_Uma.TabIndex = 0;
            this.pictureBoxSagt_Uma.TabStop = false;
            // 
            // lbSagtName
            // 
            this.lbSagtName.AutoSize = true;
            this.lbSagtName.Location = new System.Drawing.Point(24, 118);
            this.lbSagtName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSagtName.Name = "lbSagtName";
            this.lbSagtName.Size = new System.Drawing.Size(386, 16);
            this.lbSagtName.TabIndex = 1;
            this.lbSagtName.Text = "SAGT: Aplicación Software de la Teoría de la Generalizabilidad";
            // 
            // lbAlumName
            // 
            this.lbAlumName.AutoSize = true;
            this.lbAlumName.Location = new System.Drawing.Point(51, 150);
            this.lbAlumName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAlumName.Name = "lbAlumName";
            this.lbAlumName.Size = new System.Drawing.Size(281, 16);
            this.lbAlumName.TabIndex = 2;
            this.lbAlumName.Text = "Realizado por: Francisco Jesús Ramos Pérez";
            // 
            // lbProjectDirector
            // 
            this.lbProjectDirector.AutoSize = true;
            this.lbProjectDirector.Location = new System.Drawing.Point(51, 182);
            this.lbProjectDirector.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbProjectDirector.Name = "lbProjectDirector";
            this.lbProjectDirector.Size = new System.Drawing.Size(343, 16);
            this.lbProjectDirector.TabIndex = 3;
            this.lbProjectDirector.Text = "Director de Proyecto: Dr. Don Antonio Hernández Mendo";
            // 
            // lbAcademicDirector
            // 
            this.lbAcademicDirector.AutoSize = true;
            this.lbAcademicDirector.Location = new System.Drawing.Point(188, 212);
            this.lbAcademicDirector.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAcademicDirector.Name = "lbAcademicDirector";
            this.lbAcademicDirector.Size = new System.Drawing.Size(232, 16);
            this.lbAcademicDirector.TabIndex = 4;
            this.lbAcademicDirector.Text = "Dr. Don José Luis Pastrana Brincones";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(468, 118);
            this.lbVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(53, 16);
            this.lbVersion.TabIndex = 6;
            this.lbVersion.Text = "Versión";
            // 
            // btAccept
            // 
            this.btAccept.Location = new System.Drawing.Point(273, 343);
            this.btAccept.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(100, 28);
            this.btAccept.TabIndex = 8;
            this.btAccept.Text = "Aceptar";
            this.btAccept.UseVisualStyleBackColor = true;
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(23, 270);
            this.tbComment.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.ReadOnly = true;
            this.tbComment.Size = new System.Drawing.Size(600, 66);
            this.tbComment.TabIndex = 9;
            // 
            // lbMethodologicalAdviser
            // 
            this.lbMethodologicalAdviser.AutoSize = true;
            this.lbMethodologicalAdviser.Location = new System.Drawing.Point(51, 240);
            this.lbMethodologicalAdviser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbMethodologicalAdviser.Name = "lbMethodologicalAdviser";
            this.lbMethodologicalAdviser.Size = new System.Drawing.Size(501, 16);
            this.lbMethodologicalAdviser.TabIndex = 10;
            this.lbMethodologicalAdviser.Text = "Asesor Metodológico: Dr. Don Ángel Blanco Villaseñor (Universidad de Barcelona)";
            // 
            // FormAboutOf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 380);
            this.Controls.Add(this.lbMethodologicalAdviser);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.btAccept);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.lbAcademicDirector);
            this.Controls.Add(this.lbProjectDirector);
            this.Controls.Add(this.lbAlumName);
            this.Controls.Add(this.lbSagtName);
            this.Controls.Add(this.pictureBoxSagt_Uma);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAboutOf";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Acerca de SAGT";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSagt_Uma)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxSagt_Uma;
        private System.Windows.Forms.Label lbSagtName;
        private System.Windows.Forms.Label lbAlumName;
        private System.Windows.Forms.Label lbProjectDirector;
        private System.Windows.Forms.Label lbAcademicDirector;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Button btAccept;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Label lbMethodologicalAdviser;
    }
}
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
            this.lbProjectDirector1 = new System.Windows.Forms.Label();
            this.lbProjectDirector2 = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.btAccept = new System.Windows.Forms.Button();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.lbMethodologicalAdviser = new System.Windows.Forms.Label();
            this.lbAlumnName2 = new System.Windows.Forms.Label();
            this.lbProjectDirector = new System.Windows.Forms.Label();
            this.lbAlumnName1 = new System.Windows.Forms.Label();
            this.lbMethodologicalAdviser1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSagt_Uma)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxSagt_Uma
            // 
            this.pictureBoxSagt_Uma.Image = global::GUI_GT.Properties.Resources.Barra_de_about_of;
            this.pictureBoxSagt_Uma.Location = new System.Drawing.Point(0, -1);
            this.pictureBoxSagt_Uma.Name = "pictureBoxSagt_Uma";
            this.pictureBoxSagt_Uma.Size = new System.Drawing.Size(486, 90);
            this.pictureBoxSagt_Uma.TabIndex = 0;
            this.pictureBoxSagt_Uma.TabStop = false;
            // 
            // lbSagtName
            // 
            this.lbSagtName.AutoSize = true;
            this.lbSagtName.Location = new System.Drawing.Point(18, 96);
            this.lbSagtName.Name = "lbSagtName";
            this.lbSagtName.Size = new System.Drawing.Size(306, 13);
            this.lbSagtName.TabIndex = 1;
            this.lbSagtName.Text = "SAGT: Aplicación Software de la Teoría de la Generalizabilidad";
            // 
            // lbAlumName
            // 
            this.lbAlumName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAlumName.Location = new System.Drawing.Point(7, 120);
            this.lbAlumName.Name = "lbAlumName";
            this.lbAlumName.Size = new System.Drawing.Size(140, 13);
            this.lbAlumName.TabIndex = 2;
            this.lbAlumName.Text = "Realizado por:";
            this.lbAlumName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbProjectDirector1
            // 
            this.lbProjectDirector1.AutoSize = true;
            this.lbProjectDirector1.Location = new System.Drawing.Point(160, 168);
            this.lbProjectDirector1.Name = "lbProjectDirector1";
            this.lbProjectDirector1.Size = new System.Drawing.Size(174, 13);
            this.lbProjectDirector1.TabIndex = 3;
            this.lbProjectDirector1.Text = "Dr. Don Antonio Hernández Mendo";
            // 
            // lbProjectDirector2
            // 
            this.lbProjectDirector2.AutoSize = true;
            this.lbProjectDirector2.Location = new System.Drawing.Point(160, 192);
            this.lbProjectDirector2.Name = "lbProjectDirector2";
            this.lbProjectDirector2.Size = new System.Drawing.Size(186, 13);
            this.lbProjectDirector2.TabIndex = 4;
            this.lbProjectDirector2.Text = "Dr. Don José Luis Pastrana Brincones";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(351, 96);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(42, 13);
            this.lbVersion.TabIndex = 6;
            this.lbVersion.Text = "Versión";
            // 
            // btAccept
            // 
            this.btAccept.Location = new System.Drawing.Point(205, 279);
            this.btAccept.Name = "btAccept";
            this.btAccept.Size = new System.Drawing.Size(75, 23);
            this.btAccept.TabIndex = 8;
            this.btAccept.Text = "Aceptar";
            this.btAccept.UseVisualStyleBackColor = true;
            this.btAccept.Click += new System.EventHandler(this.btAccept_Click);
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(17, 240);
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.ReadOnly = true;
            this.tbComment.Size = new System.Drawing.Size(451, 33);
            this.tbComment.TabIndex = 9;
            this.tbComment.Text = "Proyecto fin de carrera de carácter interdisciplinar desarrollado en colaboración" +
    " con el Area de Psicología Social (Facultad de Psicología).";
            // 
            // lbMethodologicalAdviser
            // 
            this.lbMethodologicalAdviser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMethodologicalAdviser.Location = new System.Drawing.Point(7, 216);
            this.lbMethodologicalAdviser.Name = "lbMethodologicalAdviser";
            this.lbMethodologicalAdviser.Size = new System.Drawing.Size(140, 13);
            this.lbMethodologicalAdviser.TabIndex = 10;
            this.lbMethodologicalAdviser.Text = "Asesor Metodológico:";
            this.lbMethodologicalAdviser.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbAlumnName2
            // 
            this.lbAlumnName2.AutoSize = true;
            this.lbAlumnName2.Location = new System.Drawing.Point(160, 144);
            this.lbAlumnName2.Name = "lbAlumnName2";
            this.lbAlumnName2.Size = new System.Drawing.Size(154, 13);
            this.lbAlumnName2.TabIndex = 11;
            this.lbAlumnName2.Text = "Fernando Jesús Ruano Linares";
            // 
            // lbProjectDirector
            // 
            this.lbProjectDirector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbProjectDirector.Location = new System.Drawing.Point(7, 168);
            this.lbProjectDirector.Name = "lbProjectDirector";
            this.lbProjectDirector.Size = new System.Drawing.Size(140, 13);
            this.lbProjectDirector.TabIndex = 12;
            this.lbProjectDirector.Text = "Director de Proyecto:";
            this.lbProjectDirector.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbAlumnName1
            // 
            this.lbAlumnName1.AutoSize = true;
            this.lbAlumnName1.Location = new System.Drawing.Point(160, 120);
            this.lbAlumnName1.Name = "lbAlumnName1";
            this.lbAlumnName1.Size = new System.Drawing.Size(149, 13);
            this.lbAlumnName1.TabIndex = 13;
            this.lbAlumnName1.Text = "Francisco Jesús Ramos Pérez";
            // 
            // lbMethodologicalAdviser1
            // 
            this.lbMethodologicalAdviser1.AutoSize = true;
            this.lbMethodologicalAdviser1.Location = new System.Drawing.Point(160, 216);
            this.lbMethodologicalAdviser1.Name = "lbMethodologicalAdviser1";
            this.lbMethodologicalAdviser1.Size = new System.Drawing.Size(158, 13);
            this.lbMethodologicalAdviser1.TabIndex = 14;
            this.lbMethodologicalAdviser1.Text = "Dr. Don Ángel Blanco Villaseñor";
            // 
            // FormAboutOf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 309);
            this.Controls.Add(this.lbMethodologicalAdviser1);
            this.Controls.Add(this.lbAlumnName1);
            this.Controls.Add(this.lbProjectDirector);
            this.Controls.Add(this.lbAlumnName2);
            this.Controls.Add(this.lbMethodologicalAdviser);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.btAccept);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.lbProjectDirector2);
            this.Controls.Add(this.lbProjectDirector1);
            this.Controls.Add(this.lbAlumName);
            this.Controls.Add(this.lbSagtName);
            this.Controls.Add(this.pictureBoxSagt_Uma);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
        private System.Windows.Forms.Label lbProjectDirector1;
        private System.Windows.Forms.Label lbProjectDirector2;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Button btAccept;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Label lbMethodologicalAdviser;
        private System.Windows.Forms.Label lbAlumnName2;
        private System.Windows.Forms.Label lbProjectDirector;
        private System.Windows.Forms.Label lbAlumnName1;
        private System.Windows.Forms.Label lbMethodologicalAdviser1;
    }
}
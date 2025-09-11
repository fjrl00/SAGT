namespace GUI_GT
{
    partial class FormMeansImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMeansImport));
            this.cBoxTypesFiles = new System.Windows.Forms.ComboBox();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.lbSelectTypeFile = new System.Windows.Forms.Label();
            this.btBrowse = new System.Windows.Forms.Button();
            this.tbNameFile = new System.Windows.Forms.TextBox();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.pictBoxEduG = new System.Windows.Forms.PictureBox();
            this.pictBoxGT = new System.Windows.Forms.PictureBox();
            this.pictBoxNormal = new System.Windows.Forms.PictureBox();
            this.pictBoxExcel = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxEduG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxGT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxNormal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxExcel)).BeginInit();
            this.SuspendLayout();
            // 
            // cBoxTypesFiles
            // 
            this.cBoxTypesFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cBoxTypesFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxTypesFiles.FormattingEnabled = true;
            this.cBoxTypesFiles.Location = new System.Drawing.Point(8, 91);
            this.cBoxTypesFiles.Name = "cBoxTypesFiles";
            this.cBoxTypesFiles.Size = new System.Drawing.Size(453, 21);
            this.cBoxTypesFiles.TabIndex = 2;
            this.cBoxTypesFiles.SelectedIndexChanged += new System.EventHandler(this.Selection_cBoxtypeFile);
            // 
            // tbComment
            // 
            this.tbComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbComment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbComment.Location = new System.Drawing.Point(8, 125);
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.ReadOnly = true;
            this.tbComment.Size = new System.Drawing.Size(453, 96);
            this.tbComment.TabIndex = 10;
            // 
            // lbSelectTypeFile
            // 
            this.lbSelectTypeFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSelectTypeFile.AutoSize = true;
            this.lbSelectTypeFile.Location = new System.Drawing.Point(13, 75);
            this.lbSelectTypeFile.Name = "lbSelectTypeFile";
            this.lbSelectTypeFile.Size = new System.Drawing.Size(151, 13);
            this.lbSelectTypeFile.TabIndex = 11;
            this.lbSelectTypeFile.Text = "Seleccione un tipo de archivo:";
            // 
            // btBrowse
            // 
            this.btBrowse.Location = new System.Drawing.Point(382, 238);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.Size = new System.Drawing.Size(75, 23);
            this.btBrowse.TabIndex = 18;
            this.btBrowse.Text = "Examinar...";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
            // 
            // tbNameFile
            // 
            this.tbNameFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNameFile.BackColor = System.Drawing.Color.White;
            this.tbNameFile.Location = new System.Drawing.Point(8, 238);
            this.tbNameFile.Name = "tbNameFile";
            this.tbNameFile.ReadOnly = true;
            this.tbNameFile.Size = new System.Drawing.Size(357, 20);
            this.tbNameFile.TabIndex = 17;
            // 
            // btOk
            // 
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Image = global::GUI_GT.Properties.Resources.button_ok_h22x22;
            this.btOk.Location = new System.Drawing.Point(134, 275);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(92, 32);
            this.btOk.TabIndex = 0;
            this.btOk.Text = "Aceptar";
            this.btOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Image = global::GUI_GT.Properties.Resources.button_cancel_h22x22;
            this.btCancel.Location = new System.Drawing.Point(242, 275);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // pictBoxEduG
            // 
            this.pictBoxEduG.Image = global::GUI_GT.Properties.Resources.barra_para_importar_ficheros_edug;
            this.pictBoxEduG.Location = new System.Drawing.Point(0, 0);
            this.pictBoxEduG.Name = "pictBoxEduG";
            this.pictBoxEduG.Size = new System.Drawing.Size(473, 69);
            this.pictBoxEduG.TabIndex = 21;
            this.pictBoxEduG.TabStop = false;
            // 
            // pictBoxGT
            // 
            this.pictBoxGT.Image = global::GUI_GT.Properties.Resources.barra_para_importar_rsm1;
            this.pictBoxGT.Location = new System.Drawing.Point(0, 0);
            this.pictBoxGT.Name = "pictBoxGT";
            this.pictBoxGT.Size = new System.Drawing.Size(473, 69);
            this.pictBoxGT.TabIndex = 20;
            this.pictBoxGT.TabStop = false;
            // 
            // pictBoxNormal
            // 
            this.pictBoxNormal.Image = global::GUI_GT.Properties.Resources.barra_para_importar_normal;
            this.pictBoxNormal.Location = new System.Drawing.Point(0, 0);
            this.pictBoxNormal.Name = "pictBoxNormal";
            this.pictBoxNormal.Size = new System.Drawing.Size(473, 69);
            this.pictBoxNormal.TabIndex = 19;
            this.pictBoxNormal.TabStop = false;
            // 
            // pictBoxExcel
            // 
            this.pictBoxExcel.Image = global::GUI_GT.Properties.Resources.barra_para_importar_excel;
            this.pictBoxExcel.Location = new System.Drawing.Point(0, 0);
            this.pictBoxExcel.Name = "pictBoxExcel";
            this.pictBoxExcel.Size = new System.Drawing.Size(473, 69);
            this.pictBoxExcel.TabIndex = 22;
            this.pictBoxExcel.TabStop = false;
            // 
            // FormMeansImport
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(469, 319);
            this.Controls.Add(this.pictBoxExcel);
            this.Controls.Add(this.pictBoxEduG);
            this.Controls.Add(this.pictBoxGT);
            this.Controls.Add(this.pictBoxNormal);
            this.Controls.Add(this.btBrowse);
            this.Controls.Add(this.tbNameFile);
            this.Controls.Add(this.lbSelectTypeFile);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.cBoxTypesFiles);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMeansImport";
            this.Text = "Importar archivo de medias";
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxEduG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxGT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxNormal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxExcel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.ComboBox cBoxTypesFiles;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Label lbSelectTypeFile;
        private System.Windows.Forms.Button btBrowse;
        private System.Windows.Forms.TextBox tbNameFile;
        private System.Windows.Forms.PictureBox pictBoxNormal;
        private System.Windows.Forms.PictureBox pictBoxGT;
        private System.Windows.Forms.PictureBox pictBoxEduG;
        private System.Windows.Forms.PictureBox pictBoxExcel;
    }
}
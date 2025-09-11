namespace GUI_GT
{
    partial class FormSelectSaves
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectSaves));
            this.checkBoxSelectDatas = new System.Windows.Forms.CheckBox();
            this.checkBoxSelectTableMeans = new System.Windows.Forms.CheckBox();
            this.checkBoxSelectSSQ = new System.Windows.Forms.CheckBox();
            this.lbSavesText = new System.Windows.Forms.Label();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.pictureBoxHeader = new System.Windows.Forms.PictureBox();
            this.lbNameFile = new System.Windows.Forms.Label();
            this.tbNameFile = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxSelectDatas
            // 
            this.checkBoxSelectDatas.AutoSize = true;
            this.checkBoxSelectDatas.Location = new System.Drawing.Point(47, 125);
            this.checkBoxSelectDatas.Name = "checkBoxSelectDatas";
            this.checkBoxSelectDatas.Size = new System.Drawing.Size(237, 17);
            this.checkBoxSelectDatas.TabIndex = 110;
            this.checkBoxSelectDatas.Text = "Diseño de facetas y Tabla de observaciones";
            this.checkBoxSelectDatas.UseVisualStyleBackColor = true;
            // 
            // checkBoxSelectTableMeans
            // 
            this.checkBoxSelectTableMeans.AutoSize = true;
            this.checkBoxSelectTableMeans.Location = new System.Drawing.Point(47, 155);
            this.checkBoxSelectTableMeans.Name = "checkBoxSelectTableMeans";
            this.checkBoxSelectTableMeans.Size = new System.Drawing.Size(109, 17);
            this.checkBoxSelectTableMeans.TabIndex = 111;
            this.checkBoxSelectTableMeans.Text = "Tablas de medias";
            this.checkBoxSelectTableMeans.UseVisualStyleBackColor = true;
            // 
            // checkBoxSelectSSQ
            // 
            this.checkBoxSelectSSQ.AutoSize = true;
            this.checkBoxSelectSSQ.Location = new System.Drawing.Point(47, 185);
            this.checkBoxSelectSSQ.Name = "checkBoxSelectSSQ";
            this.checkBoxSelectSSQ.Size = new System.Drawing.Size(506, 17);
            this.checkBoxSelectSSQ.TabIndex = 112;
            this.checkBoxSelectSSQ.Text = "Sumas de cuadrados, Tabla de análisis de varianza, Tabla de G-Parámetros y Nivele" +
                "s de optimización";
            this.checkBoxSelectSSQ.UseVisualStyleBackColor = true;
            // 
            // lbSavesText
            // 
            this.lbSavesText.AutoSize = true;
            this.lbSavesText.BackColor = System.Drawing.Color.Transparent;
            this.lbSavesText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSavesText.Location = new System.Drawing.Point(4, 29);
            this.lbSavesText.Name = "lbSavesText";
            this.lbSavesText.Size = new System.Drawing.Size(398, 24);
            this.lbSavesText.TabIndex = 113;
            this.lbSavesText.Text = "Seleccione los elementos que desee guardar:";
            // 
            // btOk
            // 
            this.btOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Image = global::GUI_GT.Properties.Resources.button_ok_h22x22;
            this.btOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btOk.Location = new System.Drawing.Point(390, 254);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(92, 32);
            this.btOk.TabIndex = 109;
            this.btOk.Text = "Aceptar";
            this.btOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btOk.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Image = global::GUI_GT.Properties.Resources.button_cancel_h22x22;
            this.btCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btCancel.Location = new System.Drawing.Point(500, 254);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 108;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // pictureBoxHeader
            // 
            this.pictureBoxHeader.Image = global::GUI_GT.Properties.Resources.barra_de_guardado;
            this.pictureBoxHeader.Location = new System.Drawing.Point(-2, 0);
            this.pictureBoxHeader.Name = "pictureBoxHeader";
            this.pictureBoxHeader.Size = new System.Drawing.Size(614, 90);
            this.pictureBoxHeader.TabIndex = 114;
            this.pictureBoxHeader.TabStop = false;
            // 
            // lbNameFile
            // 
            this.lbNameFile.AutoSize = true;
            this.lbNameFile.Location = new System.Drawing.Point(44, 221);
            this.lbNameFile.Name = "lbNameFile";
            this.lbNameFile.Size = new System.Drawing.Size(44, 13);
            this.lbNameFile.TabIndex = 115;
            this.lbNameFile.Text = "Nombre";
            // 
            // tbNameFile
            // 
            this.tbNameFile.Location = new System.Drawing.Point(106, 217);
            this.tbNameFile.Name = "tbNameFile";
            this.tbNameFile.Size = new System.Drawing.Size(447, 20);
            this.tbNameFile.TabIndex = 116;
            // 
            // FormSelectSaves
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(612, 299);
            this.Controls.Add(this.tbNameFile);
            this.Controls.Add(this.lbNameFile);
            this.Controls.Add(this.lbSavesText);
            this.Controls.Add(this.checkBoxSelectSSQ);
            this.Controls.Add(this.checkBoxSelectTableMeans);
            this.Controls.Add(this.checkBoxSelectDatas);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.pictureBoxHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectSaves";
            this.ShowInTaskbar = false;
            this.Text = "Selección de elementos a guardar";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.CheckBox checkBoxSelectDatas;
        private System.Windows.Forms.CheckBox checkBoxSelectTableMeans;
        private System.Windows.Forms.CheckBox checkBoxSelectSSQ;
        private System.Windows.Forms.Label lbSavesText;
        private System.Windows.Forms.PictureBox pictureBoxHeader;
        private System.Windows.Forms.Label lbNameFile;
        private System.Windows.Forms.TextBox tbNameFile;
    }
}
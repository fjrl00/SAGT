namespace GUI_GT
{
    partial class FormSelectReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectReports));
            this.lbSelectReportText = new System.Windows.Forms.Label();
            this.checkBoxSelectSSQ = new System.Windows.Forms.CheckBox();
            this.checkBoxSelectTableMeans = new System.Windows.Forms.CheckBox();
            this.checkBoxSelectDatas = new System.Windows.Forms.CheckBox();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.pictureBoxHeader = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeader)).BeginInit();
            this.SuspendLayout();
            // 
            // lbSelectReportText
            // 
            this.lbSelectReportText.AutoSize = true;
            this.lbSelectReportText.BackColor = System.Drawing.Color.Transparent;
            this.lbSelectReportText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSelectReportText.Location = new System.Drawing.Point(5, 29);
            this.lbSelectReportText.Name = "lbSelectReportText";
            this.lbSelectReportText.Size = new System.Drawing.Size(331, 24);
            this.lbSelectReportText.TabIndex = 120;
            this.lbSelectReportText.Text = "Seleccione los elementos del informe:";
            // 
            // checkBoxSelectSSQ
            // 
            this.checkBoxSelectSSQ.AutoSize = true;
            this.checkBoxSelectSSQ.Location = new System.Drawing.Point(48, 185);
            this.checkBoxSelectSSQ.Name = "checkBoxSelectSSQ";
            this.checkBoxSelectSSQ.Size = new System.Drawing.Size(506, 17);
            this.checkBoxSelectSSQ.TabIndex = 119;
            this.checkBoxSelectSSQ.Text = "Sumas de cuadrados, Tabla de análisis de varianza, Tabla de G-Parámetros y Nivele" +
                "s de optimización";
            this.checkBoxSelectSSQ.UseVisualStyleBackColor = true;
            // 
            // checkBoxSelectTableMeans
            // 
            this.checkBoxSelectTableMeans.AutoSize = true;
            this.checkBoxSelectTableMeans.Location = new System.Drawing.Point(48, 155);
            this.checkBoxSelectTableMeans.Name = "checkBoxSelectTableMeans";
            this.checkBoxSelectTableMeans.Size = new System.Drawing.Size(109, 17);
            this.checkBoxSelectTableMeans.TabIndex = 118;
            this.checkBoxSelectTableMeans.Text = "Tablas de medias";
            this.checkBoxSelectTableMeans.UseVisualStyleBackColor = true;
            // 
            // checkBoxSelectDatas
            // 
            this.checkBoxSelectDatas.AutoSize = true;
            this.checkBoxSelectDatas.Location = new System.Drawing.Point(48, 125);
            this.checkBoxSelectDatas.Name = "checkBoxSelectDatas";
            this.checkBoxSelectDatas.Size = new System.Drawing.Size(237, 17);
            this.checkBoxSelectDatas.TabIndex = 117;
            this.checkBoxSelectDatas.Text = "Diseño de facetas y Tabla de observaciones";
            this.checkBoxSelectDatas.UseVisualStyleBackColor = true;
            // 
            // btOk
            // 
            this.btOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Image = global::GUI_GT.Properties.Resources.button_ok_h22x22;
            this.btOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btOk.Location = new System.Drawing.Point(391, 254);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(92, 32);
            this.btOk.TabIndex = 116;
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
            this.btCancel.Location = new System.Drawing.Point(501, 254);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 115;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // pictureBoxHeader
            // 
            this.pictureBoxHeader.Image = global::GUI_GT.Properties.Resources.barra_de_impresión2;
            this.pictureBoxHeader.Location = new System.Drawing.Point(-1, 0);
            this.pictureBoxHeader.Name = "pictureBoxHeader";
            this.pictureBoxHeader.Size = new System.Drawing.Size(614, 90);
            this.pictureBoxHeader.TabIndex = 121;
            this.pictureBoxHeader.TabStop = false;
            // 
            // FormSelectReports
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btOk;
            this.ClientSize = new System.Drawing.Size(612, 299);
            this.Controls.Add(this.lbSelectReportText);
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
            this.Name = "FormSelectReports";
            this.ShowInTaskbar = false;
            this.Text = "Selección de elementos del informe";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHeader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbSelectReportText;
        private System.Windows.Forms.CheckBox checkBoxSelectSSQ;
        private System.Windows.Forms.CheckBox checkBoxSelectTableMeans;
        private System.Windows.Forms.CheckBox checkBoxSelectDatas;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.PictureBox pictureBoxHeader;
    }
}
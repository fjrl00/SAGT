namespace GUI_GT
{
    partial class FormOmitLevelFacet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOmitLevelFacet));
            this.cBoxSelectFacet = new System.Windows.Forms.ComboBox();
            this.lbSelectFacet = new System.Windows.Forms.Label();
            this.checkedListBoxSelectShipLevels = new System.Windows.Forms.CheckedListBox();
            this.lbSelectSkipLevels = new System.Windows.Forms.Label();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cBoxSelectFacet
            // 
            this.cBoxSelectFacet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cBoxSelectFacet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxSelectFacet.FormattingEnabled = true;
            this.cBoxSelectFacet.Location = new System.Drawing.Point(21, 33);
            this.cBoxSelectFacet.Name = "cBoxSelectFacet";
            this.cBoxSelectFacet.Size = new System.Drawing.Size(251, 21);
            this.cBoxSelectFacet.TabIndex = 0;
            this.cBoxSelectFacet.SelectedIndexChanged += new System.EventHandler(this.cBoxSelectFacet_SelectedIndexChanged);
            // 
            // lbSelectFacet
            // 
            this.lbSelectFacet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSelectFacet.AutoSize = true;
            this.lbSelectFacet.Location = new System.Drawing.Point(21, 14);
            this.lbSelectFacet.Name = "lbSelectFacet";
            this.lbSelectFacet.Size = new System.Drawing.Size(90, 13);
            this.lbSelectFacet.TabIndex = 1;
            this.lbSelectFacet.Text = "Selecionar faceta";
            // 
            // checkedListBoxSelectShipLevels
            // 
            this.checkedListBoxSelectShipLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxSelectShipLevels.CheckOnClick = true;
            this.checkedListBoxSelectShipLevels.FormattingEnabled = true;
            this.checkedListBoxSelectShipLevels.Location = new System.Drawing.Point(21, 93);
            this.checkedListBoxSelectShipLevels.Name = "checkedListBoxSelectShipLevels";
            this.checkedListBoxSelectShipLevels.Size = new System.Drawing.Size(251, 199);
            this.checkedListBoxSelectShipLevels.TabIndex = 2;
            this.checkedListBoxSelectShipLevels.SelectedValueChanged += new System.EventHandler(this.checkedListBoxSelectShipLevels_SelectedValueChanged);
            // 
            // lbSelectSkipLevels
            // 
            this.lbSelectSkipLevels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSelectSkipLevels.AutoSize = true;
            this.lbSelectSkipLevels.Location = new System.Drawing.Point(24, 74);
            this.lbSelectSkipLevels.Name = "lbSelectSkipLevels";
            this.lbSelectSkipLevels.Size = new System.Drawing.Size(112, 13);
            this.lbSelectSkipLevels.TabIndex = 3;
            this.lbSelectSkipLevels.Text = "Marcar niveles a omitir";
            // 
            // btOk
            // 
            this.btOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Image = global::GUI_GT.Properties.Resources.button_ok_h22x22;
            this.btOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btOk.Location = new System.Drawing.Point(46, 307);
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
            this.btCancel.Location = new System.Drawing.Point(156, 307);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 108;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // FormOmitLevelFacet
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(294, 351);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.lbSelectSkipLevels);
            this.Controls.Add(this.checkedListBoxSelectShipLevels);
            this.Controls.Add(this.lbSelectFacet);
            this.Controls.Add(this.cBoxSelectFacet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOmitLevelFacet";
            this.ShowInTaskbar = false;
            this.Text = "Omitir niveles";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cBoxSelectFacet;
        private System.Windows.Forms.Label lbSelectFacet;
        private System.Windows.Forms.CheckedListBox checkedListBoxSelectShipLevels;
        private System.Windows.Forms.Label lbSelectSkipLevels;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
    }
}
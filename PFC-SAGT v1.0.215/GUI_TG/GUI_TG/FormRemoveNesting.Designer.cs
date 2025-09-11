namespace GUI_GT
{
    partial class FormRemoveNesting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRemoveNesting));
            this.btRemove = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.cListBoxSelectNestingRemove = new System.Windows.Forms.CheckedListBox();
            this.lbSelectNesting = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btRemove
            // 
            this.btRemove.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btRemove.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btRemove.Image = global::GUI_GT.Properties.Resources.Recycle_Bin_h22x22;
            this.btRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btRemove.Location = new System.Drawing.Point(64, 284);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(92, 32);
            this.btRemove.TabIndex = 25;
            this.btRemove.Text = "Eliminar";
            this.btRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btRemove.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Image = global::GUI_GT.Properties.Resources.button_cancel_h22x22;
            this.btCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btCancel.Location = new System.Drawing.Point(174, 284);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 24;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // cListBoxSelectNestingRemove
            // 
            this.cListBoxSelectNestingRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cListBoxSelectNestingRemove.CheckOnClick = true;
            this.cListBoxSelectNestingRemove.FormattingEnabled = true;
            this.cListBoxSelectNestingRemove.Location = new System.Drawing.Point(19, 43);
            this.cListBoxSelectNestingRemove.Name = "cListBoxSelectNestingRemove";
            this.cListBoxSelectNestingRemove.Size = new System.Drawing.Size(294, 199);
            this.cListBoxSelectNestingRemove.TabIndex = 26;
            // 
            // lbSelectNesting
            // 
            this.lbSelectNesting.AutoSize = true;
            this.lbSelectNesting.Location = new System.Drawing.Point(19, 24);
            this.lbSelectNesting.Name = "lbSelectNesting";
            this.lbSelectNesting.Size = new System.Drawing.Size(120, 13);
            this.lbSelectNesting.TabIndex = 27;
            this.lbSelectNesting.Text = "Seleccione anidamiento";
            // 
            // FormRemoveNesting
            // 
            this.AcceptButton = this.btRemove;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(331, 329);
            this.Controls.Add(this.lbSelectNesting);
            this.Controls.Add(this.cListBoxSelectNestingRemove);
            this.Controls.Add(this.btRemove);
            this.Controls.Add(this.btCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(275, 258);
            this.Name = "FormRemoveNesting";
            this.Text = "Quitar anidamiento";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.CheckedListBox cListBoxSelectNestingRemove;
        private System.Windows.Forms.Label lbSelectNesting;
    }
}
namespace GUI_GT
{
    partial class FormListFacets
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormListFacets));
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.cListBoxListsFacets = new System.Windows.Forms.CheckedListBox();
            this.btSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Image = global::GUI_GT.Properties.Resources.button_ok_h22x22;
            this.btOK.Location = new System.Drawing.Point(38, 446);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(92, 32);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "Aceptar";
            this.btOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Image = global::GUI_GT.Properties.Resources.button_cancel_h22x22;
            this.btCancel.Location = new System.Drawing.Point(142, 446);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // cListBoxListsFacets
            // 
            this.cListBoxListsFacets.CheckOnClick = true;
            this.cListBoxListsFacets.FormattingEnabled = true;
            this.cListBoxListsFacets.Location = new System.Drawing.Point(13, 13);
            this.cListBoxListsFacets.Name = "cListBoxListsFacets";
            this.cListBoxListsFacets.Size = new System.Drawing.Size(247, 379);
            this.cListBoxListsFacets.TabIndex = 2;
            // 
            // btSelect
            // 
            this.btSelect.Location = new System.Drawing.Point(38, 408);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(196, 32);
            this.btSelect.TabIndex = 3;
            this.btSelect.Text = "Seleccionar todos";
            this.btSelect.UseVisualStyleBackColor = true;
            this.btSelect.Click += new System.EventHandler(this.btSelect_Click);
            // 
            // FormListFacets
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(272, 486);
            this.Controls.Add(this.btSelect);
            this.Controls.Add(this.cListBoxListsFacets);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormListFacets";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generar medias";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.CheckedListBox cListBoxListsFacets;
        private System.Windows.Forms.Button btSelect;
    }
}
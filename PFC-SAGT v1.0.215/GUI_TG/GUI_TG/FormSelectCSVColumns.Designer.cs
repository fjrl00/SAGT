namespace GUI_GT
{
    partial class FormSelectCSVColumns
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectCSVColumns));
            this.listBoxExcludedColumns = new System.Windows.Forms.ListBox();
            this.listBoxFacets = new System.Windows.Forms.ListBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.labelExcludedColumns = new System.Windows.Forms.Label();
            this.labelFacets = new System.Windows.Forms.Label();
            this.btExcludedToFacets = new System.Windows.Forms.Button();
            this.btFacetsToExcluded = new System.Windows.Forms.Button();
            this.btExcludedToDependent = new System.Windows.Forms.Button();
            this.btDependentToExcluded = new System.Windows.Forms.Button();
            this.lbDependent = new System.Windows.Forms.Label();
            this.tbDependent = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // listBoxExcludedColumns
            // 
            this.listBoxExcludedColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxExcludedColumns.FormattingEnabled = true;
            this.listBoxExcludedColumns.Location = new System.Drawing.Point(26, 40);
            this.listBoxExcludedColumns.Name = "listBoxExcludedColumns";
            this.listBoxExcludedColumns.Size = new System.Drawing.Size(193, 212);
            this.listBoxExcludedColumns.TabIndex = 0;
            this.listBoxExcludedColumns.SelectedIndexChanged += new System.EventHandler(this.listBoxSourceDiff_SelectedIndexChanged);
            // 
            // listBoxFacets
            // 
            this.listBoxFacets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxFacets.FormattingEnabled = true;
            this.listBoxFacets.Location = new System.Drawing.Point(340, 40);
            this.listBoxFacets.Name = "listBoxFacets";
            this.listBoxFacets.Size = new System.Drawing.Size(193, 147);
            this.listBoxFacets.TabIndex = 1;
            // 
            // btOK
            // 
            this.btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Image = global::GUI_GT.Properties.Resources.button_ok_h22x22;
            this.btOK.Location = new System.Drawing.Point(179, 271);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(92, 32);
            this.btOK.TabIndex = 2;
            this.btOK.Text = "Aceptar";
            this.btOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Image = global::GUI_GT.Properties.Resources.button_cancel_h22x22;
            this.btCancel.Location = new System.Drawing.Point(288, 271);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // labelExcludedColumns
            // 
            this.labelExcludedColumns.AutoSize = true;
            this.labelExcludedColumns.Location = new System.Drawing.Point(26, 21);
            this.labelExcludedColumns.Name = "labelExcludedColumns";
            this.labelExcludedColumns.Size = new System.Drawing.Size(101, 13);
            this.labelExcludedColumns.TabIndex = 6;
            this.labelExcludedColumns.Text = "Columnas Excluidas";
            // 
            // labelFacets
            // 
            this.labelFacets.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelFacets.AutoSize = true;
            this.labelFacets.Location = new System.Drawing.Point(340, 20);
            this.labelFacets.Name = "labelFacets";
            this.labelFacets.Size = new System.Drawing.Size(45, 13);
            this.labelFacets.TabIndex = 7;
            this.labelFacets.Text = "Facetas";
            // 
            // btExcludedToFacets
            // 
            this.btExcludedToFacets.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btExcludedToFacets.Image = global::GUI_GT.Properties.Resources.Restart_right_h32;
            this.btExcludedToFacets.Location = new System.Drawing.Point(224, 40);
            this.btExcludedToFacets.Name = "btExcludedToFacets";
            this.btExcludedToFacets.Size = new System.Drawing.Size(52, 147);
            this.btExcludedToFacets.TabIndex = 9;
            this.btExcludedToFacets.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btExcludedToFacets.UseVisualStyleBackColor = true;
            this.btExcludedToFacets.Click += new System.EventHandler(this.btMoveRight_Click);
            // 
            // btFacetsToExcluded
            // 
            this.btFacetsToExcluded.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btFacetsToExcluded.Image = global::GUI_GT.Properties.Resources.Restart_left_h32;
            this.btFacetsToExcluded.Location = new System.Drawing.Point(282, 40);
            this.btFacetsToExcluded.Name = "btFacetsToExcluded";
            this.btFacetsToExcluded.Size = new System.Drawing.Size(52, 147);
            this.btFacetsToExcluded.TabIndex = 8;
            this.btFacetsToExcluded.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btFacetsToExcluded.UseVisualStyleBackColor = true;
            this.btFacetsToExcluded.Click += new System.EventHandler(this.btMoveLeft_Click);
            // 
            // btExcludedToDependent
            // 
            this.btExcludedToDependent.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btExcludedToDependent.Image = global::GUI_GT.Properties.Resources.Restart_right_h32;
            this.btExcludedToDependent.Location = new System.Drawing.Point(225, 205);
            this.btExcludedToDependent.Name = "btExcludedToDependent";
            this.btExcludedToDependent.Size = new System.Drawing.Size(52, 49);
            this.btExcludedToDependent.TabIndex = 10;
            this.btExcludedToDependent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btExcludedToDependent.UseVisualStyleBackColor = true;
            // 
            // btDependentToExcluded
            // 
            this.btDependentToExcluded.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btDependentToExcluded.Image = global::GUI_GT.Properties.Resources.Restart_left_h32;
            this.btDependentToExcluded.Location = new System.Drawing.Point(282, 205);
            this.btDependentToExcluded.Name = "btDependentToExcluded";
            this.btDependentToExcluded.Size = new System.Drawing.Size(52, 49);
            this.btDependentToExcluded.TabIndex = 11;
            this.btDependentToExcluded.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btDependentToExcluded.UseVisualStyleBackColor = true;
            // 
            // lbDependent
            // 
            this.lbDependent.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbDependent.AutoSize = true;
            this.lbDependent.Location = new System.Drawing.Point(340, 212);
            this.lbDependent.Name = "lbDependent";
            this.lbDependent.Size = new System.Drawing.Size(98, 13);
            this.lbDependent.TabIndex = 13;
            this.lbDependent.Text = "Variable de Medida";
            // 
            // tbDependent
            // 
            this.tbDependent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDependent.BackColor = System.Drawing.Color.White;
            this.tbDependent.Location = new System.Drawing.Point(340, 232);
            this.tbDependent.Name = "tbDependent";
            this.tbDependent.ReadOnly = true;
            this.tbDependent.Size = new System.Drawing.Size(193, 20);
            this.tbDependent.TabIndex = 14;
            // 
            // FormSelectCSVColumns
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(558, 316);
            this.Controls.Add(this.tbDependent);
            this.Controls.Add(this.lbDependent);
            this.Controls.Add(this.btDependentToExcluded);
            this.Controls.Add(this.btExcludedToDependent);
            this.Controls.Add(this.btExcludedToFacets);
            this.Controls.Add(this.btFacetsToExcluded);
            this.Controls.Add(this.labelFacets);
            this.Controls.Add(this.labelExcludedColumns);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.listBoxFacets);
            this.Controls.Add(this.listBoxExcludedColumns);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectCSVColumns";
            this.Text = "Seleccionar Columnas";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxExcludedColumns;
        private System.Windows.Forms.ListBox listBoxFacets;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label labelExcludedColumns;
        private System.Windows.Forms.Label labelFacets;
        private System.Windows.Forms.Button btFacetsToExcluded;
        private System.Windows.Forms.Button btExcludedToFacets;
        private System.Windows.Forms.Button btExcludedToDependent;
        private System.Windows.Forms.Button btDependentToExcluded;
        private System.Windows.Forms.Label lbDependent;
        private System.Windows.Forms.TextBox tbDependent;
    }
}
namespace GUI_GT
{
    partial class FormAddSignificanceLevels
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddSignificanceLevels));
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.dgvExInstFacts = new DataGridViewEx.DataGridViewEx();
            this.dgvExDiffFacts = new DataGridViewEx.DataGridViewEx();
            this.lbInstr_Facets = new System.Windows.Forms.Label();
            this.lbDiff_Facets = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExInstFacts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExDiffFacts)).BeginInit();
            this.SuspendLayout();
            // 
            // btOk
            // 
            this.btOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Image = global::GUI_GT.Properties.Resources.button_ok_h22x22;
            this.btOk.Location = new System.Drawing.Point(242, 355);
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
            this.btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Image = global::GUI_GT.Properties.Resources.button_cancel_h22x22;
            this.btCancel.Location = new System.Drawing.Point(365, 355);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // dgvExInstFacts
            // 
            this.dgvExInstFacts.AllowUserToAddRows = false;
            this.dgvExInstFacts.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgvExInstFacts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvExInstFacts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvExInstFacts.BackgroundColor = System.Drawing.Color.White;
            this.dgvExInstFacts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvExInstFacts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.MenuText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvExInstFacts.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvExInstFacts.Location = new System.Drawing.Point(12, 173);
            this.dgvExInstFacts.MinNumeracionFilas = 0;
            this.dgvExInstFacts.Name = "dgvExInstFacts";
            this.dgvExInstFacts.NumeracionFilas = true;
            this.dgvExInstFacts.NumeroColumnas = 0;
            this.dgvExInstFacts.NumeroFilas = 0;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvExInstFacts.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvExInstFacts.RowHeadersVisible = false;
            this.dgvExInstFacts.RowHeadersWidth = 50;
            this.dgvExInstFacts.Size = new System.Drawing.Size(657, 174);
            this.dgvExInstFacts.TabIndex = 3;
            this.dgvExInstFacts.KeyPressEditorCelda += new DataGridViewEx.EditorCeldaKeyPress(this.dgvExInstFacts_KeyPressEditorCelda);
            // 
            // dgvExDiffFacts
            // 
            this.dgvExDiffFacts.AllowUserToAddRows = false;
            this.dgvExDiffFacts.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgvExDiffFacts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvExDiffFacts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvExDiffFacts.BackgroundColor = System.Drawing.Color.White;
            this.dgvExDiffFacts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvExDiffFacts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.MenuText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvExDiffFacts.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvExDiffFacts.Location = new System.Drawing.Point(12, 28);
            this.dgvExDiffFacts.MinNumeracionFilas = 0;
            this.dgvExDiffFacts.Name = "dgvExDiffFacts";
            this.dgvExDiffFacts.NumeracionFilas = true;
            this.dgvExDiffFacts.NumeroColumnas = 0;
            this.dgvExDiffFacts.NumeroFilas = 0;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomRight;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvExDiffFacts.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvExDiffFacts.RowHeadersVisible = false;
            this.dgvExDiffFacts.RowHeadersWidth = 50;
            this.dgvExDiffFacts.Size = new System.Drawing.Size(657, 126);
            this.dgvExDiffFacts.TabIndex = 4;
            this.dgvExDiffFacts.KeyPressEditorCelda += new DataGridViewEx.EditorCeldaKeyPress(this.dgvExDiffFacts_KeyPressEditorCelda);
            // 
            // lbInstr_Facets
            // 
            this.lbInstr_Facets.AutoSize = true;
            this.lbInstr_Facets.Location = new System.Drawing.Point(13, 157);
            this.lbInstr_Facets.Name = "lbInstr_Facets";
            this.lbInstr_Facets.Size = new System.Drawing.Size(131, 13);
            this.lbInstr_Facets.TabIndex = 5;
            this.lbInstr_Facets.Text = "Facetas de instrumetación";
            // 
            // lbDiff_Facets
            // 
            this.lbDiff_Facets.AutoSize = true;
            this.lbDiff_Facets.Location = new System.Drawing.Point(12, 12);
            this.lbDiff_Facets.Name = "lbDiff_Facets";
            this.lbDiff_Facets.Size = new System.Drawing.Size(129, 13);
            this.lbDiff_Facets.TabIndex = 6;
            this.lbDiff_Facets.Text = "Facetas de diferenciación";
            // 
            // FormAddSignificanceLevels
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(682, 395);
            this.Controls.Add(this.lbDiff_Facets);
            this.Controls.Add(this.lbInstr_Facets);
            this.Controls.Add(this.dgvExDiffFacts);
            this.Controls.Add(this.dgvExInstFacts);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAddSignificanceLevels";
            this.Text = "Niveles de Optimización";
            ((System.ComponentModel.ISupportInitialize)(this.dgvExInstFacts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExDiffFacts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private DataGridViewEx.DataGridViewEx dgvExInstFacts;
        private DataGridViewEx.DataGridViewEx dgvExDiffFacts;
        private System.Windows.Forms.Label lbInstr_Facets;
        private System.Windows.Forms.Label lbDiff_Facets;
    }
}
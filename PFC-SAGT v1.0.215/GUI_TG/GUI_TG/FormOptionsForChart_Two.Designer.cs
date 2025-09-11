namespace GUI_GT
{
    partial class FormOptionsForChart_Two
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOptionsForChart_Two));
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.cListBoxListsFacets = new System.Windows.Forms.CheckedListBox();
            this.lbInst_Facets = new System.Windows.Forms.Label();
            this.groupBoxRangeValues = new System.Windows.Forms.GroupBox();
            this.numericUpDownInterval = new System.Windows.Forms.NumericUpDown();
            this.lbIncrement = new System.Windows.Forms.Label();
            this.textBoxEnding = new System.Windows.Forms.TextBox();
            this.textBoxBeginning = new System.Windows.Forms.TextBox();
            this.lbEnding = new System.Windows.Forms.Label();
            this.lbBeginning = new System.Windows.Forms.Label();
            this.groupBoxRangeValues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Image = global::GUI_GT.Properties.Resources.button_cancel_h22x22;
            this.btCancel.Location = new System.Drawing.Point(230, 321);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Image = global::GUI_GT.Properties.Resources.button_ok_h22x22;
            this.btOK.Location = new System.Drawing.Point(118, 321);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(92, 32);
            this.btOK.TabIndex = 2;
            this.btOK.Text = "Aceptar";
            this.btOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // cListBoxListsFacets
            // 
            this.cListBoxListsFacets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cListBoxListsFacets.CheckOnClick = true;
            this.cListBoxListsFacets.FormattingEnabled = true;
            this.cListBoxListsFacets.Location = new System.Drawing.Point(12, 41);
            this.cListBoxListsFacets.Name = "cListBoxListsFacets";
            this.cListBoxListsFacets.Size = new System.Drawing.Size(417, 154);
            this.cListBoxListsFacets.TabIndex = 4;
            // 
            // lbInst_Facets
            // 
            this.lbInst_Facets.AutoSize = true;
            this.lbInst_Facets.Location = new System.Drawing.Point(13, 22);
            this.lbInst_Facets.Name = "lbInst_Facets";
            this.lbInst_Facets.Size = new System.Drawing.Size(140, 13);
            this.lbInst_Facets.TabIndex = 5;
            this.lbInst_Facets.Text = "Facetas de instrumentación:";
            // 
            // groupBoxRangeValues
            // 
            this.groupBoxRangeValues.Controls.Add(this.numericUpDownInterval);
            this.groupBoxRangeValues.Controls.Add(this.lbIncrement);
            this.groupBoxRangeValues.Controls.Add(this.textBoxEnding);
            this.groupBoxRangeValues.Controls.Add(this.textBoxBeginning);
            this.groupBoxRangeValues.Controls.Add(this.lbEnding);
            this.groupBoxRangeValues.Controls.Add(this.lbBeginning);
            this.groupBoxRangeValues.Location = new System.Drawing.Point(12, 201);
            this.groupBoxRangeValues.Name = "groupBoxRangeValues";
            this.groupBoxRangeValues.Size = new System.Drawing.Size(417, 102);
            this.groupBoxRangeValues.TabIndex = 6;
            this.groupBoxRangeValues.TabStop = false;
            this.groupBoxRangeValues.Text = "Valores del intervalo";
            // 
            // numericUpDownInterval
            // 
            this.numericUpDownInterval.Location = new System.Drawing.Point(142, 68);
            this.numericUpDownInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownInterval.Name = "numericUpDownInterval";
            this.numericUpDownInterval.Size = new System.Drawing.Size(60, 20);
            this.numericUpDownInterval.TabIndex = 6;
            this.numericUpDownInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbIncrement
            // 
            this.lbIncrement.AutoSize = true;
            this.lbIncrement.Location = new System.Drawing.Point(16, 70);
            this.lbIncrement.Name = "lbIncrement";
            this.lbIncrement.Size = new System.Drawing.Size(120, 13);
            this.lbIncrement.TabIndex = 5;
            this.lbIncrement.Text = "Incremento del intervalo";
            // 
            // textBoxEnding
            // 
            this.textBoxEnding.Location = new System.Drawing.Point(288, 28);
            this.textBoxEnding.Name = "textBoxEnding";
            this.textBoxEnding.Size = new System.Drawing.Size(100, 20);
            this.textBoxEnding.TabIndex = 3;
            this.textBoxEnding.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxEnding_KeyPress);
            // 
            // textBoxBeginning
            // 
            this.textBoxBeginning.Location = new System.Drawing.Point(77, 28);
            this.textBoxBeginning.Name = "textBoxBeginning";
            this.textBoxBeginning.Size = new System.Drawing.Size(100, 20);
            this.textBoxBeginning.TabIndex = 2;
            this.textBoxBeginning.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxBeginning_KeyPress);
            // 
            // lbEnding
            // 
            this.lbEnding.AutoSize = true;
            this.lbEnding.Location = new System.Drawing.Point(232, 32);
            this.lbEnding.Name = "lbEnding";
            this.lbEnding.Size = new System.Drawing.Size(29, 13);
            this.lbEnding.TabIndex = 1;
            this.lbEnding.Text = "Final";
            // 
            // lbBeginning
            // 
            this.lbBeginning.AutoSize = true;
            this.lbBeginning.Location = new System.Drawing.Point(16, 32);
            this.lbBeginning.Name = "lbBeginning";
            this.lbBeginning.Size = new System.Drawing.Size(34, 13);
            this.lbBeginning.TabIndex = 0;
            this.lbBeginning.Text = "Inicial";
            // 
            // FormOptionsForChart_Two
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(441, 369);
            this.Controls.Add(this.groupBoxRangeValues);
            this.Controls.Add(this.lbInst_Facets);
            this.Controls.Add(this.cListBoxListsFacets);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOptionsForChart_Two";
            this.ShowInTaskbar = false;
            this.Text = "Selección de facetas";
            this.groupBoxRangeValues.ResumeLayout(false);
            this.groupBoxRangeValues.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.CheckedListBox cListBoxListsFacets;
        private System.Windows.Forms.Label lbInst_Facets;
        private System.Windows.Forms.GroupBox groupBoxRangeValues;
        private System.Windows.Forms.TextBox textBoxEnding;
        private System.Windows.Forms.TextBox textBoxBeginning;
        private System.Windows.Forms.Label lbEnding;
        private System.Windows.Forms.Label lbBeginning;
        private System.Windows.Forms.Label lbIncrement;
        private System.Windows.Forms.NumericUpDown numericUpDownInterval;
    }
}
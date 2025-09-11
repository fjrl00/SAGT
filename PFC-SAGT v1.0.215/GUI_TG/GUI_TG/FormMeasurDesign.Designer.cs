namespace GUI_GT
{
    partial class FormMeasurDesign
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMeasurDesign));
            this.listBoxSourceDiff = new System.Windows.Forms.ListBox();
            this.listBoxSourceInst = new System.Windows.Forms.ListBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.lbMeasurementDesign = new System.Windows.Forms.Label();
            this.tbMeasurementDesign = new System.Windows.Forms.TextBox();
            this.lbDiff_Facets = new System.Windows.Forms.Label();
            this.lbInstr_Facets = new System.Windows.Forms.Label();
            this.btMoveRight = new System.Windows.Forms.Button();
            this.btMoveLeft = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxSourceDiff
            // 
            this.listBoxSourceDiff.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxSourceDiff.FormattingEnabled = true;
            this.listBoxSourceDiff.Location = new System.Drawing.Point(26, 100);
            this.listBoxSourceDiff.Name = "listBoxSourceDiff";
            this.listBoxSourceDiff.Size = new System.Drawing.Size(193, 212);
            this.listBoxSourceDiff.TabIndex = 0;
            // 
            // listBoxSourceInst
            // 
            this.listBoxSourceInst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxSourceInst.FormattingEnabled = true;
            this.listBoxSourceInst.Location = new System.Drawing.Point(340, 100);
            this.listBoxSourceInst.Name = "listBoxSourceInst";
            this.listBoxSourceInst.Size = new System.Drawing.Size(193, 212);
            this.listBoxSourceInst.TabIndex = 1;
            // 
            // btOK
            // 
            this.btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Image = global::GUI_GT.Properties.Resources.button_ok_h22x22;
            this.btOK.Location = new System.Drawing.Point(179, 336);
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
            this.btCancel.Location = new System.Drawing.Point(288, 336);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // lbMeasurementDesign
            // 
            this.lbMeasurementDesign.AutoSize = true;
            this.lbMeasurementDesign.Location = new System.Drawing.Point(26, 13);
            this.lbMeasurementDesign.Name = "lbMeasurementDesign";
            this.lbMeasurementDesign.Size = new System.Drawing.Size(95, 13);
            this.lbMeasurementDesign.TabIndex = 4;
            this.lbMeasurementDesign.Text = "Diseño de medida:";
            // 
            // tbMeasurementDesign
            // 
            this.tbMeasurementDesign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMeasurementDesign.BackColor = System.Drawing.Color.White;
            this.tbMeasurementDesign.Location = new System.Drawing.Point(26, 29);
            this.tbMeasurementDesign.Name = "tbMeasurementDesign";
            this.tbMeasurementDesign.ReadOnly = true;
            this.tbMeasurementDesign.Size = new System.Drawing.Size(507, 20);
            this.tbMeasurementDesign.TabIndex = 5;
            // 
            // lbDiff_Facets
            // 
            this.lbDiff_Facets.AutoSize = true;
            this.lbDiff_Facets.Location = new System.Drawing.Point(26, 81);
            this.lbDiff_Facets.Name = "lbDiff_Facets";
            this.lbDiff_Facets.Size = new System.Drawing.Size(131, 13);
            this.lbDiff_Facets.TabIndex = 6;
            this.lbDiff_Facets.Text = "Facetas de Diferenciación";
            // 
            // lbInstr_Facets
            // 
            this.lbInstr_Facets.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbInstr_Facets.AutoSize = true;
            this.lbInstr_Facets.Location = new System.Drawing.Point(340, 80);
            this.lbInstr_Facets.Name = "lbInstr_Facets";
            this.lbInstr_Facets.Size = new System.Drawing.Size(138, 13);
            this.lbInstr_Facets.TabIndex = 7;
            this.lbInstr_Facets.Text = "Facetas de Instrumentación";
            // 
            // btMoveRight
            // 
            this.btMoveRight.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btMoveRight.Image = global::GUI_GT.Properties.Resources.Restart_right_h32;
            this.btMoveRight.Location = new System.Drawing.Point(225, 165);
            this.btMoveRight.Name = "btMoveRight";
            this.btMoveRight.Size = new System.Drawing.Size(109, 60);
            this.btMoveRight.TabIndex = 9;
            this.btMoveRight.Text = "Mover";
            this.btMoveRight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btMoveRight.UseVisualStyleBackColor = true;
            this.btMoveRight.Click += new System.EventHandler(this.btMoveRight_Click);
            // 
            // btMoveLeft
            // 
            this.btMoveLeft.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btMoveLeft.Image = global::GUI_GT.Properties.Resources.Restart_left_h32;
            this.btMoveLeft.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btMoveLeft.Location = new System.Drawing.Point(225, 100);
            this.btMoveLeft.Name = "btMoveLeft";
            this.btMoveLeft.Size = new System.Drawing.Size(109, 59);
            this.btMoveLeft.TabIndex = 8;
            this.btMoveLeft.Text = "Mover";
            this.btMoveLeft.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btMoveLeft.UseVisualStyleBackColor = true;
            this.btMoveLeft.Click += new System.EventHandler(this.btMoveLeft_Click);
            // 
            // FormMeasurDesign
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(558, 381);
            this.Controls.Add(this.btMoveRight);
            this.Controls.Add(this.btMoveLeft);
            this.Controls.Add(this.lbInstr_Facets);
            this.Controls.Add(this.lbDiff_Facets);
            this.Controls.Add(this.tbMeasurementDesign);
            this.Controls.Add(this.lbMeasurementDesign);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.listBoxSourceInst);
            this.Controls.Add(this.listBoxSourceDiff);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMeasurDesign";
            this.Text = "Diseño de medida";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxSourceDiff;
        private System.Windows.Forms.ListBox listBoxSourceInst;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label lbMeasurementDesign;
        private System.Windows.Forms.TextBox tbMeasurementDesign;
        private System.Windows.Forms.Label lbDiff_Facets;
        private System.Windows.Forms.Label lbInstr_Facets;
        private System.Windows.Forms.Button btMoveLeft;
        private System.Windows.Forms.Button btMoveRight;
    }
}
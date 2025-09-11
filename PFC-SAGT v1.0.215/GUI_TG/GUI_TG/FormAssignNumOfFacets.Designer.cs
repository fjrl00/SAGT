namespace GUI_GT
{
    partial class FormAssignNumOfFacets
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAssignNumOfFacets));
            this.lbTextAddNumFacets = new System.Windows.Forms.Label();
            this.lbTextNumFacets = new System.Windows.Forms.Label();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.tBoxNumberOfFacets = new System.Windows.Forms.TextBox();
            this.groupBoxProvisionOfFacets = new System.Windows.Forms.GroupBox();
            this.rbMixed = new System.Windows.Forms.RadioButton();
            this.rbNested = new System.Windows.Forms.RadioButton();
            this.rbCrossed = new System.Windows.Forms.RadioButton();
            this.pictureBoxCrossed_bn = new System.Windows.Forms.PictureBox();
            this.pictureBoxNested_bn = new System.Windows.Forms.PictureBox();
            this.pictureBoxMixed_bn = new System.Windows.Forms.PictureBox();
            this.pictureBoxMixed = new System.Windows.Forms.PictureBox();
            this.pictureBoxNested = new System.Windows.Forms.PictureBox();
            this.pictureBoxCrossed = new System.Windows.Forms.PictureBox();
            this.groupBoxProvisionOfFacets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCrossed_bn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNested_bn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMixed_bn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMixed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNested)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCrossed)).BeginInit();
            this.SuspendLayout();
            // 
            // lbTextAddNumFacets
            // 
            this.lbTextAddNumFacets.AutoSize = true;
            this.lbTextAddNumFacets.Location = new System.Drawing.Point(12, 20);
            this.lbTextAddNumFacets.Name = "lbTextAddNumFacets";
            this.lbTextAddNumFacets.Size = new System.Drawing.Size(211, 13);
            this.lbTextAddNumFacets.TabIndex = 0;
            this.lbTextAddNumFacets.Text = "Introduzca el número de facetas (mínimo 2)";
            // 
            // lbTextNumFacets
            // 
            this.lbTextNumFacets.AutoSize = true;
            this.lbTextNumFacets.Location = new System.Drawing.Point(12, 54);
            this.lbTextNumFacets.Name = "lbTextNumFacets";
            this.lbTextNumFacets.Size = new System.Drawing.Size(100, 13);
            this.lbTextNumFacets.TabIndex = 1;
            this.lbTextNumFacets.Text = "Número de facetas:";
            // 
            // btOk
            // 
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Image = global::GUI_GT.Properties.Resources.button_ok_h22x22;
            this.btOk.Location = new System.Drawing.Point(45, 230);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(92, 32);
            this.btOk.TabIndex = 2;
            this.btOk.Text = "Aceptar";
            this.btOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Image = global::GUI_GT.Properties.Resources.button_cancel_h22x22;
            this.btCancel.Location = new System.Drawing.Point(148, 230);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // tBoxNumberOfFacets
            // 
            this.tBoxNumberOfFacets.Location = new System.Drawing.Point(142, 51);
            this.tBoxNumberOfFacets.Name = "tBoxNumberOfFacets";
            this.tBoxNumberOfFacets.Size = new System.Drawing.Size(130, 20);
            this.tBoxNumberOfFacets.TabIndex = 5;
            this.tBoxNumberOfFacets.Text = "2";
            this.tBoxNumberOfFacets.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxNumberOfFacets_KeyPress);
            // 
            // groupBoxProvisionOfFacets
            // 
            this.groupBoxProvisionOfFacets.Controls.Add(this.pictureBoxCrossed_bn);
            this.groupBoxProvisionOfFacets.Controls.Add(this.pictureBoxNested_bn);
            this.groupBoxProvisionOfFacets.Controls.Add(this.pictureBoxMixed_bn);
            this.groupBoxProvisionOfFacets.Controls.Add(this.pictureBoxMixed);
            this.groupBoxProvisionOfFacets.Controls.Add(this.pictureBoxNested);
            this.groupBoxProvisionOfFacets.Controls.Add(this.pictureBoxCrossed);
            this.groupBoxProvisionOfFacets.Controls.Add(this.rbMixed);
            this.groupBoxProvisionOfFacets.Controls.Add(this.rbNested);
            this.groupBoxProvisionOfFacets.Controls.Add(this.rbCrossed);
            this.groupBoxProvisionOfFacets.Location = new System.Drawing.Point(15, 87);
            this.groupBoxProvisionOfFacets.Name = "groupBoxProvisionOfFacets";
            this.groupBoxProvisionOfFacets.Size = new System.Drawing.Size(257, 132);
            this.groupBoxProvisionOfFacets.TabIndex = 6;
            this.groupBoxProvisionOfFacets.TabStop = false;
            this.groupBoxProvisionOfFacets.Text = "Disposición de facetas";
            // 
            // rbMixed
            // 
            this.rbMixed.AutoSize = true;
            this.rbMixed.Location = new System.Drawing.Point(73, 102);
            this.rbMixed.Name = "rbMixed";
            this.rbMixed.Size = new System.Drawing.Size(55, 17);
            this.rbMixed.TabIndex = 2;
            this.rbMixed.TabStop = true;
            this.rbMixed.Text = "Mixtas";
            this.rbMixed.UseVisualStyleBackColor = true;
            this.rbMixed.CheckedChanged += new System.EventHandler(this.rbMixed_CheckedChanged);
            // 
            // rbNested
            // 
            this.rbNested.AutoSize = true;
            this.rbNested.Location = new System.Drawing.Point(73, 62);
            this.rbNested.Name = "rbNested";
            this.rbNested.Size = new System.Drawing.Size(69, 17);
            this.rbNested.TabIndex = 1;
            this.rbNested.TabStop = true;
            this.rbNested.Text = "Anidadas";
            this.rbNested.UseVisualStyleBackColor = true;
            this.rbNested.CheckedChanged += new System.EventHandler(this.rbNested_CheckedChanged);
            // 
            // rbCrossed
            // 
            this.rbCrossed.AutoSize = true;
            this.rbCrossed.Location = new System.Drawing.Point(73, 23);
            this.rbCrossed.Name = "rbCrossed";
            this.rbCrossed.Size = new System.Drawing.Size(69, 17);
            this.rbCrossed.TabIndex = 0;
            this.rbCrossed.TabStop = true;
            this.rbCrossed.Text = "Cruzadas";
            this.rbCrossed.UseVisualStyleBackColor = true;
            this.rbCrossed.CheckedChanged += new System.EventHandler(this.rbCrossed_CheckedChanged);
            // 
            // pictureBoxCrossed_bn
            // 
            this.pictureBoxCrossed_bn.Image = global::GUI_GT.Properties.Resources.circle_green_cross_blue_bn_v2_h34;
            this.pictureBoxCrossed_bn.Location = new System.Drawing.Point(7, 14);
            this.pictureBoxCrossed_bn.Name = "pictureBoxCrossed_bn";
            this.pictureBoxCrossed_bn.Size = new System.Drawing.Size(51, 34);
            this.pictureBoxCrossed_bn.TabIndex = 8;
            this.pictureBoxCrossed_bn.TabStop = false;
            // 
            // pictureBoxNested_bn
            // 
            this.pictureBoxNested_bn.Image = global::GUI_GT.Properties.Resources.circle_green_nested_bn_h34;
            this.pictureBoxNested_bn.Location = new System.Drawing.Point(16, 53);
            this.pictureBoxNested_bn.Name = "pictureBoxNested_bn";
            this.pictureBoxNested_bn.Size = new System.Drawing.Size(33, 34);
            this.pictureBoxNested_bn.TabIndex = 7;
            this.pictureBoxNested_bn.TabStop = false;
            // 
            // pictureBoxMixed_bn
            // 
            this.pictureBoxMixed_bn.Image = global::GUI_GT.Properties.Resources.circle_mixed_b_n_v3_h34;
            this.pictureBoxMixed_bn.Location = new System.Drawing.Point(7, 93);
            this.pictureBoxMixed_bn.Name = "pictureBoxMixed_bn";
            this.pictureBoxMixed_bn.Size = new System.Drawing.Size(51, 34);
            this.pictureBoxMixed_bn.TabIndex = 6;
            this.pictureBoxMixed_bn.TabStop = false;
            // 
            // pictureBoxMixed
            // 
            this.pictureBoxMixed.Image = global::GUI_GT.Properties.Resources.circle_mixed_v3_h34;
            this.pictureBoxMixed.Location = new System.Drawing.Point(7, 93);
            this.pictureBoxMixed.Name = "pictureBoxMixed";
            this.pictureBoxMixed.Size = new System.Drawing.Size(51, 34);
            this.pictureBoxMixed.TabIndex = 5;
            this.pictureBoxMixed.TabStop = false;
            // 
            // pictureBoxNested
            // 
            this.pictureBoxNested.Image = global::GUI_GT.Properties.Resources.circle_green_nested_h34;
            this.pictureBoxNested.Location = new System.Drawing.Point(16, 53);
            this.pictureBoxNested.Name = "pictureBoxNested";
            this.pictureBoxNested.Size = new System.Drawing.Size(33, 34);
            this.pictureBoxNested.TabIndex = 4;
            this.pictureBoxNested.TabStop = false;
            // 
            // pictureBoxCrossed
            // 
            this.pictureBoxCrossed.Image = global::GUI_GT.Properties.Resources.circle_green_cross_blue_v2_h34;
            this.pictureBoxCrossed.Location = new System.Drawing.Point(7, 14);
            this.pictureBoxCrossed.Name = "pictureBoxCrossed";
            this.pictureBoxCrossed.Size = new System.Drawing.Size(51, 34);
            this.pictureBoxCrossed.TabIndex = 3;
            this.pictureBoxCrossed.TabStop = false;
            // 
            // FormAssignNumOfFacets
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(284, 271);
            this.Controls.Add(this.groupBoxProvisionOfFacets);
            this.Controls.Add(this.tBoxNumberOfFacets);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.lbTextNumFacets);
            this.Controls.Add(this.lbTextAddNumFacets);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAssignNumOfFacets";
            this.ShowInTaskbar = false;
            this.Text = "Nuevo";
            this.groupBoxProvisionOfFacets.ResumeLayout(false);
            this.groupBoxProvisionOfFacets.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCrossed_bn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNested_bn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMixed_bn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMixed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNested)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCrossed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTextAddNumFacets;
        private System.Windows.Forms.Label lbTextNumFacets;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.TextBox tBoxNumberOfFacets;
        private System.Windows.Forms.GroupBox groupBoxProvisionOfFacets;
        private System.Windows.Forms.RadioButton rbMixed;
        private System.Windows.Forms.RadioButton rbNested;
        private System.Windows.Forms.RadioButton rbCrossed;
        private System.Windows.Forms.PictureBox pictureBoxCrossed;
        private System.Windows.Forms.PictureBox pictureBoxMixed;
        private System.Windows.Forms.PictureBox pictureBoxNested;
        private System.Windows.Forms.PictureBox pictureBoxMixed_bn;
        private System.Windows.Forms.PictureBox pictureBoxNested_bn;
        private System.Windows.Forms.PictureBox pictureBoxCrossed_bn;
    }
}
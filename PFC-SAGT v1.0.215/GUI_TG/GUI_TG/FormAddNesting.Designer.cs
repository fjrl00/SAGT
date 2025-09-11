namespace GUI_GT
{
    partial class FormAddNesting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddNesting));
            this.lbSelectFacet = new System.Windows.Forms.Label();
            this.lbSelectNestingFacet = new System.Windows.Forms.Label();
            this.cBoxNestingFacet = new System.Windows.Forms.ComboBox();
            this.cBoxSelectFacet = new System.Windows.Forms.ComboBox();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.lbResult = new System.Windows.Forms.Label();
            this.gBoxOperation = new System.Windows.Forms.GroupBox();
            this.pictureBoxSetCrossing = new System.Windows.Forms.PictureBox();
            this.pictureBoxSetNesting = new System.Windows.Forms.PictureBox();
            this.rbCross = new System.Windows.Forms.RadioButton();
            this.rbNest = new System.Windows.Forms.RadioButton();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.pictureBoxSetBlue = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBoxSetGreen = new System.Windows.Forms.PictureBox();
            this.gBoxOperation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetCrossing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetNesting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetGreen)).BeginInit();
            this.SuspendLayout();
            // 
            // lbSelectFacet
            // 
            this.lbSelectFacet.AutoSize = true;
            this.lbSelectFacet.Location = new System.Drawing.Point(76, 22);
            this.lbSelectFacet.Name = "lbSelectFacet";
            this.lbSelectFacet.Size = new System.Drawing.Size(40, 13);
            this.lbSelectFacet.TabIndex = 24;
            this.lbSelectFacet.Text = "Faceta";
            // 
            // lbSelectNestingFacet
            // 
            this.lbSelectNestingFacet.AutoSize = true;
            this.lbSelectNestingFacet.Location = new System.Drawing.Point(76, 74);
            this.lbSelectNestingFacet.Name = "lbSelectNestingFacet";
            this.lbSelectNestingFacet.Size = new System.Drawing.Size(115, 13);
            this.lbSelectNestingFacet.TabIndex = 25;
            this.lbSelectNestingFacet.Text = "Faceta de anidamiento";
            // 
            // cBoxNestingFacet
            // 
            this.cBoxNestingFacet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cBoxNestingFacet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxNestingFacet.FormattingEnabled = true;
            this.cBoxNestingFacet.Location = new System.Drawing.Point(70, 90);
            this.cBoxNestingFacet.Name = "cBoxNestingFacet";
            this.cBoxNestingFacet.Size = new System.Drawing.Size(440, 21);
            this.cBoxNestingFacet.TabIndex = 26;
            this.cBoxNestingFacet.SelectedIndexChanged += new System.EventHandler(this.cBoxNestingFacet_SelectedIndexChanged);
            // 
            // cBoxSelectFacet
            // 
            this.cBoxSelectFacet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cBoxSelectFacet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBoxSelectFacet.FormattingEnabled = true;
            this.cBoxSelectFacet.Location = new System.Drawing.Point(70, 39);
            this.cBoxSelectFacet.Name = "cBoxSelectFacet";
            this.cBoxSelectFacet.Size = new System.Drawing.Size(440, 21);
            this.cBoxSelectFacet.TabIndex = 27;
            this.cBoxSelectFacet.SelectedIndexChanged += new System.EventHandler(this.cBoxSelectFacet_SelectedIndexChanged);
            // 
            // tbResult
            // 
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tbResult.Location = new System.Drawing.Point(70, 210);
            this.tbResult.Name = "tbResult";
            this.tbResult.ReadOnly = true;
            this.tbResult.Size = new System.Drawing.Size(440, 20);
            this.tbResult.TabIndex = 30;
            // 
            // lbResult
            // 
            this.lbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbResult.AutoSize = true;
            this.lbResult.Location = new System.Drawing.Point(76, 194);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(105, 13);
            this.lbResult.TabIndex = 31;
            this.lbResult.Text = "Operación resultante";
            // 
            // gBoxOperation
            // 
            this.gBoxOperation.Controls.Add(this.pictureBoxSetCrossing);
            this.gBoxOperation.Controls.Add(this.pictureBoxSetNesting);
            this.gBoxOperation.Controls.Add(this.rbCross);
            this.gBoxOperation.Controls.Add(this.rbNest);
            this.gBoxOperation.Location = new System.Drawing.Point(70, 117);
            this.gBoxOperation.Name = "gBoxOperation";
            this.gBoxOperation.Size = new System.Drawing.Size(440, 68);
            this.gBoxOperation.TabIndex = 32;
            this.gBoxOperation.TabStop = false;
            this.gBoxOperation.Text = "Operación";
            // 
            // pictureBoxSetCrossing
            // 
            this.pictureBoxSetCrossing.Image = global::GUI_GT.Properties.Resources.circle_green_cross_blue_v2;
            this.pictureBoxSetCrossing.Location = new System.Drawing.Point(294, 12);
            this.pictureBoxSetCrossing.Name = "pictureBoxSetCrossing";
            this.pictureBoxSetCrossing.Size = new System.Drawing.Size(79, 50);
            this.pictureBoxSetCrossing.TabIndex = 3;
            this.pictureBoxSetCrossing.TabStop = false;
            // 
            // pictureBoxSetNesting
            // 
            this.pictureBoxSetNesting.Image = global::GUI_GT.Properties.Resources.circle_green___blue_h48;
            this.pictureBoxSetNesting.Location = new System.Drawing.Point(113, 12);
            this.pictureBoxSetNesting.Name = "pictureBoxSetNesting";
            this.pictureBoxSetNesting.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxSetNesting.TabIndex = 2;
            this.pictureBoxSetNesting.TabStop = false;
            // 
            // rbCross
            // 
            this.rbCross.AutoSize = true;
            this.rbCross.Location = new System.Drawing.Point(199, 31);
            this.rbCross.Name = "rbCross";
            this.rbCross.Size = new System.Drawing.Size(55, 17);
            this.rbCross.TabIndex = 1;
            this.rbCross.TabStop = true;
            this.rbCross.Text = "Cruzar";
            this.rbCross.UseVisualStyleBackColor = true;
            this.rbCross.CheckedChanged += new System.EventHandler(this.rbCross_CheckedChanged);
            // 
            // rbNest
            // 
            this.rbNest.AutoSize = true;
            this.rbNest.Location = new System.Drawing.Point(29, 31);
            this.rbNest.Name = "rbNest";
            this.rbNest.Size = new System.Drawing.Size(55, 17);
            this.rbNest.TabIndex = 0;
            this.rbNest.TabStop = true;
            this.rbNest.Text = "Anidar";
            this.rbNest.UseVisualStyleBackColor = true;
            this.rbNest.CheckedChanged += new System.EventHandler(this.rbNest_CheckedChanged);
            // 
            // btOk
            // 
            this.btOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Image = global::GUI_GT.Properties.Resources.button_ok_h22x22;
            this.btOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btOk.Location = new System.Drawing.Point(173, 257);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(92, 32);
            this.btOk.TabIndex = 23;
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
            this.btCancel.Location = new System.Drawing.Point(283, 257);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 22;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // pictureBoxSetBlue
            // 
            this.pictureBoxSetBlue.Image = global::GUI_GT.Properties.Resources.circle_blue_h24;
            this.pictureBoxSetBlue.Location = new System.Drawing.Point(29, 38);
            this.pictureBoxSetBlue.Name = "pictureBoxSetBlue";
            this.pictureBoxSetBlue.Size = new System.Drawing.Size(26, 30);
            this.pictureBoxSetBlue.TabIndex = 29;
            this.pictureBoxSetBlue.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::GUI_GT.Properties.Resources.circle_blue_h24;
            this.pictureBox2.Location = new System.Drawing.Point(29, 38);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(26, 30);
            this.pictureBox2.TabIndex = 29;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBoxSetGreen
            // 
            this.pictureBoxSetGreen.Image = global::GUI_GT.Properties.Resources.circle_green_h24;
            this.pictureBoxSetGreen.Location = new System.Drawing.Point(29, 88);
            this.pictureBoxSetGreen.Name = "pictureBoxSetGreen";
            this.pictureBoxSetGreen.Size = new System.Drawing.Size(25, 28);
            this.pictureBoxSetGreen.TabIndex = 28;
            this.pictureBoxSetGreen.TabStop = false;
            // 
            // FormAddNesting
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(548, 307);
            this.Controls.Add(this.gBoxOperation);
            this.Controls.Add(this.lbResult);
            this.Controls.Add(this.pictureBoxSetBlue);
            this.Controls.Add(this.tbResult);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBoxSetGreen);
            this.Controls.Add(this.cBoxSelectFacet);
            this.Controls.Add(this.cBoxNestingFacet);
            this.Controls.Add(this.lbSelectNestingFacet);
            this.Controls.Add(this.lbSelectFacet);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddNesting";
            this.Text = "Anidar faceta";
            this.gBoxOperation.ResumeLayout(false);
            this.gBoxOperation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetCrossing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetNesting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSetGreen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Label lbSelectFacet;
        private System.Windows.Forms.Label lbSelectNestingFacet;
        private System.Windows.Forms.ComboBox cBoxNestingFacet;
        private System.Windows.Forms.ComboBox cBoxSelectFacet;
        private System.Windows.Forms.PictureBox pictureBoxSetGreen;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Label lbResult;
        private System.Windows.Forms.GroupBox gBoxOperation;
        private System.Windows.Forms.RadioButton rbCross;
        private System.Windows.Forms.RadioButton rbNest;
        private System.Windows.Forms.PictureBox pictureBoxSetBlue;
        private System.Windows.Forms.PictureBox pictureBoxSetCrossing;
        private System.Windows.Forms.PictureBox pictureBoxSetNesting;
    }
}
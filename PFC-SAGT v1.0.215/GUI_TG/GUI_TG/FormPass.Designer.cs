namespace GUI_GT
{
    partial class FormPass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPass));
            this.lbPassword = new System.Windows.Forms.Label();
            this.lbUserId = new System.Windows.Forms.Label();
            this.m_tbPassword = new System.Windows.Forms.MaskedTextBox();
            this.tbUserId = new System.Windows.Forms.TextBox();
            this.pictureBoxPass = new System.Windows.Forms.PictureBox();
            this.pictureBoxUser = new System.Windows.Forms.PictureBox();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.linkLbForgottenPass = new System.Windows.Forms.LinkLabel();
            this.lbInfo = new System.Windows.Forms.Label();
            this.checkBoxSavePassword = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUser)).BeginInit();
            this.SuspendLayout();
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(97, 112);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(61, 13);
            this.lbPassword.TabIndex = 105;
            this.lbPassword.Text = "Contraseña";
            // 
            // lbUserId
            // 
            this.lbUserId.AutoSize = true;
            this.lbUserId.Location = new System.Drawing.Point(98, 48);
            this.lbUserId.Name = "lbUserId";
            this.lbUserId.Size = new System.Drawing.Size(43, 13);
            this.lbUserId.TabIndex = 104;
            this.lbUserId.Text = "Usuario";
            // 
            // m_tbPassword
            // 
            this.m_tbPassword.Location = new System.Drawing.Point(97, 128);
            this.m_tbPassword.Name = "m_tbPassword";
            this.m_tbPassword.PasswordChar = '*';
            this.m_tbPassword.PromptChar = '-';
            this.m_tbPassword.Size = new System.Drawing.Size(144, 20);
            this.m_tbPassword.TabIndex = 103;
            this.m_tbPassword.UseSystemPasswordChar = true;
            // 
            // tbUserId
            // 
            this.tbUserId.Location = new System.Drawing.Point(97, 64);
            this.tbUserId.MaxLength = 0;
            this.tbUserId.Name = "tbUserId";
            this.tbUserId.Size = new System.Drawing.Size(144, 20);
            this.tbUserId.TabIndex = 102;
            this.tbUserId.TextChanged += new System.EventHandler(this.tbUserId_TextChanged);
            // 
            // pictureBoxPass
            // 
            this.pictureBoxPass.Image = global::GUI_GT.Properties.Resources.icon_password_40;
            this.pictureBoxPass.InitialImage = null;
            this.pictureBoxPass.Location = new System.Drawing.Point(45, 113);
            this.pictureBoxPass.Name = "pictureBoxPass";
            this.pictureBoxPass.Size = new System.Drawing.Size(48, 49);
            this.pictureBoxPass.TabIndex = 109;
            this.pictureBoxPass.TabStop = false;
            // 
            // pictureBoxUser
            // 
            this.pictureBoxUser.Image = global::GUI_GT.Properties.Resources.User_Card_h40;
            this.pictureBoxUser.InitialImage = global::GUI_GT.Properties.Resources.User_Card_h40;
            this.pictureBoxUser.Location = new System.Drawing.Point(44, 51);
            this.pictureBoxUser.Name = "pictureBoxUser";
            this.pictureBoxUser.Size = new System.Drawing.Size(50, 44);
            this.pictureBoxUser.TabIndex = 108;
            this.pictureBoxUser.TabStop = false;
            // 
            // btOk
            // 
            this.btOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Image = global::GUI_GT.Properties.Resources.button_ok_h22x22;
            this.btOk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btOk.Location = new System.Drawing.Point(41, 232);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(92, 32);
            this.btOk.TabIndex = 107;
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
            this.btCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btCancel.Location = new System.Drawing.Point(151, 232);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(92, 32);
            this.btCancel.TabIndex = 106;
            this.btCancel.Text = "Cancelar";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // linkLbForgottenPass
            // 
            this.linkLbForgottenPass.AutoSize = true;
            this.linkLbForgottenPass.Location = new System.Drawing.Point(76, 194);
            this.linkLbForgottenPass.Name = "linkLbForgottenPass";
            this.linkLbForgottenPass.Size = new System.Drawing.Size(133, 13);
            this.linkLbForgottenPass.TabIndex = 110;
            this.linkLbForgottenPass.TabStop = true;
            this.linkLbForgottenPass.Text = "He olvidado mi contraseña";
            this.linkLbForgottenPass.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLbForgottenPass_LinkClicked);
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.Location = new System.Drawing.Point(26, 21);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(233, 13);
            this.lbInfo.TabIndex = 111;
            this.lbInfo.Text = "Introduzca el nombre de usuario y la contraseña";
            // 
            // checkBoxSavePassword
            // 
            this.checkBoxSavePassword.AutoSize = true;
            this.checkBoxSavePassword.Location = new System.Drawing.Point(82, 169);
            this.checkBoxSavePassword.Name = "checkBoxSavePassword";
            this.checkBoxSavePassword.Size = new System.Drawing.Size(120, 17);
            this.checkBoxSavePassword.TabIndex = 112;
            this.checkBoxSavePassword.Text = "Guardar contraseña";
            this.checkBoxSavePassword.UseVisualStyleBackColor = true;
            // 
            // FormPass
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GUI_GT.Properties.Resources.Fondo;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(284, 276);
            this.Controls.Add(this.checkBoxSavePassword);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.linkLbForgottenPass);
            this.Controls.Add(this.pictureBoxPass);
            this.Controls.Add(this.pictureBoxUser);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.lbPassword);
            this.Controls.Add(this.lbUserId);
            this.Controls.Add(this.m_tbPassword);
            this.Controls.Add(this.tbUserId);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPass";
            this.ShowInTaskbar = false;
            this.Text = "Conectar";
            this.Load += new System.EventHandler(this.FormPass_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUser)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.Label lbUserId;
        private System.Windows.Forms.MaskedTextBox m_tbPassword;
        private System.Windows.Forms.TextBox tbUserId;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.PictureBox pictureBoxUser;
        private System.Windows.Forms.PictureBox pictureBoxPass;
        private System.Windows.Forms.LinkLabel linkLbForgottenPass;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.CheckBox checkBoxSavePassword;
    }
}
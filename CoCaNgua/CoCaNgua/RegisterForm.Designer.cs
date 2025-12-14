namespace CoCaNgua
{
    partial class RegisterForm
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
            label1 = new Label();
            label2 = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            txtPasswordConfirm = new TextBox();
            btnRegister = new Button();
            txtEmail = new TextBox();
            label5 = new Label();
            label3 = new Label();
            label4 = new Label();
            linkToLog = new LinkLabel();
            label6 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(105, 26);
            label1.Name = "label1";
            label1.Size = new Size(167, 31);
            label1.TabIndex = 0;
            label1.Text = "Tên tài khoản:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Location = new Point(105, 99);
            label2.Name = "label2";
            label2.Size = new Size(122, 31);
            label2.TabIndex = 1;
            label2.Text = "Mật khẩu:";
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.LavenderBlush;
            txtUsername.Location = new Point(343, 23);
            txtUsername.Margin = new Padding(3, 4, 3, 4);
            txtUsername.Multiline = true;
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(347, 47);
            txtUsername.TabIndex = 4;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = Color.LavenderBlush;
            txtPassword.Location = new Point(343, 96);
            txtPassword.Margin = new Padding(3, 4, 3, 4);
            txtPassword.Multiline = true;
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(347, 47);
            txtPassword.TabIndex = 5;
            // 
            // txtPasswordConfirm
            // 
            txtPasswordConfirm.BackColor = Color.LavenderBlush;
            txtPasswordConfirm.Location = new Point(343, 170);
            txtPasswordConfirm.Margin = new Padding(3, 4, 3, 4);
            txtPasswordConfirm.Multiline = true;
            txtPasswordConfirm.Name = "txtPasswordConfirm";
            txtPasswordConfirm.PasswordChar = '*';
            txtPasswordConfirm.Size = new Size(347, 47);
            txtPasswordConfirm.TabIndex = 6;
            // 
            // btnRegister
            // 
            btnRegister.BackgroundImage = Properties.Resources.Screenshot_2025_11_28_222956;
            btnRegister.BackgroundImageLayout = ImageLayout.Stretch;
            btnRegister.FlatStyle = FlatStyle.Popup;
            btnRegister.Location = new Point(419, 316);
            btnRegister.Margin = new Padding(3, 4, 3, 4);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(271, 63);
            btnRegister.TabIndex = 7;
            btnRegister.Text = "Đăng kí ";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.LavenderBlush;
            txtEmail.Location = new Point(343, 245);
            txtEmail.Margin = new Padding(3, 4, 3, 4);
            txtEmail.Multiline = true;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(347, 47);
            txtEmail.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Location = new Point(105, 248);
            label5.Name = "label5";
            label5.Size = new Size(77, 31);
            label5.TabIndex = 9;
            label5.Text = "Email:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Location = new Point(105, 173);
            label3.Name = "label3";
            label3.Size = new Size(215, 31);
            label3.TabIndex = 2;
            label3.Text = "Nhập lại mật khẩu:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(122, 205);
            label4.Name = "label4";
            label4.Size = new Size(0, 31);
            label4.TabIndex = 3;
            // 
            // linkToLog
            // 
            linkToLog.AutoSize = true;
            linkToLog.BackColor = Color.Transparent;
            linkToLog.Location = new Point(566, 402);
            linkToLog.Name = "linkToLog";
            linkToLog.Size = new Size(124, 31);
            linkToLog.TabIndex = 10;
            linkToLog.TabStop = true;
            linkToLog.Text = "Đăng nhập";
            linkToLog.LinkClicked += linkToLog_LinkClicked;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Location = new Point(369, 402);
            label6.Name = "label6";
            label6.Size = new Size(191, 31);
            label6.TabIndex = 11;
            label6.Text = "Đã có tài khoản?";
            // 
            // RegisterForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Screenshot_2025_11_28_222745;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(809, 442);
            Controls.Add(label6);
            Controls.Add(linkToLog);
            Controls.Add(label5);
            Controls.Add(txtEmail);
            Controls.Add(btnRegister);
            Controls.Add(txtPasswordConfirm);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            DoubleBuffered = true;
            Font = new Font("Comic Sans MS", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5);
            Name = "RegisterForm";
            Text = "DangKi";
            Load += DangKi_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtPasswordConfirm;
        private Button btnRegister;
        private TextBox txtEmail;
        private Label label5;
        private Label label3;
        private Label label4;
        private LinkLabel linkToLog;
        private Label label6;
    }
}
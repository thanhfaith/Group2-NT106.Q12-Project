namespace CoCaNgua
{
    partial class LoginForm
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
            btnLogin = new Button();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            linkToRegister = new LinkLabel();
            llblQuenMatKhau = new LinkLabel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(140, 42);
            label1.Name = "label1";
            label1.Size = new Size(167, 31);
            label1.TabIndex = 0;
            label1.Text = "Tên tài khoản:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Location = new Point(140, 103);
            label2.Name = "label2";
            label2.Size = new Size(122, 31);
            label2.TabIndex = 1;
            label2.Text = "Mật khẩu:";
            // 
            // btnLogin
            // 
            btnLogin.BackgroundImage = Properties.Resources.Screenshot_2025_11_28_222956;
            btnLogin.BackgroundImageLayout = ImageLayout.Stretch;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatStyle = FlatStyle.Popup;
            btnLogin.Location = new Point(343, 162);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(304, 50);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Đăng nhập ";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(343, 39);
            txtUsername.Multiline = true;
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(304, 43);
            txtUsername.TabIndex = 3;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(343, 100);
            txtPassword.Multiline = true;
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(304, 43);
            txtPassword.TabIndex = 4;
            // 
            // linkToRegister
            // 
            linkToRegister.AutoSize = true;
            linkToRegister.BackColor = Color.Transparent;
            linkToRegister.Location = new Point(140, 230);
            linkToRegister.Name = "linkToRegister";
            linkToRegister.Size = new Size(204, 31);
            linkToRegister.TabIndex = 6;
            linkToRegister.TabStop = true;
            linkToRegister.Text = "Đăng ký tài khoản";
            linkToRegister.LinkClicked += linkToRegister_LinkClicked;
            // 
            // llblQuenMatKhau
            // 
            llblQuenMatKhau.AutoSize = true;
            llblQuenMatKhau.BackColor = Color.Transparent;
            llblQuenMatKhau.Location = new Point(458, 230);
            llblQuenMatKhau.Name = "llblQuenMatKhau";
            llblQuenMatKhau.Size = new Size(189, 31);
            llblQuenMatKhau.TabIndex = 7;
            llblQuenMatKhau.TabStop = true;
            llblQuenMatKhau.Text = "Quên mật khẩu?";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Screenshot_2025_11_28_222745;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(794, 345);
            Controls.Add(llblQuenMatKhau);
            Controls.Add(linkToRegister);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(btnLogin);
            Controls.Add(label2);
            Controls.Add(label1);
            DoubleBuffered = true;
            Font = new Font("Comic Sans MS", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(5);
            Name = "LoginForm";
            Text = "DangNhap";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Button btnLogin;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private LinkLabel linkToRegister;
        private LinkLabel llblQuenMatKhau;
    }
}
namespace CoCaNgua
{
    partial class StartGame
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
            btnLogin = new Button();
            btnRegister = new Button();
            SuspendLayout();
            // 
            // btnLogin
            // 
            btnLogin.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_010746;
            btnLogin.BackgroundImageLayout = ImageLayout.Stretch;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Times New Roman", 16.2F);
            btnLogin.Location = new Point(364, 216);
            btnLogin.Margin = new Padding(5);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(198, 74);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnRegister
            // 
            btnRegister.BackgroundImage = Properties.Resources.Screenshot_2025_11_15_010746;
            btnRegister.BackgroundImageLayout = ImageLayout.Stretch;
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Times New Roman", 16.2F);
            btnRegister.Location = new Point(364, 300);
            btnRegister.Margin = new Padding(5);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(198, 74);
            btnRegister.TabIndex = 3;
            btnRegister.Text = "Đăng ký";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // StartGame
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Screenshot_2025_11_15_005328;
            ClientSize = new Size(929, 527);
            Controls.Add(btnLogin);
            Controls.Add(btnRegister);
            Name = "StartGame";
            Text = "StartGame";
            ResumeLayout(false);
        }

        #endregion

        private Button btnLogin;
        private Button btnRegister;
    }
}
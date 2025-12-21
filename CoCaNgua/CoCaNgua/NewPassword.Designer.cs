namespace CoCaNgua
{
    partial class NewPassword
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
            label3 = new Label();
            btnXacNhan = new Button();
            txtOTP = new TextBox();
            txtMatKhauMoi = new TextBox();
            txtNhapLaiMatKhau = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Times New Roman", 13.8F);
            label1.Location = new Point(25, 65);
            label1.Name = "label1";
            label1.Size = new Size(91, 26);
            label1.TabIndex = 0;
            label1.Text = "Mã OTP";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Times New Roman", 13.8F);
            label2.Location = new Point(25, 133);
            label2.Name = "label2";
            label2.Size = new Size(141, 26);
            label2.TabIndex = 1;
            label2.Text = "Mật khẩu mới";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Times New Roman", 13.8F);
            label3.Location = new Point(25, 202);
            label3.Name = "label3";
            label3.Size = new Size(182, 26);
            label3.TabIndex = 2;
            label3.Text = "Nhập lại mật khẩu";
            // 
            // btnXacNhan
            // 
            btnXacNhan.BackgroundImage = Properties.Resources.Screenshot_2025_11_28_222956;
            btnXacNhan.BackgroundImageLayout = ImageLayout.Stretch;
            btnXacNhan.FlatAppearance.BorderSize = 0;
            btnXacNhan.FlatStyle = FlatStyle.Flat;
            btnXacNhan.Font = new Font("Times New Roman", 13.8F);
            btnXacNhan.Location = new Point(316, 248);
            btnXacNhan.Margin = new Padding(3, 4, 3, 4);
            btnXacNhan.Name = "btnXacNhan";
            btnXacNhan.Size = new Size(134, 52);
            btnXacNhan.TabIndex = 3;
            btnXacNhan.Text = "Xác nhận";
            btnXacNhan.UseVisualStyleBackColor = true;
            btnXacNhan.Click += btnXacNhan_Click;
            // 
            // txtOTP
            // 
            txtOTP.BackColor = Color.LavenderBlush;
            txtOTP.Font = new Font("Times New Roman", 13.8F);
            txtOTP.Location = new Point(211, 58);
            txtOTP.Margin = new Padding(3, 4, 3, 4);
            txtOTP.Name = "txtOTP";
            txtOTP.Size = new Size(342, 34);
            txtOTP.TabIndex = 4;
            // 
            // txtMatKhauMoi
            // 
            txtMatKhauMoi.BackColor = Color.LavenderBlush;
            txtMatKhauMoi.Font = new Font("Times New Roman", 13.8F);
            txtMatKhauMoi.Location = new Point(211, 126);
            txtMatKhauMoi.Margin = new Padding(3, 4, 3, 4);
            txtMatKhauMoi.Name = "txtMatKhauMoi";
            txtMatKhauMoi.PasswordChar = '*';
            txtMatKhauMoi.Size = new Size(342, 34);
            txtMatKhauMoi.TabIndex = 5;
            txtMatKhauMoi.UseSystemPasswordChar = true;
            // 
            // txtNhapLaiMatKhau
            // 
            txtNhapLaiMatKhau.BackColor = Color.LavenderBlush;
            txtNhapLaiMatKhau.Font = new Font("Times New Roman", 13.8F);
            txtNhapLaiMatKhau.Location = new Point(211, 195);
            txtNhapLaiMatKhau.Margin = new Padding(3, 4, 3, 4);
            txtNhapLaiMatKhau.Name = "txtNhapLaiMatKhau";
            txtNhapLaiMatKhau.PasswordChar = '*';
            txtNhapLaiMatKhau.Size = new Size(342, 34);
            txtNhapLaiMatKhau.TabIndex = 6;
            txtNhapLaiMatKhau.UseSystemPasswordChar = true;
            // 
            // NewPassword
            // 
            AutoScaleDimensions = new SizeF(12F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Screenshot_2025_11_28_222745;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(587, 332);
            Controls.Add(txtNhapLaiMatKhau);
            Controls.Add(txtMatKhauMoi);
            Controls.Add(txtOTP);
            Controls.Add(btnXacNhan);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            DoubleBuffered = true;
            Font = new Font("Comic Sans MS", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4, 5, 4, 5);
            Name = "NewPassword";
            Text = "Đặt lại mật khẩu";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Button btnXacNhan;
        private TextBox txtOTP;
        private TextBox txtMatKhauMoi;
        private TextBox txtNhapLaiMatKhau;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Collections.Specialized.BitVector32;


namespace CoCaNgua
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usernameOrEmail = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(usernameOrEmail) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hashedPassword = HashPassword(password);

            // ✅ GỬI LOGIN
            string response = SendToServer($"LOGIN|{usernameOrEmail}|{hashedPassword}");

            // ✅ XỬ LÝ LOGIN ĐÚNG
            if (response.StartsWith("LOGIN_OK|"))
            {
                Session.UserId = int.Parse(response.Split('|')[1]);

                MessageBox.Show("Đăng nhập thành công!");

                new CodeRoom().Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!",
                    "Đăng nhập thất bại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private string SendToServer(string data)
        {
            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 8888))
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = Encoding.UTF8.GetBytes(data);
                    stream.Write(buffer, 0, buffer.Length);

                    byte[] responseBuffer = new byte[2048];
                    int bytes = stream.Read(responseBuffer, 0, responseBuffer.Length);
                    return Encoding.UTF8.GetString(responseBuffer, 0, bytes);
                }
            }
            catch (Exception ex)
            {
                return $"Lỗi kết nối: {ex.Message}";
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private void linkToRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegisterForm f = new RegisterForm();
            f.Show();
            this.Hide();
        }
    }
}

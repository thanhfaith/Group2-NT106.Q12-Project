using System;
using System.Security.Cryptography;
using System.Text;
using System.Net.Sockets;
using System.Windows.Forms;

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

            // Sử dụng SendQuick (one-shot) cho login -> server trả LOGIN_OK|userId
            string response = SendQuick($"LOGIN|{usernameOrEmail}|{hashedPassword}");

            if (response.StartsWith("LOGIN_OK|"))
            {
                string[] parts = response.Split('|');
                Session.UserId = int.Parse(response.Split('|')[1]);
                if (parts.Length > 2)
                {
                    Session.Username = parts[2];
                }
                else
                {
                    Session.Username = usernameOrEmail; // Fallback
                }
                MessageBox.Show("Đăng nhập thành công!");

                // Connect persistent network once here (if chưa)
                if (!Session.Network.IsConnected)
                {
                    bool ok = Session.Network.Connect(ServerConfig.Host, ServerConfig.Port);
                    if (!ok)
                    {
                        MessageBox.Show("Không thể kết nối tới server (persistent).");
                        return;
                    }
                }

                // Show next form
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

        private string SendQuick(string message)
        {
            try
            {
                using (TcpClient client = new TcpClient(ServerConfig.Host, ServerConfig.Port))
                {
                    client.ReceiveTimeout = 5000;
                    client.SendTimeout = 5000;
                    client.NoDelay = true; // ✅ THÊM

                    NetworkStream stream = client.GetStream();

                    byte[] data = Encoding.UTF8.GetBytes(message + "\n"); // ✅ THÊM \n
                    stream.Write(data, 0, data.Length);
                    stream.Flush(); // ✅ THÊM FLUSH

                    byte[] buffer = new byte[2048];
                    int bytes = stream.Read(buffer, 0, buffer.Length);

                    string response = Encoding.UTF8.GetString(buffer, 0, bytes);
                    return response.Trim().Replace("\n", ""); // ✅ XÓA \n
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}");
                return "";
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

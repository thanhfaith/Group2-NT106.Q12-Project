using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CoCaNgua
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void DangKi_Load(object sender, EventArgs e)
        {

        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirm = txtPasswordConfirm.Text.Trim();

            // Validate
            if (string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirm))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không hợp lệ!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ✅ Disable button để tránh click nhiều lần
            btnRegister.Enabled = false;
            btnRegister.Text = "Đang xử lý...";

            try
            {
                string hashedPassword = HashPassword(password);
                string message = $"REGISTER|{username}|{email}|{hashedPassword}";

                // ✅ Gọi async
                string response = await SendToServerAsync(message);

                if (!string.IsNullOrWhiteSpace(response) &&
                    response.Trim() == "Đăng ký thành công!")
                {
                    MessageBox.Show(response,
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoginForm f = new LoginForm();
                    f.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        string.IsNullOrWhiteSpace(response)
                            ? "Không có phản hồi từ server!"
                            : response,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                // ✅ Enable lại button
                btnRegister.Enabled = true;
                btnRegister.Text = "Đăng ký";
            }
        }
        private async Task<string> SendToServerAsync(string data)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    // ✅ Connect async với timeout
                    var connectTask = client.ConnectAsync(ServerConfig.Host, ServerConfig.Port);
                    if (await Task.WhenAny(connectTask, Task.Delay(5000)) != connectTask)
                    {
                        return "Lỗi: Không thể kết nối tới server (timeout)";
                    }

                    NetworkStream stream = client.GetStream();

                    // ✅ Write async
                    byte[] buffer = Encoding.UTF8.GetBytes(data + "\n"); // THÊM \n
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                    await stream.FlushAsync();

                    // ✅ Read async với timeout
                    byte[] responseBuffer = new byte[2048];
                    var readTask = stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);

                    if (await Task.WhenAny(readTask, Task.Delay(5000)) != readTask)
                    {
                        return "Lỗi: Server không phản hồi (timeout)";
                    }

                    int bytes = await readTask;
                    string response = Encoding.UTF8.GetString(responseBuffer, 0, bytes).Trim();

                    if (string.IsNullOrWhiteSpace(response))
                        response = "(Không có phản hồi từ server)";

                    return response;
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

        private void linkToLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginForm f = new LoginForm();
            f.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}

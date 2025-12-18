using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace CoCaNgua
{
    public partial class NewPassword : Form
    {
        private string userEmail;

        public NewPassword(string email)
        {
            InitializeComponent();
            userEmail = email;
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string otp = txtOTP.Text.Trim();
            string newPass = txtMatKhauMoi.Text.Trim();
            string confirmPass = txtNhapLaiMatKhau.Text.Trim();

            // 1. Kiểm tra tính đầy đủ của thông tin
            if (string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // 2. Kiểm tra mật khẩu khớp nhau
            if (newPass != confirmPass)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!");
                return;
            }

            // 3. Mã hóa mật khẩu mới trước khi gửi qua mạng
            string hashedPassword = HashPassword(newPass);

            // 4. Gửi yêu cầu RESET_PASSWORD tới Server
            // Định dạng lệnh: RESET_PASSWORD|email|otp|hashedPassword
            string response = SendQuick($"RESET_PASSWORD|{userEmail}|{otp}|{hashedPassword}");

            // 5. Xử lý phản hồi từ Server
            if (response == "RESET_OK")
            {
                MessageBox.Show("Đổi mật khẩu thành công! Vui lòng đăng nhập lại.");
                LoginForm login = new LoginForm();
                login.Show();
                this.Close();
            }
            else if (response == "RESET_FAILED")
            {
                MessageBox.Show("Mã OTP không chính xác hoặc đã hết hạn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Lỗi kết nối Server: " + response, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string SendQuick(string message)
        {
            try
            {
                using (var client = new System.Net.Sockets.TcpClient(ServerConfig.Host, ServerConfig.Port))
                {
                    var stream = client.GetStream();
                    // Lưu ý cộng thêm \n để Server nhận biết kết thúc dòng (line-based protocol)
                    byte[] data = Encoding.UTF8.GetBytes(message + "\n");
                    stream.Write(data, 0, data.Length);
                    stream.Flush();

                    byte[] buffer = new byte[2048];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    return Encoding.UTF8.GetString(buffer, 0, bytes).Trim();
                }
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.Message;
            }
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
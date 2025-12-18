using System;
using System.Windows.Forms;

namespace CoCaNgua
{
    public partial class CheckEmail : Form
    {
        public CheckEmail()
        {
            InitializeComponent();
        }

        private void btnXacthucEmail_Click(object sender, EventArgs e)
        {
            string inputEmail = txtEmail.Text.Trim();

            // 1. Kiểm tra đầu vào cơ bản tại Client
            if (string.IsNullOrEmpty(inputEmail) || !inputEmail.Contains("@"))
            {
                MessageBox.Show("Vui lòng nhập Email hợp lệ!");
                return;
            }

            // 2. Gửi yêu cầu "FORGOT_PASSWORD" lên Server
            // Lưu ý: Hàm SendQuick phải tự động thêm \n ở cuối để Server nhận biết kết thúc lệnh.
            string response = SendQuick($"FORGOT_PASSWORD|{inputEmail}");

            // 3. Xử lý phản hồi từ Server trả về
            if (response == "FORGOT_OK")
            {
                MessageBox.Show("Mã xác thực đã được gửi qua Email của bạn!");

                // Chuyển sang Form nhập mã OTP và mật khẩu mới
                NewPassword f = new NewPassword(inputEmail);
                f.Show();
                this.Hide();
            }
            else if (response == "FORGOT_NOT_FOUND")
            {
                MessageBox.Show("Email này không tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Lỗi kết nối Server hoặc không thể gửi mail lúc này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Tái sử dụng hàm SendQuick từ LoginForm (hoặc đưa vào một class chung)
        private string SendQuick(string message)
        {
            try
            {
                using (var client = new System.Net.Sockets.TcpClient(ServerConfig.Host, ServerConfig.Port))
                {
                    var stream = client.GetStream();
                    // Server của bạn yêu cầu \n để tách message
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(message + "\n");
                    stream.Write(data, 0, data.Length);
                    stream.Flush();

                    byte[] buffer = new byte[2048];
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    return System.Text.Encoding.UTF8.GetString(buffer, 0, bytes).Trim();
                }
            }
            catch (Exception ex)
            {
                return "ERROR|" + ex.Message;
            }
        }
    }
}
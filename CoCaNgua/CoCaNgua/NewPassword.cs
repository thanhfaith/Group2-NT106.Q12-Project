using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace CoCaNgua
{
    public partial class NewPassword : Form
    {
        string connectionString =
           @"Data Source=.;Initial Catalog=GameDB;Integrated Security=True;TrustServerCertificate=True";

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

            if (string.IsNullOrEmpty(otp) ||
                string.IsNullOrEmpty(newPass) ||
                string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!");
                return;
            }

            bool result = ResetPassword(userEmail, otp, newPass);

            if (result)
            {
                MessageBox.Show("Đổi mật khẩu thành công! Vui lòng đăng nhập lại.");

                LoginForm login = new LoginForm();
                login.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(
                    "Mã OTP không chính xác hoặc đã hết hạn!",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        private bool ResetPassword(string email, string otp, string newPassword)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    UPDATE Users
                    SET Password = @NewPass,
                        ResetToken = NULL,
                        TokenExpiry = NULL
                    WHERE Email = @Email
                      AND ResetToken = @OTP
                      AND TokenExpiry > GETUTCDATE()";

                SqlCommand cmd = new SqlCommand(query, conn);

                string hashedPassword = HashPassword(newPassword);

                cmd.Parameters.Add("@NewPass", SqlDbType.VarChar).Value = hashedPassword;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
                cmd.Parameters.Add("@OTP", SqlDbType.VarChar).Value = otp;

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi database: " + ex.Message);
                    return false;
                }
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
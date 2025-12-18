using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace CoCaNgua
{
    public partial class CheckEmail : Form
    {
        string connectionString =
    @"Data Source=.; Initial Catalog=GameDB; Integrated Security=True; TrustServerCertificate=True";
        public CheckEmail()
        {
            InitializeComponent();
        }

        private void btnXacthucEmail_Click(object sender, EventArgs e)
        {
            string inputEmail = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(inputEmail))
            {
                MessageBox.Show("Vui lòng nhập Email!");
                return;
            }

            if (IsEmailExist(inputEmail))
            {
                string otp = GenerateOTP();
                SaveOTPToDatabase(inputEmail, otp);
                SendOTPEmail(inputEmail, otp);

                MessageBox.Show($"Mã xác thực đã được gửi đến!");
            }
            else
            {
                MessageBox.Show("Email không tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            NewPassword f = new NewPassword(inputEmail);
            f.Show();
            this.Hide();
        }
        private bool IsEmailExist(string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối Database: " + ex.Message);
                    return false;
                }
            }
        }
        private string GenerateOTP()
        {
            Random res = new Random();
            return res.Next(100000, 999999).ToString();
        }

        private void SaveOTPToDatabase(string email, string otp)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Users SET ResetToken = @Token, TokenExpiry = DATEADD(minute, 5, GETUTCDATE()) WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Token", otp);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        private void SendOTPEmail(string toEmail, string otp)
        {
            string fromEmail = "24520209@gm.uit.edu.vn";
            string appPassword = "xris kypu atzs zqsk";
            string displayName = "Game Cờ Cá Ngựa";

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail, displayName);
            mail.To.Add(toEmail);
            mail.Subject = "Mã xác thực đặt lại mật khẩu";

            string htmlBody = $@"
    <div style='font-family: ""Times New Roman"", Times, serif; color: #000000; padding: 30px; border: 1px solid #ccc; line-height: 1.6;'>
        <h2 style='text-align: center; color: #003366;'>XÁC THỰC TÀI KHOẢN</h2>
        <hr/>
        <p>Bạn đang thực hiện yêu cầu lấy lại mật khẩu cho tài khoản tại <b>Game Cờ Cá Ngựa</b>.</p>
        
        <div style='text-align: center; margin: 30px 0;'>
            <p style='font-size: 18px;'>Mã xác thực (OTP) của bạn là:</p>
            <span style='font-size: 36px; font-weight: bold; color: #ff0000; border: 2px dashed #ff0000; padding: 10px 20px;'>
                {otp}
            </span>
        </div>
        <p style='font-size: 14px;'><i>(*) Lưu ý: Mã này có giá trị sử dụng trong vòng <b>5 phút</b>. Vui lòng không cung cấp mã này cho bất kỳ ai.</i></p>
        <br/>
    </div>";

            mail.Body = htmlBody;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(fromEmail, appPassword);
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi mail: " + ex.Message);
            }
        }
    }
}

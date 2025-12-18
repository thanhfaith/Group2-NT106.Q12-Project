using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace CoCaNgua
{
    public partial class CodeRoom : Form
    {
        public CodeRoom()
        {
            InitializeComponent();
        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            string response = SendQuick($"CREATE_ROOM|{Session.UserId}");
            if (response.StartsWith("ROOM_CREATED|"))
            {
                string roomCode = response.Split('|')[1];
                // ✅ roomCode từ server đã là uppercase (vì database normalize)
                new WaitingRoom(roomCode).Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Không thể tạo phòng!");
            }
        }

        private void btnJoinRoom_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomCode.Text))
            {
                MessageBox.Show("Nhập mã phòng!");
                return;
            }

            // ✅ NORMALIZE ROOMCODE TRƯỚC KHI GỬI
            string roomCode = txtRoomCode.Text.Trim().ToUpper();

            string response = SendQuick($"JOIN_ROOM|{Session.UserId}|{roomCode}");

            if (response == "JOIN_OK")
            {
                new WaitingRoom(roomCode).Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Mã phòng không tồn tại hoặc đã bắt đầu!");
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
    }
}
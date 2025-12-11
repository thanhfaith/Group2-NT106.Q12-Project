using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace CoCaNgua
{
    public partial class CodeRoom : Form
    {
        public CodeRoom()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            string response = Send($"CREATE_ROOM|{Session.UserId}");

            if (response.StartsWith("ROOM_CREATED|"))
            {
                string roomCode = response.Split('|')[1];
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

            string response = Send($"JOIN_ROOM|{Session.UserId}|{txtRoomCode.Text}");

            if (response == "JOIN_OK")
            {
                new WaitingRoom(txtRoomCode.Text).Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Mã phòng không tồn tại hoặc đã bắt đầu!");
                MessageBox.Show(response);
            }
        }
        private string Send(string message)
        {
            using (TcpClient client = new TcpClient("127.0.0.1", 8888))
            {
                NetworkStream stream = client.GetStream();

                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[2048];
                int bytes = stream.Read(buffer, 0, buffer.Length);

                return Encoding.UTF8.GetString(buffer, 0, bytes);
            }
        }
    }
}

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

namespace CoCaNgua
{
    public partial class WaitingRoom : Form
    {
        private string roomCode;
        private NetworkHelper network;

        public WaitingRoom(string code)
        {
            InitializeComponent();
            roomCode = code;

            network = new NetworkHelper();
            network.OnMessageReceived += Network_OnMessageReceived;
        }
        private void Network_OnMessageReceived(string msg)
        {
            if (msg == "START")
            {
                this.Invoke(new Action(() =>
                {
                    ChessBoard cb = new ChessBoard(network);
                    cb.Show();
                    this.Hide();
                }));
            }
        }

        private void TimerRefresh_Tick(object sender, EventArgs e)
        {
            LoadPlayers();
        }
        private void LoadPlayers()
        {
            try
            {
                string res = Send($"GET_ROOM_PLAYERS|{roomCode}");

                string[] players = res.Split(',');

                lstPlayers.Items.Clear();
                foreach (var p in players)
                {
                    if (!string.IsNullOrWhiteSpace(p))
                        lstPlayers.Items.Add(p);
                }
            }
            catch { }
        }


        private void btnStartGame_Click(object sender, EventArgs e)
        {
            network.Send("JOIN_ROOM");

            ChessBoard cb = new ChessBoard(network);
            cb.Show();
            this.Hide();
        }

        private string Send(string msg)
        {
            using (TcpClient client = new TcpClient("127.0.0.1", 8888))
            {
                NetworkStream stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes(msg);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[2048];
                int bytes = stream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, bytes);
            }
        }

        private void WaitingRoom_Load(object sender, EventArgs e)
        {
            txtRoomCode.Text = roomCode;

            timerRefresh.Interval = 1500; // 1.5s
            timerRefresh.Tick += TimerRefresh_Tick;
            timerRefresh.Start();

            LoadPlayers();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}

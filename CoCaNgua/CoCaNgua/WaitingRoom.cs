using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CoCaNgua
{
    public partial class WaitingRoom : Form
    {
        private readonly string roomCode;
        private readonly NetworkHelper network;
        private bool hasStarted = false;
        private bool isRegistered = false;

        public WaitingRoom(string code)
        {
            InitializeComponent();
            roomCode = code.ToUpper(); // ✅ NORMALIZE NGAY TỪ ĐẦU

            // ✅ Tạo connection riêng
            network = new NetworkHelper();

            // Subscribe to messages
            network.OnMessageReceived -= Network_OnMessageReceived;
            network.OnMessageReceived += Network_OnMessageReceived;

            btnStartGame.Enabled = false;
        }

        private void Network_OnMessageReceived(string msg)
        {
            if (msg == null) return;
            msg = msg.Trim();

            AppendLog($"[RECV] {msg}");

            // Handle error messages
            if (msg.StartsWith("ERROR|"))
            {
                string errorMsg = msg.Substring(6);
                MessageBox.Show($"Lỗi: {errorMsg}");
                return;
            }

            // Server xác nhận đã đăng ký
            if (string.Equals(msg, "REGISTERED", StringComparison.OrdinalIgnoreCase))
            {
                isRegistered = true;
                if (InvokeRequired)
                    Invoke(new Action(() =>
                    {
                        btnStartGame.Enabled = true;
                        AppendLog("[INFO] ✅ Đã đăng ký phòng thành công!");
                    }));
                else
                {
                    btnStartGame.Enabled = true;
                    AppendLog("[INFO] ✅ Đã đăng ký phòng thành công!");
                }
                return;
            }

            // Server broadcast START
            if (msg.StartsWith("START", StringComparison.OrdinalIgnoreCase))
            {
                if (hasStarted) return;
                hasStarted = true;

                AppendLog("[INFO] 🎮 Nhận tín hiệu START! Đang mở game...");

                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        try
                        {
                            ChessBoard cb = new ChessBoard(network);
                            cb.Show();
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Lỗi mở game: {ex.Message}");
                        }
                    }));
                }
                else
                {
                    try
                    {
                        ChessBoard cb = new ChessBoard(network);
                        cb.Show();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi mở game: {ex.Message}");
                    }
                }
            }
        }

        private void AppendLog(string text)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(text);

                if (this.Controls.ContainsKey("lstLog"))
                {
                    var ctrl = this.Controls["lstLog"] as ListBox;
                    if (ctrl != null)
                    {
                        if (ctrl.InvokeRequired)
                            ctrl.Invoke(new Action(() =>
                            {
                                ctrl.Items.Add(text);
                                if (ctrl.Items.Count > 0) ctrl.TopIndex = ctrl.Items.Count - 1;
                            }));
                        else
                        {
                            ctrl.Items.Add(text);
                            if (ctrl.Items.Count > 0) ctrl.TopIndex = ctrl.Items.Count - 1;
                        }
                    }
                }
            }
            catch { }
        }

        private void TimerRefresh_Tick(object sender, EventArgs e)
        {
            LoadPlayers();
        }

        private void LoadPlayers()
        {
            try
            {
                // ✅ roomCode đã được normalize trong constructor
                string res = SendQuick($"GET_ROOM_PLAYERS|{roomCode}");
                if (string.IsNullOrEmpty(res)) return;

                string[] players = res.Split(',');

                if (lstPlayers.InvokeRequired)
                {
                    lstPlayers.Invoke(new Action(() =>
                    {
                        lstPlayers.Items.Clear();
                        foreach (var p in players)
                        {
                            if (!string.IsNullOrWhiteSpace(p))
                                lstPlayers.Items.Add(p);
                        }
                    }));
                }
                else
                {
                    lstPlayers.Items.Clear();
                    foreach (var p in players)
                    {
                        if (!string.IsNullOrWhiteSpace(p))
                            lstPlayers.Items.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog($"[ERROR] LoadPlayers: {ex.Message}");
            }
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            if (hasStarted)
            {
                MessageBox.Show("Game đã bắt đầu!");
                return;
            }

            if (!isRegistered)
            {
                MessageBox.Show("Vui lòng đợi server xác nhận đăng ký phòng.");
                return;
            }

            btnStartGame.Enabled = false;
            AppendLog("[SENT] 📤 Gửi lệnh START_GAME...");

            try
            {
                // ✅ roomCode đã được normalize
                network.Send($"START_GAME|{roomCode}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi lệnh: {ex.Message}");
                btnStartGame.Enabled = true;
            }
        }

        private string SendQuick(string msg)
        {
            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 8888))
                {
                    client.ReceiveTimeout = 3000;
                    client.SendTimeout = 3000;

                    NetworkStream stream = client.GetStream();

                    byte[] data = Encoding.UTF8.GetBytes(msg);
                    stream.Write(data, 0, data.Length);

                    byte[] buffer = new byte[2048];
                    int bytes = stream.Read(buffer, 0, buffer.Length);

                    return Encoding.UTF8.GetString(buffer, 0, bytes);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"[ERROR] SendQuick: {ex.Message}");
                return "";
            }
        }

        private void WaitingRoom_Load(object sender, EventArgs e)
        {
            // ✅ Hiển thị mã phòng đã normalize
            txtRoomCode.Text = roomCode;
            txtRoomCode.ReadOnly = true;

            AppendLog("[INFO] 🔌 Đang kết nối tới server...");

            bool ok = network.Connect("127.0.0.1", 8888);
            if (!ok)
            {
                MessageBox.Show("Không thể kết nối tới server!");
                this.Close();
                return;
            }

            AppendLog("[INFO] ✅ Đã kết nối! Đang đăng ký phòng...");

            Thread.Sleep(100);

            // ✅ Gửi roomCode đã normalize
            network.Send($"REGISTER_ROOM|{Session.UserId}|{roomCode}");
            AppendLog($"[SENT] 📤 REGISTER_ROOM|{Session.UserId}|{roomCode}");

            timerRefresh.Interval = 1000;
            timerRefresh.Tick += TimerRefresh_Tick;
            timerRefresh.Start();

            LoadPlayers();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timerRefresh.Stop();

            if (!hasStarted)
            {
                AppendLog("[INFO] 🔌 Đang ngắt kết nối...");
                network.Disconnect();
            }

            base.OnFormClosing(e);
        }
    }
}
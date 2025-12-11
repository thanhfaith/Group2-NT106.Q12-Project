using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace CoCaNguaServer
{
    public partial class Form1 : Form
    {
        TcpListener listener;
        bool isRunning = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Start_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                listener = new TcpListener(IPAddress.Any, 8888);
                listener.Start();
                isRunning = true;
                lb_status.Text = "🟢 Server đang chạy...";
                Thread thread = new Thread(ListenForClients);
                thread.Start();
            }
        }
        private void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    string clientInfo = client.Client.RemoteEndPoint.ToString();
                    Invoke(new Action(() => lstClients.Items.Add(clientInfo)));
                    Thread thread = new Thread(() => HandleClient(client, clientInfo));
                    thread.Start();
                }
                catch (SocketException)
                {
                    break;
                }
            }
        }
        private void HandleClient(TcpClient client, string clientInfo)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[2048];
                int byteCount = stream.Read(buffer, 0, buffer.Length);
                string request = Encoding.UTF8.GetString(buffer, 0, byteCount);
                string response = "";

                // Log ra listbox nếu muốn
                // Log($"Từ {clientInfo}: {request}");

                if (request.StartsWith("REGISTER|"))
                {
                    string[] parts = request.Split('|');
                    // REGISTER|username|email|passwordHash
                    if (parts.Length == 4)
                    {
                        string username = parts[1];
                        string email = parts[2];
                        string passwordHash = parts[3];

                        bool success = DatabaseHelper.RegisterUser(username, email, passwordHash);
                        response = success ? "Đăng ký thành công!" : "Username hoặc email đã tồn tại!";
                    }
                    else response = "Dữ liệu đăng ký không hợp lệ.";
                }
                else if (request.StartsWith("LOGIN|"))
                {
                    var parts = request.Split('|');
                    if (parts.Length == 3)
                    {
                        int userId = DatabaseHelper.GetUserId(parts[1], parts[2]);
                        response = userId > 0
                            ? $"LOGIN_OK|{userId}"
                            : "LOGIN_FAIL";
                    }
                }
                else if (request.StartsWith("CREATE_ROOM|"))
                {
                    int userId = int.Parse(request.Split('|')[1]);
                    string roomCode = DatabaseHelper.CreateRoom(userId);
                    response = "ROOM_CREATED|" + roomCode;
                }
                else if (request.StartsWith("JOIN_ROOM|"))
                {
                    var p = request.Split('|');
                    int userId = int.Parse(p[1]);
                    string roomCode = p[2];

                    bool ok = DatabaseHelper.JoinRoom(roomCode, userId);
                    response = ok ? "JOIN_OK" : "JOIN_FAIL";
                }
                else if (request.StartsWith("GET_ROOM_PLAYERS|"))
                {
                    string roomCode = request.Split('|')[1];
                    var players = DatabaseHelper.GetRoomPlayers(roomCode);
                    response = string.Join(",", players);
                }
                else if (request.StartsWith("START_GAME|"))
                {
                    string roomCode = request.Split('|')[1];
                    var players = DatabaseHelper.GetRoomPlayers(roomCode);

                    // Gửi cho từng client ở phòng
                    ServerBroadcaster.BroadcastToRoom(roomCode,
                        "GAME_START|" + string.Join(",", players));

                    response = "START_OK";
                }

                else
                {
                    response = "Lệnh không hợp lệ.";
                }

                byte[] responseData = Encoding.UTF8.GetBytes(response);
                stream.Write(responseData, 0, responseData.Length);
            }
            catch (Exception ex)
            {
                string err = "ERROR|" + ex.Message;
                byte[] errData = Encoding.UTF8.GetBytes(err);
                client.GetStream().Write(errData, 0, errData.Length);


            }
            finally
            {
                client.Close();
            }
        }
        public static class ServerBroadcaster
        {
            public static Dictionary<string, List<TcpClient>> Rooms = new Dictionary<string, List<TcpClient>>();

            public static void AddClientToRoom(string roomCode, TcpClient client)
            {
                if (!Rooms.ContainsKey(roomCode))
                    Rooms[roomCode] = new List<TcpClient>();

                Rooms[roomCode].Add(client);
            }

            public static void BroadcastToRoom(string roomCode, string msg)
            {
                if (!Rooms.ContainsKey(roomCode)) return;

                byte[] data = Encoding.UTF8.GetBytes(msg);

                foreach (var c in Rooms[roomCode])
                {
                    try
                    {
                        c.GetStream().Write(data, 0, data.Length);
                    }
                    catch { }
                }
            }
        }

        private void Log(string message)
        {
            Invoke(new Action(() =>
            {
                lstLog.Items.Add(message); // lstLog là ListBox
                                           // Tự cuộn xuống item mới
                lstLog.TopIndex = lstLog.Items.Count - 1;

                // Giới hạn số item tối đa (ví dụ 1000)
                if (lstLog.Items.Count > 1000)
                    lstLog.Items.RemoveAt(0);
            }));
        }
        private void btn_Stop_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                isRunning = false; // Dừng vòng lặp accept client
                listener.Stop();   // Ngắt TcpListener
                lb_status.Text = "🔴 Server đã dừng.";
                Log("Server đã dừng.");
            }
        }

        private void lstClients_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

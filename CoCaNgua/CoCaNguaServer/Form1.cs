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

                // ✅ PERSISTENT CONNECTION - Đọc liên tục
                while (client.Connected)
                {
                    int byteCount = stream.Read(buffer, 0, buffer.Length);
                    if (byteCount == 0) break; // Client ngắt kết nối

                    string request = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    string response = "";

                    if (request.StartsWith("REGISTER|"))
                    {
                        string[] parts = request.Split('|');
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
                    // ✅ REGISTER_ROOM - Client thông báo đang ở phòng nào
                    else if (request.StartsWith("REGISTER_ROOM|"))
                    {
                        var p = request.Split('|');
                        int userId = int.Parse(p[1]);
                        string roomCode = p[2];

                        string clientEndpoint = client.Client.RemoteEndPoint.ToString();

                        ServerBroadcaster.AddClientToRoom(roomCode, client);

                        int clientCount = ServerBroadcaster.GetRoomClientCount(roomCode);

                        response = "REGISTERED";

                        Log($"REGISTER_ROOM -> user:{userId} room:{roomCode} endpoint:{clientEndpoint} (clients in room: {clientCount})");
                    }

                    // ✅ START_GAME - Broadcast cho tất cả trong phòng
                    else if (request.StartsWith("START_GAME|"))
                    {
                        string roomCode = request.Split('|')[1];

                        int clientCount = ServerBroadcaster.GetRoomClientCount(roomCode);
                        Log($"START_GAME -> room:{roomCode} broadcasting to {clientCount} clients");

                        // Gửi tín hiệu START cho tất cả clients trong phòng
                        ServerBroadcaster.BroadcastToRoom(roomCode, "START");

                        response = "START_OK";
                    }

                    else
                    {
                        response = "Lệnh không hợp lệ.";
                    }

                    // Gửi response
                    if (!string.IsNullOrEmpty(response))
                    {
                        byte[] responseData = Encoding.UTF8.GetBytes(response);
                        stream.Write(responseData, 0, responseData.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
            }
            finally
            {
                // ✅ Xóa client khỏi tất cả các phòng khi disconnect
                ServerBroadcaster.RemoveClient(client);
                client.Close();
            }
        }

        public static class ServerBroadcaster
        {
            // Dictionary: RoomCode -> Dictionary<string (endpoint), TcpClient>
            public static Dictionary<string, Dictionary<string, TcpClient>> Rooms = new Dictionary<string, Dictionary<string, TcpClient>>();
            public static HashSet<string> StartedRooms = new HashSet<string>();
            private static object lockObj = new object();

            public static void AddClientToRoom(string roomCode, TcpClient client)
            {
                lock (lockObj)
                {
                    if (!Rooms.ContainsKey(roomCode))
                        Rooms[roomCode] = new Dictionary<string, TcpClient>();

                    // ✅ DÙNG ENDPOINT LÀM KEY ĐỂ TRÁNH TRÙNG
                    string endpoint = client.Client.RemoteEndPoint.ToString();

                    if (!Rooms[roomCode].ContainsKey(endpoint))
                    {
                        Rooms[roomCode][endpoint] = client;
                    }
                    else
                    {
                        // Update reference nếu reconnect
                        Rooms[roomCode][endpoint] = client;
                    }
                }
            }

            public static void BroadcastToRoom(string roomCode, string msg)
            {
                lock (lockObj)
                {
                    if (!Rooms.ContainsKey(roomCode)) return;

                    // ✅ KIỂM TRA ĐÃ START CHƯA
                    if (msg == "START")
                    {
                        if (StartedRooms.Contains(roomCode))
                        {
                            return;
                        }
                        StartedRooms.Add(roomCode);
                    }

                    byte[] data = Encoding.UTF8.GetBytes(msg);

                    // ✅ LẤY DANH SÁCH CLIENT TỪ DICTIONARY
                    List<string> toRemove = new List<string>();

                    foreach (var kvp in Rooms[roomCode])
                    {
                        try
                        {
                            if (kvp.Value.Connected)
                            {
                                kvp.Value.GetStream().Write(data, 0, data.Length);
                            }
                            else
                            {
                                toRemove.Add(kvp.Key);
                            }
                        }
                        catch
                        {
                            toRemove.Add(kvp.Key);
                        }
                    }

                    // Xóa các clients đã disconnect
                    foreach (var key in toRemove)
                    {
                        Rooms[roomCode].Remove(key);
                    }
                }
            }

            public static void RemoveClient(TcpClient client)
            {
                lock (lockObj)
                {
                    string endpoint = client.Client.RemoteEndPoint.ToString();

                    foreach (var room in Rooms.Values)
                    {
                        if (room.ContainsKey(endpoint))
                            room.Remove(endpoint);
                    }
                }
            }

            // ✅ HÀM HELPER ĐỂ ĐẾM CLIENT TRONG PHÒNG
            public static int GetRoomClientCount(string roomCode)
            {
                lock (lockObj)
                {
                    if (!Rooms.ContainsKey(roomCode))
                        return 0;

                    return Rooms[roomCode].Count;
                }
            }
        }

        private void Log(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Log(message)));
                return;
            }

            lstLog.Items.Add(message);
            lstLog.TopIndex = lstLog.Items.Count - 1;

            if (lstLog.Items.Count > 1000)
                lstLog.Items.RemoveAt(0);
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                isRunning = false;
                listener.Stop();
                lb_status.Text = "🔴 Server đã dừng.";
                Log("Server đã dừng.");
            }
        }

        private void lstClients_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
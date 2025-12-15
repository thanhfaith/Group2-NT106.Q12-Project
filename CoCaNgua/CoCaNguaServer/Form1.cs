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

                // PERSISTENT CONNECTION - Đọc liên tục
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
                        string roomCode = p[2].ToUpper();

                        bool ok = DatabaseHelper.JoinRoom(roomCode, userId);
                        response = ok ? "JOIN_OK" : "JOIN_FAIL";
                    }
                    else if (request.StartsWith("GET_ROOM_PLAYERS|"))
                    {
                        string roomCode = request.Split('|')[1].ToUpper();
                        var players = DatabaseHelper.GetRoomPlayers(roomCode);
                        response = string.Join(",", players);
                    }
                    // Client thông báo đang ở phòng nào
                    else if (request.StartsWith("REGISTER_ROOM|"))
                    {
                        var p = request.Split('|');
                        int userId = int.Parse(p[1]);
                        string roomCode = p[2].ToUpper();

                        string clientEndpoint = client.Client.RemoteEndPoint.ToString();

                        ServerBroadcaster.AddClientToRoom(roomCode, client);

                        int clientCount = ServerBroadcaster.GetRoomClientCount(roomCode);

                        response = "REGISTERED";

                        Log($"REGISTER_ROOM -> user:{userId} room:{roomCode} endpoint:{clientEndpoint} (clients in room: {clientCount})");
                    }

                    // Broadcast cho tất cả trong phòng + Phân đội
                    else if (request.StartsWith("START_GAME|"))
                    {
                        string roomCode = request.Split('|')[1].ToUpper();

                        int clientCount = ServerBroadcaster.GetRoomClientCount(roomCode);
                        Log($"START_GAME -> room:{roomCode} broadcasting to {clientCount} clients");

                        // Gửi tín hiệu START cho tất cả clients trong phòng
                        ServerBroadcaster.BroadcastToRoom(roomCode, "START");

                        // ĐỢI CLIENT MỞ CHESSBOARD (500ms)
                        Thread.Sleep(500);

                        // PHÂN ĐỘI CHO CÁC CLIENT
                        var clients = ServerBroadcaster.Rooms[roomCode].Values.ToList();
                        string[] teams = { "Red", "Green", "Yellow", "Blue" };

                        // teamCount = số người thật (tối đa 4)
                        int teamCount = Math.Min(clients.Count, 4);

                        // Lưu danh sách team thật sự có trong phòng
                        var activeTeams = teams.Take(teamCount).ToList();
                        ServerBroadcaster.SetRoomTeams(roomCode, activeTeams);

                        for (int i = 0; i < teamCount; i++)
                        {
                            try
                            {
                                byte[] assignData = Encoding.UTF8.GetBytes($"ASSIGN|{teams[i]}");
                                clients[i].GetStream().Write(assignData, 0, assignData.Length);
                                Log($"ASSIGN -> room:{roomCode} client:{i} team:{teams[i]}");
                            }
                            catch (Exception ex)
                            {
                                Log($"ASSIGN Error: {ex.Message}");
                            }
                        }

                        Thread.Sleep(200);
                        string firstTurn = activeTeams[0];
                        ServerBroadcaster.SetRoomTurn(roomCode, firstTurn);
                        ServerBroadcaster.BroadcastToRoom(roomCode, $"TURN|{firstTurn}");
                        Log($"TURN -> room:{roomCode} first turn: {firstTurn}");
                        response = "START_OK";
                    }

                    // ============= GAME LOGIC =============

                    else if (request.StartsWith("ROLL"))
                    {
                        string roomCode = ServerBroadcaster.GetClientRoom(client);
                        if (roomCode != null)
                        {
                            Random rand = new Random();
                            int diceValue = RollDiceWeighted();

                            ServerBroadcaster.BroadcastToRoom(roomCode, $"DICE|{diceValue}");
                            Log($"ROLL -> room:{roomCode} dice:{diceValue}");
                        }
                        response = "ROLL_OK";
                    }

                    else if (request.StartsWith("MOVE|"))
                    {
                        string roomCode = ServerBroadcaster.GetClientRoom(client);
                        if (roomCode != null)
                        {
                            ServerBroadcaster.BroadcastToRoom(roomCode, request);
                            Log($"MOVE -> room:{roomCode} data:{request}");
                        }
                        response = "MOVE_OK";
                    }
                    else if (request.StartsWith("END_TURN"))
                    {
                        string roomCode = ServerBroadcaster.GetClientRoom(client);
                        if (roomCode != null)
                        {
                            // Lấy danh sách team thật sự có trong phòng
                            var teamOrder = ServerBroadcaster.GetRoomTeams(roomCode);
                            if (teamOrder == null || teamOrder.Count == 0)
                                teamOrder = new List<string> { "Red", "Green", "Yellow", "Blue" };

                            string currentTurn = ServerBroadcaster.GetRoomTurn(roomCode);

                            int idx = teamOrder.IndexOf(currentTurn);
                            if (idx < 0) idx = 0;

                            int nextIdx = (idx + 1) % teamOrder.Count;
                            string nextTurn = teamOrder[nextIdx];

                            ServerBroadcaster.SetRoomTurn(roomCode, nextTurn);
                            ServerBroadcaster.BroadcastToRoom(roomCode, $"TURN|{nextTurn}");
                            Log($"END_TURN -> room:{roomCode} next turn: {nextTurn} (players={teamOrder.Count})");
                        }
                        response = "TURN_OK";
                    }


                    else if (request.StartsWith("DONE"))
                    {
                        string roomCode = ServerBroadcaster.GetClientRoom(client);
                        if (roomCode != null)
                        {
                            Log($"WINNER FOUND in room {roomCode}. Broadcasting GAME_OVER immediately.");

                            // Gửi lệnh GAME_OVER tới TẤT CẢ client trong phòng bao gồm cả người thắng
                            ServerBroadcaster.BroadcastToRoom(roomCode, "GAME_OVER");
                        }
                        response = "DONE_OK";
                    }

                    else if (request.StartsWith("CHAT|"))
                    {
                        string roomCode = ServerBroadcaster.GetClientRoom(client);
                        if (roomCode != null)
                        {
                            string message = request.Substring(5);
                            ServerBroadcaster.BroadcastToRoom(roomCode, $"CHAT|{message}");
                            Log($"CHAT -> room:{roomCode} msg:{message}");
                        }
                        response = "CHAT_OK";
                    }

                    else
                    {
                        response = "Lệnh không hợp lệ.";
                    }

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
                // Xóa client khỏi tất cả các phòng khi disconnect
                ServerBroadcaster.RemoveClient(client);
                client.Close();
            }
        }

        public static class ServerBroadcaster
        {
            // Dictionary: RoomCode -> Dictionary<string (endpoint), TcpClient>
            public static Dictionary<string, Dictionary<string, TcpClient>> Rooms = new Dictionary<string, Dictionary<string, TcpClient>>();

            // Track client thuộc phòng nào
            public static Dictionary<TcpClient, string> ClientToRoom = new Dictionary<TcpClient, string>();

            // Track lượt hiện tại của từng phòng
            public static Dictionary<string, string> RoomTurns = new Dictionary<string, string>();
            public static Dictionary<string, List<string>> RoomTeamOrders = new Dictionary<string, List<string>>();

            // Track số người đã về đích
            public static Dictionary<string, int> RoomRanks = new Dictionary<string, int>();

            public static HashSet<string> StartedRooms = new HashSet<string>();
            private static object lockObj = new object();
            // SET/GET danh sách team theo phòng
            public static void SetRoomTeams(string roomCode, List<string> teams)
            {
                lock (lockObj)
                {
                    RoomTeamOrders[roomCode] = teams;
                }
            }

            public static List<string> GetRoomTeams(string roomCode)
            {
                lock (lockObj)
                {
                    if (RoomTeamOrders.ContainsKey(roomCode))
                        return RoomTeamOrders[roomCode];

                    return new List<string> { "Red", "Green", "Yellow", "Blue" };
                }
            }

            public static void AddClientToRoom(string roomCode, TcpClient client)
            {
                lock (lockObj)
                {
                    if (!Rooms.ContainsKey(roomCode))
                        Rooms[roomCode] = new Dictionary<string, TcpClient>();

                    string endpoint = client.Client.RemoteEndPoint.ToString();

                    if (!Rooms[roomCode].ContainsKey(endpoint))
                    {
                        Rooms[roomCode][endpoint] = client;
                    }
                    else
                    {
                        Rooms[roomCode][endpoint] = client;
                    }

                    ClientToRoom[client] = roomCode;
                }
            }

            public static void BroadcastToRoom(string roomCode, string msg)
            {
                lock (lockObj)
                {
                    if (!Rooms.ContainsKey(roomCode)) return;

                    if (msg == "START")
                    {
                        if (StartedRooms.Contains(roomCode))
                        {
                            return;
                        }
                        StartedRooms.Add(roomCode);
                    }

                    byte[] data = Encoding.UTF8.GetBytes(msg);
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
                    try
                    {
                        string endpoint = client.Client.RemoteEndPoint.ToString();

                        foreach (var room in Rooms.Values)
                        {
                            if (room.ContainsKey(endpoint))
                                room.Remove(endpoint);
                        }

                        if (ClientToRoom.ContainsKey(client))
                            ClientToRoom.Remove(client);
                    }
                    catch { }
                }
            }

            public static int GetRoomClientCount(string roomCode)
            {
                lock (lockObj)
                {
                    if (!Rooms.ContainsKey(roomCode))
                        return 0;

                    return Rooms[roomCode].Count;
                }
            }

            // HÀM HELPER ĐỂ LẤY ROOMCODE CỦA CLIENT
            public static string GetClientRoom(TcpClient client)
            {
                lock (lockObj)
                {
                    return ClientToRoom.ContainsKey(client) ? ClientToRoom[client] : null;
                }
            }

            // QUẢN LÝ LƯỢT CHƠI
            public static string GetRoomTurn(string roomCode)
            {
                lock (lockObj)
                {
                    return RoomTurns.ContainsKey(roomCode) ? RoomTurns[roomCode] : "Red";
                }
            }

            public static void SetRoomTurn(string roomCode, string turn)
            {
                lock (lockObj)
                {
                    RoomTurns[roomCode] = turn;
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
        static Random diceRand = new Random();

        int RollDiceWeighted()
        {
            int[] dice = { 1, 2, 3, 4, 5, 6 };
            int[] weight = { 15, 15, 13, 12, 10, 35 };
            int roll = diceRand.Next(1, 101); 
            int sum = 0;

            for (int i = 0; i < dice.Length; i++)
            {
                sum += weight[i];
                if (roll <= sum)
                    return dice[i];
            }

            return 1;
        }
        private void lstClients_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
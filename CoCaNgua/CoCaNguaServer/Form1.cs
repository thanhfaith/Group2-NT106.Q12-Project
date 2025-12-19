using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        static Random diceRand = new Random();

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
                lb_status.Text = "Server đang chạy...";
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
                StringBuilder messageBuffer = new StringBuilder(); 

                while (client.Connected)
                {
                    int byteCount = stream.Read(buffer, 0, buffer.Length);
                    if (byteCount == 0) break;

                    string data = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    messageBuffer.Append(data);

                    string allData = messageBuffer.ToString();
                    int newlineIndex;

                    while ((newlineIndex = allData.IndexOf('\n')) >= 0)
                    {
                        string request = allData.Substring(0, newlineIndex).Trim();
                        allData = allData.Substring(newlineIndex + 1);

                        if (string.IsNullOrEmpty(request)) continue;

                        string response = "";

                        // === XỬ LÝ CÁC LỆNH ===
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
                                var (userId, username) = DatabaseHelper.GetUserInfo(parts[1], parts[2]);

                                if (userId > 0)
                                {
                                    ServerBroadcaster.SetClientUsername(client, username);
                                    response = $"LOGIN_OK|{userId}|{username}";
                                    Log($"LOGIN -> user:{username} (id:{userId})");
                                }
                                else
                                {
                                    response = "LOGIN_FAIL";
                                    Log($"LOGIN_FAIL -> username/email:{parts[1]}");
                                }
                            }
                        }
                        else if (request.StartsWith("FORGOT_PASSWORD|"))
                        {
                            string email = request.Split('|')[1];
                            string otp = diceRand.Next(100000, 999999).ToString();

                            if (DatabaseHelper.VerifyEmailAndSaveOTP(email, otp))
                            {
                                SendOTPEmail(email, otp);
                                response = "FORGOT_OK";
                                Log($"FORGOT_PASS -> Gửi OTP cho {email}");
                            }
                            else
                            {
                                response = "FORGOT_NOT_FOUND";
                                Log($"FORGOT_PASS_FAIL -> Không tìm thấy email {email}");
                            }
                        }
                        else if (request.StartsWith("RESET_PASSWORD|"))
                        {
                            string[] parts = request.Split('|');
                            string email = parts[1];
                            string otp = parts[2];
                            string newPassHash = parts[3];

                            bool success = DatabaseHelper.ServerResetPassword(email, otp, newPassHash);
                            if (success)
                            {
                                response = "RESET_OK";
                                Log($"RESET_PASS_SUCCESS -> Email: {email}");
                            }
                            else
                            {
                                response = "RESET_FAILED";
                                Log($"RESET_PASS_FAIL -> OTP sai hoặc hết hạn: {email}");
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
                        else if (request.StartsWith("REGISTER_ROOM|"))
                        {
                            var p = request.Split('|');
                            int userId = int.Parse(p[1]);
                            string roomCode = p[2].ToUpper();

                            string username = DatabaseHelper.GetUsername(userId);
                            ServerBroadcaster.SetClientUsername(client, username);
                            ServerBroadcaster.AddClientToRoom(roomCode, client);

                            response = "REGISTERED";
                        }
                        else if (request.StartsWith("START_GAME|"))
                        {
                            string roomCode = request.Split('|')[1].ToUpper();

                            int clientCount = ServerBroadcaster.GetRoomClientCount(roomCode);
                            Log($"START_GAME -> room:{roomCode} broadcasting to {clientCount} clients");

                            ServerBroadcaster.BroadcastToRoom(roomCode, "START");
                            Thread.Sleep(500);

                            var clients = ServerBroadcaster.Rooms[roomCode].Values.ToList();
                            string[] teams = { "Red", "Green", "Yellow", "Blue" };
                            int teamCount = Math.Min(clients.Count, 4);
                            var activeTeams = teams.Take(teamCount).ToList();
                            ServerBroadcaster.SetRoomTeams(roomCode, activeTeams);

                            for (int i = 0; i < teamCount; i++)
                            {
                                try
                                {
                                    byte[] assignData = Encoding.UTF8.GetBytes($"ASSIGN|{teams[i]}\n"); // THÊM \n
                                    clients[i].GetStream().Write(assignData, 0, assignData.Length);
                                    clients[i].GetStream().Flush(); // THÊM FLUSH
                                    ServerBroadcaster.SetClientTeam(clients[i], teams[i]);
                                    Log($"ASSIGN -> room:{roomCode} client:{i} team:{teams[i]}");
                                }
                                catch (Exception ex)
                                {
                                    Log($"ASSIGN Error: {ex.Message}");
                                }
                            }

                            var nameParts = new List<string> { "NAMES" };
                            for (int i = 0; i < teamCount; i++)
                            {
                                string team = teams[i];
                                string uname = ServerBroadcaster.GetClientUsername(clients[i]);
                                nameParts.Add($"{team}:{uname}");
                            }
                            ServerBroadcaster.BroadcastToRoom(roomCode, string.Join("|", nameParts));
                            Log($"NAMES -> room:{roomCode} {string.Join("|", nameParts)}");

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
                                int diceValue = RollDiceWeighted();
                                ServerBroadcaster.BroadcastToRoom(roomCode, $"DICE|{diceValue}");
                                Log($"ROLL -> room:{roomCode} dice:{diceValue}");
                            }
                            // KHÔNG GỬI RESPONSE
                        }
                        else if (request.StartsWith("MOVE|"))
                        {
                            string roomCode = ServerBroadcaster.GetClientRoom(client);
                            if (roomCode != null)
                            {
                                ServerBroadcaster.BroadcastToRoom(roomCode, request);
                                Log($"MOVE -> room:{roomCode} data:{request}");
                            }
                            // KHÔNG GỬI RESPONSE
                        }
                        else if (request.StartsWith("END_TURN"))
                        {
                            string roomCode = ServerBroadcaster.GetClientRoom(client);
                            if (roomCode != null)
                            {
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
                            // KHÔNG GỬI RESPONSE
                        }
                        else if (request.StartsWith("DONE"))
                        {
                            string roomCode = ServerBroadcaster.GetClientRoom(client);
                            if (roomCode != null)
                            {
                                int rank = ServerBroadcaster.IncrementRank(roomCode);
                                string winnerTeam = ServerBroadcaster.GetClientTeam(client);

                                ServerBroadcaster.BroadcastToRoom(roomCode, $"RANK|{winnerTeam}|{rank}");
                                Log($"DONE -> room:{roomCode} team:{winnerTeam} rank:{rank}");

                                if (rank == 1)
                                {
                                    ServerBroadcaster.BroadcastToRoom(roomCode, $"GAME_OVER|{winnerTeam}");
                                    Log($"GAME_OVER -> room:{roomCode} winner:{winnerTeam}");
                                }
                            }
                            // KHÔNG GỬI RESPONSE
                        }
                        else if (request.StartsWith("CHAT|"))
                        {
                            string roomCode = ServerBroadcaster.GetClientRoom(client);
                            if (roomCode != null)
                            {
                                var parts = request.Split(new[] { '|' }, 3);
                                if (parts.Length >= 3)
                                {
                                    string senderName = parts[1];
                                    string message = parts[2];
                                    string team = ServerBroadcaster.GetClientTeam(client);

                                    ServerBroadcaster.BroadcastToRoom(roomCode, $"CHAT|{team}|{senderName}|{message}");
                                    Log($"CHAT -> room:{roomCode} from:{senderName} msg:{message}");
                                }
                            }
                            // KHÔNG GỬI RESPONSE
                        }
                        else
                        {
                            response = "Lệnh không hợp lệ.";
                        }

                        // GỬI RESPONSE VỚI DELIMITER
                        if (!string.IsNullOrEmpty(response))
                        {
                            byte[] responseData = Encoding.UTF8.GetBytes(response + "\n");
                            stream.Write(responseData, 0, responseData.Length);
                            stream.Flush(); // THÊM FLUSH
                        }
                    }

                    // Cập nhật buffer
                    messageBuffer.Clear();
                    messageBuffer.Append(allData);
                }
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
            }
            finally
            {
                ServerBroadcaster.RemoveClient(client);
                client.Close();
            }
        }

        public static class ServerBroadcaster
        {
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

            public static Dictionary<TcpClient, string> ClientToTeam = new Dictionary<TcpClient, string>();

            public static void SetClientTeam(TcpClient client, string team)
            {
                lock (lockObj) ClientToTeam[client] = team;
            }

            public static string GetClientTeam(TcpClient client)
            {
                lock (lockObj) return ClientToTeam.ContainsKey(client) ? ClientToTeam[client] : "Unknown";
            }

            public static Dictionary<TcpClient, string> ClientToUsername = new Dictionary<TcpClient, string>();

            public static void SetClientUsername(TcpClient client, string username)
            {
                lock (lockObj) ClientToUsername[client] = username;
            }

            public static string GetClientUsername(TcpClient client)
            {
                lock (lockObj) return ClientToUsername.ContainsKey(client) ? ClientToUsername[client] : "Player";
            }

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

                    // mặc định nếu chưa set
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

                    // LƯU MAPPING
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

                    byte[] data = Encoding.UTF8.GetBytes(msg + "\n");
                    List<string> toRemove = new List<string>();

                    foreach (var kvp in Rooms[roomCode])
                    {
                        try
                        {
                            if (kvp.Value.Connected)
                            {
                                kvp.Value.GetStream().Write(data, 0, data.Length);
                                kvp.Value.GetStream().Flush();
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

                        // Xóa mapping phòng
                        if (ClientToRoom.ContainsKey(client))
                            ClientToRoom.Remove(client);

                        // XÓA TEAM
                        if (ClientToTeam.ContainsKey(client))
                            ClientToTeam.Remove(client);

                        // XÓA USERNAME
                        if (ClientToUsername.ContainsKey(client))
                            ClientToUsername.Remove(client);
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

            // QUẢN LÝ XẾP HẠNG
            public static int IncrementRank(string roomCode)
            {
                lock (lockObj)
                {
                    if (!RoomRanks.ContainsKey(roomCode))
                        RoomRanks[roomCode] = 0;

                    RoomRanks[roomCode]++;
                    return RoomRanks[roomCode];
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
                lb_status.Text = " Server đã dừng.";
                Log("Server đã dừng.");
            }
        }

        int RollDiceWeighted()
        {
            int[] dice = { 1, 2, 3, 4, 5, 6 };
            int[] weight = { 20, 10, 10, 10, 10, 40 };
            // 1:13% | 2:13% | 3:13% | 4:13% | 5:13% | 6:35%

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

        private void SendOTPEmail(string toEmail, string otp)
        {
            string fromEmail = "24520209@gm.uit.edu.vn";
            string appPassword = "xris kypu atzs zqsk";
            string displayName = "Game Cờ Cá Ngựa";

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail, displayName);
                mail.To.Add(toEmail);
                mail.Subject = "Mã xác thực đặt lại mật khẩu";

                mail.Body = $@"
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
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(fromEmail, appPassword),
                    EnableSsl = true
                };

                smtp.Send(mail);
                Log($"[EMAIL] Đã gửi OTP thành công tới: {toEmail}");
            }
            catch (Exception ex)
            {
                Log($"[EMAIL ERROR] Lỗi gửi mail: {ex.Message}");
            }
        }

        private void lstClients_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
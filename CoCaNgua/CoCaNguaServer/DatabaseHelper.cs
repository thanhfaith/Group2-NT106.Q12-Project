using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CoCaNguaServer
{
    internal class DatabaseHelper
    {
        private static string connStr =
            //@"Data Source=DESKTOP-JCO2TD6;
            @"Data Source=DESKTOP-JCO2TD6;
  Initial Catalog=GameDB;
  Integrated Security=True";

        private static readonly object joinRoomLock = new object();

        private static bool IsUserExists(string username, string email)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @u OR Email = @e";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@e", email);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public static bool RegisterUser(string username, string email, string passwordHash)
        {
            if (IsUserExists(username, email))
                return false;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"INSERT INTO Users (Username, Password, Email)
                                 VALUES (@u, @p, @e)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", passwordHash);
                cmd.Parameters.AddWithValue("@e", email);

                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public static bool CheckLogin(string usernameOrEmail, string passwordHash)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"SELECT COUNT(*) FROM Users
                                 WHERE (Username = @u OR Email = @u) AND Password = @p";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", usernameOrEmail);
                cmd.Parameters.AddWithValue("@p", passwordHash);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private static string GenerateRoomCode()
        {
            Random rand = new Random();
            return rand.Next(1000, 9999).ToString(); 
        }

        public static string CreateRoom(int hostUserId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string roomCode = GenerateRoomCode();

                string sql = @"
                INSERT INTO Rooms(RoomCode, HostUserId)
                VALUES (@code, @hostId);

                SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@code", roomCode);
                cmd.Parameters.AddWithValue("@hostId", hostUserId);

                int roomId = Convert.ToInt32(cmd.ExecuteScalar());

                // Chủ phòng vào RoomPlayers
                string joinSql = @"
                INSERT INTO RoomPlayers(RoomId, UserId, SeatIndex)
                VALUES (@roomId, @userId, 0)";

                SqlCommand joinCmd = new SqlCommand(joinSql, conn);
                joinCmd.Parameters.AddWithValue("@roomId", roomId);
                joinCmd.Parameters.AddWithValue("@userId", hostUserId);
                joinCmd.ExecuteNonQuery();

                return roomCode;
            }
        }

        // ✅ FIX RACE CONDITION: Thêm lock + transaction + check số lượng người
        public static bool JoinRoom(string roomCode, int userId)
        {
            // Lock để chỉ 1 thread xử lý JOIN_ROOM tại 1 thời điểm
            lock (joinRoomLock)
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Bắt đầu transaction để đảm bảo atomic operation
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Kiểm tra phòng tồn tại và chưa chơi
                            string sql = "SELECT RoomId FROM Rooms WHERE RoomCode=@code AND Status=0";
                            SqlCommand cmd = new SqlCommand(sql, conn, transaction);
                            cmd.Parameters.AddWithValue("@code", roomCode);

                            object result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                transaction.Rollback();
                                return false;
                            }

                            int roomId = Convert.ToInt32(result);

                            // 2. Check đã trong phòng chưa
                            string checkSql = @"SELECT COUNT(*) FROM RoomPlayers 
                                        WHERE RoomId=@r AND UserId=@u";
                            SqlCommand checkCmd = new SqlCommand(checkSql, conn, transaction);
                            checkCmd.Parameters.AddWithValue("@r", roomId);
                            checkCmd.Parameters.AddWithValue("@u", userId);

                            if ((int)checkCmd.ExecuteScalar() > 0)
                            {
                                transaction.Commit();
                                return true; // Đã trong phòng rồi
                            }

                            // ✅ 3. THÊM CHECK SỐ LƯỢNG NGƯỜI (ví dụ tối đa 4 người)
                            string countSql = "SELECT COUNT(*) FROM RoomPlayers WHERE RoomId=@r";
                            SqlCommand countCmd = new SqlCommand(countSql, conn, transaction);
                            countCmd.Parameters.AddWithValue("@r", roomId);

                            int currentPlayers = (int)countCmd.ExecuteScalar();

                            if (currentPlayers >= 4) // Giới hạn 4 người
                            {
                                transaction.Rollback();
                                return false; // Phòng đã đầy
                            }

                            // 4. Thêm vào phòng
                            string joinSql = @"INSERT INTO RoomPlayers(RoomId, UserId)
                                       VALUES (@r, @u)";
                            SqlCommand joinCmd = new SqlCommand(joinSql, conn, transaction);
                            joinCmd.Parameters.AddWithValue("@r", roomId);
                            joinCmd.Parameters.AddWithValue("@u", userId);
                            joinCmd.ExecuteNonQuery();

                            // Commit transaction
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
        }

        public static List<string> GetRoomPlayers(string roomCode)
        {
            List<string> players = new List<string>();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string sql = @"
                SELECT u.Username
                FROM RoomPlayers rp
                JOIN Rooms r ON rp.RoomId = r.RoomId
                JOIN Users u ON rp.UserId = u.UserId
                WHERE r.RoomCode = @code";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@code", roomCode);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    players.Add(reader.GetString(0));
                }
            }
            return players;
        }

        public static int GetUserId(string usernameOrEmail, string passwordHash)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"SELECT UserId FROM Users 
                       WHERE (Username=@u OR Email=@u) AND Password=@p";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", usernameOrEmail);
                cmd.Parameters.AddWithValue("@p", passwordHash);

                object result = cmd.ExecuteScalar();
                return result == null ? -1 : Convert.ToInt32(result);
            }
        }
    }
}
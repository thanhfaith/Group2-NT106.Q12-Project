using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CoCaNguaServer
{
    internal class DatabaseHelper
    {
        // TODO: sửa Data Source cho đúng tên SQL Server của bạn
        private static string connStr =
            @"Data Source=DESKTOP-9Q6P0AS\MSSQLSERVER01;
  Initial Catalog=GameDB;
  Integrated Security=True";

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
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
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
        public static bool JoinRoom(string roomCode, int userId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Kiểm tra phòng tồn tại và chưa chơi
                string sql = "SELECT RoomId FROM Rooms WHERE RoomCode=@code AND Status=0";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@code", roomCode);

                object result = cmd.ExecuteScalar();
                if (result == null)
                    return false;

                int roomId = Convert.ToInt32(result);

                // Check đã trong phòng chưa
                string checkSql = @"SELECT COUNT(*) FROM RoomPlayers 
                            WHERE RoomId=@r AND UserId=@u";
                SqlCommand checkCmd = new SqlCommand(checkSql, conn);
                checkCmd.Parameters.AddWithValue("@r", roomId);
                checkCmd.Parameters.AddWithValue("@u", userId);

                if ((int)checkCmd.ExecuteScalar() > 0)
                    return true;

                // Thêm vào phòng
                string joinSql = @"INSERT INTO RoomPlayers(RoomId, UserId)
                           VALUES (@r, @u)";
                SqlCommand joinCmd = new SqlCommand(joinSql, conn);
                joinCmd.Parameters.AddWithValue("@r", roomId);
                joinCmd.Parameters.AddWithValue("@u", userId);
                joinCmd.ExecuteNonQuery();

                return true;
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

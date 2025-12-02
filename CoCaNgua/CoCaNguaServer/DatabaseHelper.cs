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
            @"Data Source=LAPTOP-EDR1OHEC;Initial Catalog=GameDB;Integrated Security=True";

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
    }
}

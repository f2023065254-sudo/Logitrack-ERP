using Microsoft.Data.SqlClient;
using System;

namespace Logitrack_ERP.Models
{
    public class User_DAL
    {
        // Check if Username or Email already exists
        public bool CheckUserExists(string? conn, string username, string email)
        {
            bool exists = false;
            string query = "SELECT COUNT(1) FROM Users WHERE UserName = @UserName OR Email = @Email";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters.AddWithValue("@Email", email);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0) exists = true;
            }
            return exists;
        }

        // Register New User
        public void RegisterUser(string? conn, User user)
        {
            string query = "INSERT INTO Users (UserName, Email, Password, Role) VALUES (@UserName, @Email, @Password, @Role)";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Role", user.Role);
                cmd.ExecuteNonQuery();
            }
        }

        // Login Authentication
        public User AuthenticateUser(string? conn, string email, string password)
        {
            User user = null;
            string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        UserName = reader["UserName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }
            }
            return user;
        }
    }
}
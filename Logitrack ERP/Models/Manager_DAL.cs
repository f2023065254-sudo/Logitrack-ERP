using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class Manager_DAL
    {
        public void AddManager(string conn, Manager manager)
        {
            string query = @"INSERT INTO MANAGER (ManagerName, Email, Phone, Department, Status)
                             VALUES (@name, @email, @phone, @dept, @status);";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", manager.ManagerName);
                cmd.Parameters.AddWithValue("@email", manager.Email);
                cmd.Parameters.AddWithValue("@phone", manager.Phone);
                cmd.Parameters.AddWithValue("@dept", manager.Department);
                cmd.Parameters.AddWithValue("@status", manager.Status ?? "Active");
                cmd.ExecuteNonQuery();
            }
        }

        public List<Manager> GetAllManagers(string conn)
        {
            List<Manager> list = new List<Manager>();
            string query = "SELECT * FROM MANAGER;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Manager
                    {
                        ManagerID = Convert.ToInt32(reader["ManagerID"]),
                        ManagerName = reader["ManagerName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Department = reader["Department"].ToString(),
                        Status = reader["Status"].ToString()
                    });
                }
            }
            return list;
        }

        public Manager GetManagerById(string conn, int id)
        {
            Manager manager = null;
            string query = "SELECT * FROM MANAGER WHERE ManagerID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    manager = new Manager
                    {
                        ManagerID = Convert.ToInt32(reader["ManagerID"]),
                        ManagerName = reader["ManagerName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Department = reader["Department"].ToString(),
                        Status = reader["Status"].ToString()
                    };
                }
            }
            return manager;
        }

        public void UpdateManager(string conn, Manager manager)
        {
            string query = @"UPDATE MANAGER 
                             SET ManagerName = @name, Email = @email, Phone = @phone, Department = @dept, Status = @status
                             WHERE ManagerID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", manager.ManagerID);
                cmd.Parameters.AddWithValue("@name", manager.ManagerName);
                cmd.Parameters.AddWithValue("@email", manager.Email);
                cmd.Parameters.AddWithValue("@phone", manager.Phone);
                cmd.Parameters.AddWithValue("@dept", manager.Department);
                cmd.Parameters.AddWithValue("@status", manager.Status);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteManager(string conn, int id)
        {
            string query = "DELETE FROM MANAGER WHERE ManagerID = @id;";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
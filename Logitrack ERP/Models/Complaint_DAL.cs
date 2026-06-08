using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class Complaint_DAL
    {
        // 1. LOG COMPLAINT (CREATE)
        public void AddComplaint(string conn, Complaint comp)
        {
            string query = @"INSERT INTO COMPLAINT 
                             (CustomerID, OrderID, ComplaintText, Status, CreatedDate)
                             VALUES 
                             (@customerID, @orderID, @text, @status, @createdDate);";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@customerID", comp.CustomerID);
                cmd.Parameters.AddWithValue("@orderID", comp.OrderID);
                cmd.Parameters.AddWithValue("@text", comp.ComplaintText);
                cmd.Parameters.AddWithValue("@status", comp.Status ?? "Open");
                cmd.Parameters.AddWithValue("@createdDate", string.IsNullOrEmpty(comp.CreatedDate) ? DateTime.Now.ToString("yyyy-MM-dd") : comp.CreatedDate);

                cmd.ExecuteNonQuery();
            }
        }

        // 2. READ ALL (With Join to display Customer Names)
        public List<Complaint> GetAllComplaints(string conn)
        {
            List<Complaint> list = new List<Complaint>();
            string query = @"SELECT c.*, cust.Name AS CustomerName 
                             FROM COMPLAINT c
                             INNER JOIN CUSTOMER cust ON c.CustomerID = cust.CustomerID;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Complaint
                    {
                        ComplaintID = Convert.ToInt32(reader["ComplaintID"]),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        ComplaintText = reader["ComplaintText"].ToString(),
                        Status = reader["Status"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]).ToString("yyyy-MM-dd"),
                        CustomerName = reader["CustomerName"].ToString()
                    });
                }
            }
            return list;
        }

        // 3. READ SINGLE 
        public Complaint GetComplaintById(string conn, int id)
        {
            Complaint comp = null;
            string query = @"SELECT c.*, cust.Name AS CustomerName 
                             FROM COMPLAINT c
                             INNER JOIN CUSTOMER cust ON c.CustomerID = cust.CustomerID
                             WHERE c.ComplaintID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    comp = new Complaint
                    {
                        ComplaintID = Convert.ToInt32(reader["ComplaintID"]),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        ComplaintText = reader["ComplaintText"].ToString(),
                        Status = reader["Status"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]).ToString("yyyy-MM-dd"),
                        CustomerName = reader["CustomerName"].ToString()
                    };
                }
            }
            return comp;
        }

        // 4. UPDATE STATUS / TEXT
        public void UpdateComplaint(string conn, Complaint comp)
        {
            string query = @"UPDATE COMPLAINT 
                             SET CustomerID = @customerID, 
                                 OrderID = @orderID, 
                                 ComplaintText = @text, 
                                 Status = @status,
                                 CreatedDate = @createdDate
                             WHERE ComplaintID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", comp.ComplaintID);
                cmd.Parameters.AddWithValue("@customerID", comp.CustomerID);
                cmd.Parameters.AddWithValue("@orderID", comp.OrderID);
                cmd.Parameters.AddWithValue("@text", comp.ComplaintText);
                cmd.Parameters.AddWithValue("@status", comp.Status);
                cmd.Parameters.AddWithValue("@createdDate", comp.CreatedDate);

                cmd.ExecuteNonQuery();
            }
        }

        // 5. DELETE COMPLAINT
        public void DeleteComplaint(string conn, int id)
        {
            string query = "DELETE FROM COMPLAINT WHERE ComplaintID = @id;";
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
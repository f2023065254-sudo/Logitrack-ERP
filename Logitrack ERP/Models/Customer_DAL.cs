using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class Customer_DAL
    {
        // 1. CREATE (Standard)
        public void AddCustomer(string conn, Customer cust)
        {
            string query = @"INSERT INTO CUSTOMER (Name, Email, Phone, Address) 
                             VALUES (@name, @email, @phone, @address);";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", cust.Name);
                cmd.Parameters.AddWithValue("@email", cust.Email);
                cmd.Parameters.AddWithValue("@phone", cust.Phone);
                cmd.Parameters.AddWithValue("@address", cust.Address);
                cmd.ExecuteNonQuery();
            }
        }

        // 2. READ ALL
        public List<Customer> GetAllCustomers(string conn)
        {
            List<Customer> list = new List<Customer>();
            string query = "SELECT * FROM CUSTOMER;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Customer
                    {
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Address = reader["Address"].ToString()
                    });
                }
            }
            return list;
        }

        // 3. READ SINGLE
        public Customer GetCustomerById(string conn, int id)
        {
            Customer cust = null;
            string query = "SELECT * FROM CUSTOMER WHERE CustomerID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    cust = new Customer
                    {
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Address = reader["Address"].ToString()
                    };
                }
            }
            return cust;
        }

        // 4. UPDATE
        public void UpdateCustomer(string conn, Customer cust)
        {
            string query = @"UPDATE CUSTOMER 
                             SET Name = @name, Email = @email, Phone = @phone, Address = @address 
                             WHERE CustomerID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", cust.CustomerID);
                cmd.Parameters.AddWithValue("@name", cust.Name);
                cmd.Parameters.AddWithValue("@email", cust.Email);
                cmd.Parameters.AddWithValue("@phone", cust.Phone);
                cmd.Parameters.AddWithValue("@address", cust.Address);
                cmd.ExecuteNonQuery();
            }
        }

        // 5. DELETE
        public void DeleteCustomer(string conn, int id)
        {
            string query = "DELETE FROM CUSTOMER WHERE CustomerID = @id;";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // 6. QUICK ADD (For the modal workflow)
        public int QuickAddCustomer(string conn, Customer cust)
        {
            int newId = 0;
            string query = @"INSERT INTO CUSTOMER (Name, Email, Phone, Address) 
                             OUTPUT INSERTED.CustomerID 
                             VALUES (@name, @email, @phone, @address);";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", cust.Name);
                cmd.Parameters.AddWithValue("@email", cust.Email);
                cmd.Parameters.AddWithValue("@phone", cust.Phone);
                cmd.Parameters.AddWithValue("@address", cust.Address);
                newId = (int)cmd.ExecuteScalar();
            }
            return newId;
        }
    }
}
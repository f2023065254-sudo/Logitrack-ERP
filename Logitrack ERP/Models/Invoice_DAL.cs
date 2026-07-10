using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class Invoice_DAL
    {
        // 1. CREATE
        public void CreateOrder(Order order, string? conn)
        {
            // Ek hi query block mein hum Order bhi banayenge aur uski Automatic Invoice bhi!
            string query = @"
                -- 1. Pehle Naya Order Insert Karein
                INSERT INTO Orders 
                (CustomerID, OrderDate, Status, TotalAmount, DeliveryAddress, CustomerName, ContactNo) 
                VALUES 
                (@CustomerID, @OrderDate, @Status, @TotalAmount, @DeliveryAddress, (SELECT Name FROM CUSTOMER WHERE CustomerID = @CustomerID), @ContactNo);

                -- 2. Naye Ban'ne Walay Order ka 'OrderID' Get Karein
                DECLARE @NewOrderID INT = SCOPE_IDENTITY();

                -- 3. Usi OrderID ki madad se AUTOMATIC INVOICE banayein
                -- Note: Due Date by default Order Date se 7 din aagay (DATEADD) ki set ki gayi hai
                INSERT INTO INVOICE 
                (OrderID, CustomerID, InvoiceDate, TotalAmount, DueDate, Status)
                VALUES 
                (@NewOrderID, @CustomerID, GETDATE(), @TotalAmount, DATEADD(day, 7, @OrderDate), 'Unpaid');
            ";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@ContactNo", order.ContactNo);
                cmd.Parameters.AddWithValue("@Status", order.Status.ToString());
                cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                cmd.Parameters.AddWithValue("@DeliveryAddress", order.DeliveryAddress);

                cmd.ExecuteNonQuery();
            }
        }

        // 2. READ ALL
        public List<Invoice> GetAllInvoices(string conn)
        {
            List<Invoice> list = new List<Invoice>();
            string query = "SELECT * FROM INVOICE;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Invoice
                    {
                        InvoiceID = Convert.ToInt32(reader["InvoiceID"]),
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        // Format dates so HTML <input type="date"> can read them
                        InvoiceDate = Convert.ToDateTime(reader["InvoiceDate"]).ToString("yyyy-MM-dd"),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        DueDate = Convert.ToDateTime(reader["DueDate"]).ToString("yyyy-MM-dd"),
                        Status = reader["Status"].ToString()
                    });
                }
            }
            return list;
        }

        // 3. READ SINGLE
        public Invoice GetInvoiceById(string conn, int id)
        {
            Invoice invoice = null;
            string query = "SELECT * FROM INVOICE WHERE InvoiceID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    invoice = new Invoice
                    {
                        InvoiceID = Convert.ToInt32(reader["InvoiceID"]),
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        InvoiceDate = Convert.ToDateTime(reader["InvoiceDate"]).ToString("yyyy-MM-dd"),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                        DueDate = Convert.ToDateTime(reader["DueDate"]).ToString("yyyy-MM-dd"),
                        Status = reader["Status"].ToString()
                    };
                }
            }
            return invoice;
        }

        // 4. UPDATE
        public void UpdateInvoice(string conn, Invoice invoice)
        {
            string query = @"UPDATE INVOICE 
                             SET OrderID = @order, 
                                 CustomerID = @customer, 
                                 InvoiceDate = @invDate, 
                                 TotalAmount = @amount,
                                 DueDate = @dueDate,
                                 Status = @status
                             WHERE InvoiceID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", invoice.InvoiceID);
                cmd.Parameters.AddWithValue("@order", invoice.OrderID);
                cmd.Parameters.AddWithValue("@customer", invoice.CustomerID);
                cmd.Parameters.AddWithValue("@invDate", invoice.InvoiceDate);
                cmd.Parameters.AddWithValue("@amount", invoice.TotalAmount);
                cmd.Parameters.AddWithValue("@dueDate", invoice.DueDate);
                cmd.Parameters.AddWithValue("@status", invoice.Status);

                cmd.ExecuteNonQuery();
            }
        }

        // 5. DELETE
        public void DeleteInvoice(string conn, int id)
        {
            string query = "DELETE FROM INVOICE WHERE InvoiceID = @id;";
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
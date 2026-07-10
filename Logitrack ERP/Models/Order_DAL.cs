using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class Order_DAL
    {
        public List<Order> getallorders(string? conn)
        {
            List<Order> orders = new List<Order>();
            string query = "select * from Orders;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader rows = cmd.ExecuteReader();
                while (rows.Read())
                {
                    orders.Add(new Order
                    {
                        OrderID = Convert.ToInt32(rows["OrderId"].ToString()),
                        CustomerName = rows["CustomerName"].ToString(),
                        CustomerID = Convert.ToInt32(rows["CustomerID"].ToString()),
                        OrderDate = Convert.ToDateTime(rows["OrderDate"].ToString()),
                        ContactNo = rows["ContactNo"].ToString(),
                        Status = Enum.Parse<OrderStatus>(rows["Status"].ToString()),
                        TotalAmount = Convert.ToDouble(rows["TotalAmount"].ToString()),
                        DeliveryAddress = rows["DeliveryAddress"].ToString()
                    });
                }
            }
            return orders;
        }

        public void CreateOrder(Order order, string? conn)
        {
            string query = @"
                -- 1. Naya Order Insert Karein (CustomerName Subquery ke sath)
                INSERT INTO Orders 
                (CustomerID, OrderDate, Status, TotalAmount, DeliveryAddress, CustomerName, ContactNo) 
                VALUES 
                (@CustomerID, @OrderDate, @Status, @TotalAmount, @DeliveryAddress, (SELECT Name FROM CUSTOMER WHERE CustomerID = @CustomerID), @ContactNo);

                -- 2. Naye Ban'ne Walay Order ka 'OrderID' Get Karein
                DECLARE @NewOrderID INT = SCOPE_IDENTITY();

                -- 3. Usi OrderID ki madad se AUTOMATIC INVOICE banayein
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

        public void Update(Order order, string? conn, int id)
        {
            string query = @"UPDATE Orders
                       SET 
                         CustomerID = @CustomerID, 
                         OrderDate = @OrderDate, 
                         Status = @Status, 
                         TotalAmount = @TotalAmount, 
                         DeliveryAddress = @DeliveryAddress,
                         CustomerName = (SELECT Name FROM CUSTOMER WHERE CustomerID = @CustomerID),
                         ContactNo = @ContactNo
                     WHERE OrderID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                cmd.Parameters.AddWithValue("@ContactNo", order.ContactNo);
                cmd.Parameters.AddWithValue("@Status", order.Status.ToString());
                cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                cmd.Parameters.AddWithValue("@DeliveryAddress", order.DeliveryAddress);

                cmd.ExecuteNonQuery();
            }
        }

        public void delete(string? conn, int id)
        {
            string query = @"UPDATE Orders SET Status = 'Cancelled' WHERE OrderID = @id;";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public Order GetOrder(string? conn, int id)
        {
            string query = @"Select * from Orders where OrderID = @Id;";
            Order o = null;
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader rows = cmd.ExecuteReader();
                if (rows.Read())
                {
                    o = new Order
                    {
                        OrderID = Convert.ToInt32(rows["OrderId"].ToString()),
                        CustomerName = rows["CustomerName"].ToString(),
                        CustomerID = Convert.ToInt32(rows["CustomerID"].ToString()),
                        OrderDate = Convert.ToDateTime(rows["OrderDate"].ToString()),
                        ContactNo = rows["ContactNo"].ToString(),
                        Status = Enum.Parse<OrderStatus>(rows["Status"].ToString()),
                        TotalAmount = Convert.ToDouble(rows["TotalAmount"].ToString()),
                        DeliveryAddress = rows["DeliveryAddress"].ToString()
                    };
                }
            }
            return o;
        }

        // ========================================================
        // NAYA METHOD: Sirf ek customer ke orders lane ke liye
        // ========================================================
        public List<Order> GetOrdersByCustomerEmail(string? conn, string email)
        {
            List<Order> orders = new List<Order>();
            string query = @"
                SELECT o.* FROM Orders o
                INNER JOIN CUSTOMER c ON o.CustomerID = c.CustomerID
                WHERE c.Email = @Email;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", email);
                SqlDataReader rows = cmd.ExecuteReader();
                while (rows.Read())
                {
                    orders.Add(new Order
                    {
                        OrderID = Convert.ToInt32(rows["OrderId"].ToString()),
                        CustomerName = rows["CustomerName"].ToString(),
                        CustomerID = Convert.ToInt32(rows["CustomerID"].ToString()),
                        OrderDate = Convert.ToDateTime(rows["OrderDate"].ToString()),
                        ContactNo = rows["ContactNo"].ToString(),
                        Status = Enum.Parse<OrderStatus>(rows["Status"].ToString()),
                        TotalAmount = Convert.ToDouble(rows["TotalAmount"].ToString()),
                        DeliveryAddress = rows["DeliveryAddress"].ToString()
                    });
                }
            }
            return orders;
        }
    }
}
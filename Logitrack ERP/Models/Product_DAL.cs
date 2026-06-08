using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class Product_DAL
    {
        public List<Product> GetAllProducts(string conn)
        {
            List<Product> products = new List<Product>();
            string query = "SELECT * FROM PRODUCT;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                        Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                        Price = Convert.ToDecimal(reader["Price"]),
                        StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                        Unit = reader["Unit"].ToString(),
                        Status = reader["Status"].ToString()
                    });
                }
            }
            return products;
        }

        public void AddProduct(string conn, Product product)
        {
            string query = @"INSERT INTO PRODUCT (ProductName, Description, Price, StockQuantity, Unit, Status) 
                             VALUES (@name, @desc, @price, @stock, @unit, @status);";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", product.ProductName);
                cmd.Parameters.AddWithValue("@desc", string.IsNullOrEmpty(product.Description) ? (object)DBNull.Value : product.Description);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@stock", product.StockQuantity);
                cmd.Parameters.AddWithValue("@unit", product.Unit);

                // Defaults to 'Available' if not provided, matching your DB default
                cmd.Parameters.AddWithValue("@status", string.IsNullOrEmpty(product.Status) ? "Available" : product.Status);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateProduct(string conn, Product product)
        {
            string query = @"UPDATE PRODUCT 
                             SET ProductName = @name, 
                                 Description = @desc, 
                                 Price = @price, 
                                 StockQuantity = @stock, 
                                 Unit = @unit, 
                                 Status = @status 
                             WHERE ProductID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", product.ProductID);
                cmd.Parameters.AddWithValue("@name", product.ProductName);
                cmd.Parameters.AddWithValue("@desc", string.IsNullOrEmpty(product.Description) ? (object)DBNull.Value : product.Description);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@stock", product.StockQuantity);
                cmd.Parameters.AddWithValue("@unit", product.Unit);
                cmd.Parameters.AddWithValue("@status", product.Status);

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(string conn, int id)
        {
            string query = "DELETE FROM PRODUCT WHERE ProductID = @id;";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public Product GetProductById(string conn, int id)
        {
            Product product = null;
            string query = "SELECT * FROM PRODUCT WHERE ProductID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    product = new Product
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                        Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                        Price = Convert.ToDecimal(reader["Price"]),
                        StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                        Unit = reader["Unit"].ToString(),
                        Status = reader["Status"].ToString()
                    };
                }
            }
            return product;
        }
    }
}
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class Inventory_DAL
    {
        // 1. CREATE
        public void AddInventoryItem(string conn, InventoryItem item)
        {
            string query = @"INSERT INTO INVENTORY 
                             (WarehouseID, ProductID, Category, StockLevel, BatchNo, ProductName, LocationStorage)
                             VALUES 
                             (@warehouseId, @productId, @category, @stockLevel, @batchNo, @productName, @locationStorage);";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@warehouseId", item.WarehouseID);
                cmd.Parameters.AddWithValue("@productId", item.ProductID);
                cmd.Parameters.AddWithValue("@category", item.Category);
                cmd.Parameters.AddWithValue("@stockLevel", item.StockLevel);
                cmd.Parameters.AddWithValue("@batchNo", item.BatchNo);
                cmd.Parameters.AddWithValue("@productName", item.ProductName);
                cmd.Parameters.AddWithValue("@locationStorage", item.LocationStorage);

                cmd.ExecuteNonQuery();
            }
        }

        // 2. READ ALL
        public List<InventoryItem> GetAllInventory(string conn)
        {
            List<InventoryItem> list = new List<InventoryItem>();
            string query = "SELECT * FROM INVENTORY;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new InventoryItem
                    {
                        InventoryID = Convert.ToInt32(reader["InventoryID"]),
                        WarehouseID = Convert.ToInt32(reader["WarehouseID"]),
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        Category = reader["Category"].ToString(),
                        StockLevel = Convert.ToInt32(reader["StockLevel"]),
                        BatchNo = reader["BatchNo"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        LocationStorage = reader["LocationStorage"].ToString()
                    });
                }
            }
            return list;
        }

        // 3. READ SINGLE
        public InventoryItem GetInventoryItemById(string conn, int id)
        {
            InventoryItem item = null;
            string query = "SELECT * FROM INVENTORY WHERE InventoryID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    item = new InventoryItem
                    {
                        InventoryID = Convert.ToInt32(reader["InventoryID"]),
                        WarehouseID = Convert.ToInt32(reader["WarehouseID"]),
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        Category = reader["Category"].ToString(),
                        StockLevel = Convert.ToInt32(reader["StockLevel"]),
                        BatchNo = reader["BatchNo"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        LocationStorage = reader["LocationStorage"].ToString()
                    };
                }
            }
            return item;
        }

        // 4. UPDATE
        public void UpdateInventoryItem(string conn, InventoryItem item)
        {
            string query = @"UPDATE INVENTORY 
                             SET WarehouseID = @warehouseId,
                                 ProductID = @productId,
                                 Category = @category, 
                                 StockLevel = @stockLevel,
                                 BatchNo = @batchNo,
                                 ProductName = @productName, 
                                 LocationStorage = @locationStorage
                             WHERE InventoryID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", item.InventoryID);
                cmd.Parameters.AddWithValue("@warehouseId", item.WarehouseID);
                cmd.Parameters.AddWithValue("@productId", item.ProductID);
                cmd.Parameters.AddWithValue("@category", item.Category);
                cmd.Parameters.AddWithValue("@stockLevel", item.StockLevel);
                cmd.Parameters.AddWithValue("@batchNo", item.BatchNo);
                cmd.Parameters.AddWithValue("@productName", item.ProductName);
                cmd.Parameters.AddWithValue("@locationStorage", item.LocationStorage);

                cmd.ExecuteNonQuery();
            }
        }

        // 5. DELETE
        public void DeleteInventoryItem(string conn, int id)
        {
            string query = "DELETE FROM INVENTORY WHERE InventoryID = @id;";
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
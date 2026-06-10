using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class Warehouse_DAL
    {
        // 1. CREATE WAREHOUSE
        public void createWarehouse(string conn, Warehouse warehouse)
        {
            string query = @"INSERT INTO WAREHOUSE 
                             (WarehouseName, Location, Capacity, CurrentStock)
                             VALUES 
                             (@warehousename, @location, @capacity, @currentstock);";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);

                // Using DBNull.Value safely handles empty fields
                cmd.Parameters.AddWithValue("@warehousename", (object)warehouse.WarehouseName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@location", (object)warehouse.Location ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@capacity", warehouse.Capacity);
                cmd.Parameters.AddWithValue("@currentstock", warehouse.CurrentStock);

                cmd.ExecuteNonQuery();
            }
        }

       
        public List<Warehouse> GetAllWarehouses(string conn)
        {
            List<Warehouse> warehouses = new List<Warehouse>();
            string query = "SELECT * FROM WAREHOUSE;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader rows = cmd.ExecuteReader();

                while (rows.Read())
                {
                    warehouses.Add(new Warehouse
                    {
                        WarehouseID = Convert.ToInt32(rows["WarehouseID"]),
                        WarehouseName = rows["WarehouseName"].ToString(),
                        Location = rows["Location"].ToString(),
                        Capacity = Convert.ToDouble(rows["Capacity"]),
                        CurrentStock = Convert.ToDouble(rows["CurrentStock"])
                    });
                }
            }
            return warehouses;
        }

     
        public Warehouse GetWarehouseById(string conn, int id)
        {
            Warehouse warehouse = null;
            string query = "SELECT * FROM WAREHOUSE WHERE WarehouseID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader rows = cmd.ExecuteReader();

                if (rows.Read())
                {
                    warehouse = new Warehouse
                    {
                        WarehouseID = Convert.ToInt32(rows["WarehouseID"]),
                        WarehouseName = rows["WarehouseName"].ToString(),
                        Location = rows["Location"].ToString(),
                        Capacity = Convert.ToDouble(rows["Capacity"]),
                        CurrentStock = Convert.ToDouble(rows["CurrentStock"])
                    };
                }
            }
            return warehouse;
        }

        // 4. UPDATE WAREHOUSE
        public void updateWarehouse(string conn, Warehouse warehouse)
        {
            string query = @"UPDATE WAREHOUSE 
                             SET WarehouseName = @warehousename, 
                                 Location = @location, 
                                 Capacity = @capacity, 
                                 CurrentStock = @currentstock 
                             WHERE WarehouseID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", warehouse.WarehouseID);
                cmd.Parameters.AddWithValue("@warehousename", (object)warehouse.WarehouseName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@location", (object)warehouse.Location ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@capacity", warehouse.Capacity);
                cmd.Parameters.AddWithValue("@currentstock", warehouse.CurrentStock);

                cmd.ExecuteNonQuery();
            }
        }

  
        public void deletewarehouse(string conn, int id)
        {
            string query = "DELETE FROM WAREHOUSE WHERE WarehouseID = @id;";

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
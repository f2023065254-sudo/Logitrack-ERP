using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class Vehicle_DAL
    {
        // 1. CREATE
        public void AddVehicle(string conn, Vehicle vehicle)
        {
            string query = @"INSERT INTO VEHICLE 
                             (VehicleNumber, VehicleType, Capacity, Status, CurrentDriver, NextMaintenanceDate)
                             VALUES 
                             (@number, @type, @capacity, @status, @driver, @maintenance);";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@number", vehicle.VehicleNumber);
                cmd.Parameters.AddWithValue("@type", vehicle.VehicleType);
                cmd.Parameters.AddWithValue("@capacity", vehicle.Capacity);
                cmd.Parameters.AddWithValue("@status", vehicle.Status ?? "Available");
                cmd.Parameters.AddWithValue("@driver", vehicle.CurrentDriver);
                cmd.Parameters.AddWithValue("@maintenance", vehicle.NextMaintenanceDate);

                cmd.ExecuteNonQuery();
            }
        }

        // 2. READ ALL
        public List<Vehicle> GetAllVehicles(string conn)
        {
            List<Vehicle> list = new List<Vehicle>();
            string query = "SELECT * FROM VEHICLE;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Vehicle
                    {
                        VehicleID = Convert.ToInt32(reader["VehicleID"]),
                        VehicleNumber = reader["VehicleNumber"].ToString(),
                        VehicleType = reader["VehicleType"].ToString(),
                        Capacity = Convert.ToDecimal(reader["Capacity"]),
                        Status = reader["Status"].ToString(),
                        CurrentDriver = reader["CurrentDriver"].ToString(),
                        NextMaintenanceDate = reader["NextMaintenanceDate"].ToString()
                    });
                }
            }
            return list;
        }

        // 3. READ SINGLE
        public Vehicle GetVehicleById(string conn, int id)
        {
            Vehicle vehicle = null;
            string query = "SELECT * FROM VEHICLE WHERE VehicleID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    vehicle = new Vehicle
                    {
                        VehicleID = Convert.ToInt32(reader["VehicleID"]),
                        VehicleNumber = reader["VehicleNumber"].ToString(),
                        VehicleType = reader["VehicleType"].ToString(),
                        Capacity = Convert.ToDecimal(reader["Capacity"]),
                        Status = reader["Status"].ToString(),
                        CurrentDriver = reader["CurrentDriver"].ToString(),
                        NextMaintenanceDate = reader["NextMaintenanceDate"].ToString()
                    };
                }
            }
            return vehicle;
        }

        // 4. UPDATE
        public void UpdateVehicle(string conn, Vehicle vehicle)
        {
            string query = @"UPDATE VEHICLE 
                             SET VehicleNumber = @number, 
                                 VehicleType = @type, 
                                 Capacity = @capacity, 
                                 Status = @status,
                                 CurrentDriver = @driver,
                                 NextMaintenanceDate = @maintenance
                             WHERE VehicleID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", vehicle.VehicleID);
                cmd.Parameters.AddWithValue("@number", vehicle.VehicleNumber);
                cmd.Parameters.AddWithValue("@type", vehicle.VehicleType);
                cmd.Parameters.AddWithValue("@capacity", vehicle.Capacity);
                cmd.Parameters.AddWithValue("@status", vehicle.Status);
                cmd.Parameters.AddWithValue("@driver", vehicle.CurrentDriver);
                cmd.Parameters.AddWithValue("@maintenance", vehicle.NextMaintenanceDate);

                cmd.ExecuteNonQuery();
            }
        }

        // 5. DELETE
        public void DeleteVehicle(string conn, int id)
        {
            string query = "DELETE FROM VEHICLE WHERE VehicleID = @id;";
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
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class Driver_DAL
    {
        // 1. CREATE
        public void AddDriver(string conn, Driver driver)
        {
            string query = @"INSERT INTO DRIVER 
                             (DriverName, Phone, LicenseNumber, AssignedVehicle, Status)
                             VALUES 
                             (@name, @phone, @license, @vehicle, @status);";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", driver.DriverName);
                cmd.Parameters.AddWithValue("@phone", driver.Phone);
                cmd.Parameters.AddWithValue("@license", driver.LicenseNumber);
                cmd.Parameters.AddWithValue("@vehicle", driver.AssignedVehicle);
                cmd.Parameters.AddWithValue("@status", driver.Status ?? "Available");

                cmd.ExecuteNonQuery();
            }
        }

        // 2. READ ALL
        public List<Driver> GetAllDrivers(string conn)
        {
            List<Driver> list = new List<Driver>();
            string query = "SELECT * FROM DRIVER;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Driver
                    {
                        DriverID = Convert.ToInt32(reader["DriverID"]),
                        DriverName = reader["DriverName"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        LicenseNumber = reader["LicenseNumber"].ToString(),
                        AssignedVehicle = reader["AssignedVehicle"].ToString(),
                        Status = reader["Status"].ToString()
                    });
                }
            }
            return list;
        }

        // 3. READ SINGLE
        public Driver GetDriverById(string conn, int id)
        {
            Driver driver = null;
            string query = "SELECT * FROM DRIVER WHERE DriverID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    driver = new Driver
                    {
                        DriverID = Convert.ToInt32(reader["DriverID"]),
                        DriverName = reader["DriverName"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        LicenseNumber = reader["LicenseNumber"].ToString(),
                        AssignedVehicle = reader["AssignedVehicle"].ToString(),
                        Status = reader["Status"].ToString()
                    };
                }
            }
            return driver;
        }

        // 4. UPDATE
        public void UpdateDriver(string conn, Driver driver)
        {
            string query = @"UPDATE DRIVER 
                             SET DriverName = @name, 
                                 Phone = @phone, 
                                 LicenseNumber = @license, 
                                 AssignedVehicle = @vehicle,
                                 Status = @status
                             WHERE DriverID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", driver.DriverID);
                cmd.Parameters.AddWithValue("@name", driver.DriverName);
                cmd.Parameters.AddWithValue("@phone", driver.Phone);
                cmd.Parameters.AddWithValue("@license", driver.LicenseNumber);
                cmd.Parameters.AddWithValue("@vehicle", driver.AssignedVehicle);
                cmd.Parameters.AddWithValue("@status", driver.Status);

                cmd.ExecuteNonQuery();
            }
        }

        // 5. DELETE
        public void DeleteDriver(string conn, int id)
        {
            string query = "DELETE FROM DRIVER WHERE DriverID = @id;";
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
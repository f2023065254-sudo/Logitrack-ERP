using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class Route_DAL
    {
        // 1. CREATE
        public void AddRoute(string conn, Route route)
        {
            string query = @"INSERT INTO ROUTE 
                             (StartLocation, Destination, Distance, EstimatedTime, AssignedVehicle)
                             VALUES 
                             (@start, @dest, @distance, @time, @vehicle);";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@start", route.StartLocation);
                cmd.Parameters.AddWithValue("@dest", route.Destination);
                cmd.Parameters.AddWithValue("@distance", route.Distance);
                cmd.Parameters.AddWithValue("@time", route.EstimatedTime);
                cmd.Parameters.AddWithValue("@vehicle", route.AssignedVehicle ?? "Unassigned");

                cmd.ExecuteNonQuery();
            }
        }

        // 2. READ ALL
        public List<Route> GetAllRoutes(string conn)
        {
            List<Route> list = new List<Route>();
            string query = "SELECT * FROM ROUTE;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Route
                    {
                        RouteID = Convert.ToInt32(reader["RouteID"]),
                        StartLocation = reader["StartLocation"].ToString(),
                        Destination = reader["Destination"].ToString(),
                        Distance = Convert.ToDecimal(reader["Distance"]),
                        EstimatedTime = reader["EstimatedTime"].ToString(),
                        AssignedVehicle = reader["AssignedVehicle"].ToString()
                    });
                }
            }
            return list;
        }

        // 3. READ SINGLE
        public Route GetRouteById(string conn, int id)
        {
            Route route = null;
            string query = "SELECT * FROM ROUTE WHERE RouteID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    route = new Route
                    {
                        RouteID = Convert.ToInt32(reader["RouteID"]),
                        StartLocation = reader["StartLocation"].ToString(),
                        Destination = reader["Destination"].ToString(),
                        Distance = Convert.ToDecimal(reader["Distance"]),
                        EstimatedTime = reader["EstimatedTime"].ToString(),
                        AssignedVehicle = reader["AssignedVehicle"].ToString()
                    };
                }
            }
            return route;
        }

        // 4. UPDATE
        public void UpdateRoute(string conn, Route route)
        {
            string query = @"UPDATE ROUTE 
                             SET StartLocation = @start, 
                                 Destination = @dest, 
                                 Distance = @distance, 
                                 EstimatedTime = @time,
                                 AssignedVehicle = @vehicle
                             WHERE RouteID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", route.RouteID);
                cmd.Parameters.AddWithValue("@start", route.StartLocation);
                cmd.Parameters.AddWithValue("@dest", route.Destination);
                cmd.Parameters.AddWithValue("@distance", route.Distance);
                cmd.Parameters.AddWithValue("@time", route.EstimatedTime);
                cmd.Parameters.AddWithValue("@vehicle", route.AssignedVehicle);

                cmd.ExecuteNonQuery();
            }
        }

        // 5. DELETE
        public void DeleteRoute(string conn, int id)
        {
            string query = "DELETE FROM ROUTE WHERE RouteID = @id;";
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
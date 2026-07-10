using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class HRM_DAL
    {
        // 1. Sirf Employees aur Drivers ko fetch karna taake list ban sake
        public List<Attendance> GetStaffForAttendance(string? conn)
        {
            List<Attendance> staffList = new List<Attendance>();
            // Users table se data la rahe hain jo humne pehle banaya tha
            string query = "SELECT UserID, UserName, Role FROM Users WHERE Role IN ('Employee', 'Driver')";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    staffList.Add(new Attendance
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        EmployeeName = reader["UserName"].ToString(),
                        Role = reader["Role"].ToString(),
                        Status = "Present" // Default status Present rakh diya
                    });
                }
            }
            return staffList;
        }

        // 2. Bulk Attendance Save karna (Smart UPSERT Logic)
        public void SaveBulkAttendance(string? conn, List<Attendance> attendances, DateTime date)
        {
            // Nayi SQL Query: Pehle check karegi ke kya aaj ki attendance majood hai?
            // Agar HAAN -> To sirf Status UPDATE karegi.
            // Agar NAHI -> To naya record INSERT karegi.
            string query = @"
        IF EXISTS (SELECT 1 FROM Attendance WHERE UserID = @UserID AND AttendanceDate = @AttendanceDate)
        BEGIN
            UPDATE Attendance 
            SET Status = @Status 
            WHERE UserID = @UserID AND AttendanceDate = @AttendanceDate
        END
        ELSE
        BEGIN
            INSERT INTO Attendance (UserID, EmployeeName, Role, AttendanceDate, Status) 
            VALUES (@UserID, @EmployeeName, @Role, @AttendanceDate, @Status)
        END";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                foreach (var item in attendances)
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", item.UserID);
                        cmd.Parameters.AddWithValue("@EmployeeName", item.EmployeeName);
                        cmd.Parameters.AddWithValue("@Role", item.Role);
                        cmd.Parameters.AddWithValue("@AttendanceDate", date.Date);
                        cmd.Parameters.AddWithValue("@Status", item.Status);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // 3. Aaj ki attendance check karna (Report ke liye)
        // Sab dinon ki attendance fetch karne ke liye naya method
        public List<Attendance> GetAllAttendance(string? conn)
        {
            List<Attendance> list = new List<Attendance>();
            // ORDER BY AttendanceDate DESC se naye records sab se upar aayenge
            string query = "SELECT * FROM Attendance ORDER BY AttendanceDate DESC";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Attendance
                    {
                        AttendanceID = Convert.ToInt32(reader["AttendanceID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        EmployeeName = reader["EmployeeName"].ToString(),
                        Role = reader["Role"].ToString(),
                        AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"]),
                        Status = reader["Status"].ToString()
                    });
                }
            }
            return list;
        }
    }
}
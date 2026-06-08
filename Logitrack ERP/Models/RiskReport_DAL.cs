using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Logitrack_ERP.Models
{
    public class RiskReport_DAL
    {
        // 1. CREATE
        public void AddRiskReport(string conn, RiskReport risk)
        {
            string query = @"INSERT INTO RISKREPORT 
                             (ComplaintID, RiskType, Description, Severity, ReportedDate, Status, ResolvedDate)
                             VALUES 
                             (@complaintID, @type, @desc, @severity, @reportedDate, @status, @resolvedDate);";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@complaintID", risk.ComplaintID);
                cmd.Parameters.AddWithValue("@type", risk.RiskType);
                cmd.Parameters.AddWithValue("@desc", risk.Description);
                cmd.Parameters.AddWithValue("@severity", risk.Severity);
                cmd.Parameters.AddWithValue("@reportedDate", string.IsNullOrEmpty(risk.ReportedDate) ? DateTime.Now.ToString("yyyy-MM-dd") : risk.ReportedDate);
                cmd.Parameters.AddWithValue("@status", risk.Status ?? "Investigating");
                cmd.Parameters.AddWithValue("@resolvedDate", (object)risk.ResolvedDate ?? DBNull.Value);

                cmd.ExecuteNonQuery();
            }
        }

        // 2. READ ALL
        public List<RiskReport> GetAllRiskReports(string conn)
        {
            List<RiskReport> list = new List<RiskReport>();
            string query = "SELECT * FROM RISKREPORT;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new RiskReport
                    {
                        RiskID = Convert.ToInt32(reader["RiskID"]),
                        ComplaintID = Convert.ToInt32(reader["ComplaintID"]),
                        RiskType = reader["RiskType"].ToString(),
                        Description = reader["Description"].ToString(),
                        Severity = reader["Severity"].ToString(),
                        ReportedDate = Convert.ToDateTime(reader["ReportedDate"]).ToString("yyyy-MM-dd"),
                        Status = reader["Status"].ToString(),
                        ResolvedDate = reader["ResolvedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ResolvedDate"]).ToString("yyyy-MM-dd") : ""
                    });
                }
            }
            return list;
        }

        // 3. READ SINGLE
        public RiskReport GetRiskReportById(string conn, int id)
        {
            RiskReport risk = null;
            string query = "SELECT * FROM RISKREPORT WHERE RiskID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    risk = new RiskReport
                    {
                        RiskID = Convert.ToInt32(reader["RiskID"]),
                        ComplaintID = Convert.ToInt32(reader["ComplaintID"]),
                        RiskType = reader["RiskType"].ToString(),
                        Description = reader["Description"].ToString(),
                        Severity = reader["Severity"].ToString(),
                        ReportedDate = Convert.ToDateTime(reader["ReportedDate"]).ToString("yyyy-MM-dd"),
                        Status = reader["Status"].ToString(),
                        ResolvedDate = reader["ResolvedDate"] != DBNull.Value ? Convert.ToDateTime(reader["ResolvedDate"]).ToString("yyyy-MM-dd") : ""
                    };
                }
            }
            return risk;
        }

        // 4. UPDATE
        public void UpdateRiskReport(string conn, RiskReport risk)
        {
            string query = @"UPDATE RISKREPORT 
                             SET ComplaintID = @complaintID, 
                                 RiskType = @type, 
                                 Description = @desc, 
                                 Severity = @severity,
                                 ReportedDate = @reportedDate,
                                 Status = @status,
                                 ResolvedDate = @resolvedDate
                             WHERE RiskID = @id;";

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", risk.RiskID);
                cmd.Parameters.AddWithValue("@complaintID", risk.ComplaintID);
                cmd.Parameters.AddWithValue("@type", risk.RiskType);
                cmd.Parameters.AddWithValue("@desc", risk.Description);
                cmd.Parameters.AddWithValue("@severity", risk.Severity);
                cmd.Parameters.AddWithValue("@reportedDate", risk.ReportedDate);
                cmd.Parameters.AddWithValue("@status", risk.Status);
                cmd.Parameters.AddWithValue("@resolvedDate", string.IsNullOrEmpty(risk.ResolvedDate) ? DBNull.Value : (object)risk.ResolvedDate);

                cmd.ExecuteNonQuery();
            }
        }

        // 5. DELETE
        public void DeleteRiskReport(string conn, int id)
        {
            string query = "DELETE FROM RISKREPORT WHERE RiskID = @id;";
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
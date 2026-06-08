using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Logitrack_ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly string connString;

        public ReportsController(IConfiguration configuration)
        {
            connString = configuration.GetConnectionString("DefaultConnection");
        }

        // ==========================================
        // API 1: LOW STOCK INVENTORY REPORT
        // ==========================================
        [HttpGet("low-stock")]
        public IActionResult GetLowStockReport()
        {
            List<LowStockDTO> report = new List<LowStockDTO>();
            string query = "SELECT ProductID, ProductName, StockQuantity, Unit FROM PRODUCT WHERE StockQuantity < 50;";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    report.Add(new LowStockDTO
                    {
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        ProductName = reader["ProductName"].ToString(),
                        StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                        Unit = reader["Unit"].ToString()
                    });
                }
            }
            return Ok(new { ReportName = "Low Stock Alert Report", TotalItems = report.Count, Data = report });
        }

        // ==========================================
        // API 2: ORDER REVENUE BY STATUS REPORT
        // ==========================================
        [HttpGet("revenue-summary")]
        public IActionResult GetRevenueSummaryReport()
        {
            List<RevenueSummaryDTO> report = new List<RevenueSummaryDTO>();
            string query = "SELECT Status, COUNT(OrderID) as TotalOrders, SUM(TotalAmount) as TotalRevenue FROM Orders GROUP BY Status;";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    report.Add(new RevenueSummaryDTO
                    {
                        OrderStatus = reader["Status"].ToString(),
                        TotalOrders = Convert.ToInt32(reader["TotalOrders"]),
                        TotalRevenue = reader["TotalRevenue"] != DBNull.Value ? Convert.ToDecimal(reader["TotalRevenue"]) : 0
                    });
                }
            }
            return Ok(new { ReportName = "Revenue Summary by Order Status", Data = report });
        }

        // ==========================================
        // API 3: FLEET STATUS REPORT
        // ==========================================
        [HttpGet("fleet-status")]
        public IActionResult GetFleetStatusReport()
        {
            List<FleetStatusDTO> report = new List<FleetStatusDTO>();
            string query = "SELECT Status, COUNT(VehicleID) as VehicleCount FROM VEHICLE GROUP BY Status;";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    report.Add(new FleetStatusDTO
                    {
                        VehicleStatus = reader["Status"].ToString(),
                        TotalVehicles = Convert.ToInt32(reader["VehicleCount"])
                    });
                }
            }
            return Ok(new { ReportName = "Fleet Status Distribution Report", Data = report });
        }

        // ==========================================
        // API 4: EMPLOYEE HEADCOUNT BY DEPARTMENT
        // ==========================================
        [HttpGet("employee-headcount")]
        public IActionResult GetEmployeeHeadcountReport()
        {
            List<DepartmentHeadcountDTO> report = new List<DepartmentHeadcountDTO>();
            string query = "SELECT Department, COUNT(EmployeeID) as EmployeeCount FROM EMPLOYEE GROUP BY Department;";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    report.Add(new DepartmentHeadcountDTO
                    {
                        DepartmentName = reader["Department"].ToString(),
                        TotalEmployees = Convert.ToInt32(reader["EmployeeCount"])
                    });
                }
            }
            return Ok(new { ReportName = "Department Headcount Report", Data = report });
        }

        // ==========================================
        // API 5: CRITICAL RISKS REPORT
        // ==========================================
        [HttpGet("critical-risks")]
        public IActionResult GetCriticalRisksReport()
        {
            List<CriticalRiskDTO> report = new List<CriticalRiskDTO>();
            // Adjust table name/columns if your Risk table differs slightly
            string query = "SELECT RiskType, Description, ReportedDate, Status FROM RISK WHERE Severity = 'Critical' AND Status != 'Resolved';";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    report.Add(new CriticalRiskDTO
                    {
                        RiskType = reader["RiskType"].ToString(),
                        Description = reader["Description"].ToString(),
                        ReportedDate = Convert.ToDateTime(reader["ReportedDate"]).ToString("yyyy-MM-dd"),
                        MitigationStatus = reader["Status"].ToString()
                    });
                }
            }
            return Ok(new { ReportName = "Active Critical Risks Report", TotalRisks = report.Count, Data = report });
        }
    }

    // ==========================================
    // DATA TRANSFER OBJECTS (DTOs) FOR THE APIs
    // ==========================================
    public class LowStockDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int StockQuantity { get; set; }
        public string Unit { get; set; }
    }

    public class RevenueSummaryDTO
    {
        public string OrderStatus { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class FleetStatusDTO
    {
        public string VehicleStatus { get; set; }
        public int TotalVehicles { get; set; }
    }

    public class DepartmentHeadcountDTO
    {
        public string DepartmentName { get; set; }
        public int TotalEmployees { get; set; }
    }

    public class CriticalRiskDTO
    {
        public string RiskType { get; set; }
        public string Description { get; set; }
        public string ReportedDate { get; set; }
        public string MitigationStatus { get; set; }
    }
}
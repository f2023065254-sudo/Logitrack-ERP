using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Logitrack_ERP.API.Models;

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

        
        // API 1: LOW STOCK INVENTORY REPORT
        
        [HttpGet("low-stock")]
        public IActionResult GetLowStockReport()
        {
            List<LowStock> report = new List<LowStock>();
            string query = "SELECT ProductID, ProductName, StockQuantity, Unit FROM PRODUCT WHERE StockQuantity < 50;";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    report.Add(new LowStock
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

        
        // API 2: ORDER REVENUE BY STATUS REPORT
        
        [HttpGet("revenue-summary")]
        public IActionResult GetRevenueSummaryReport()
        {
            List<RevenueSummary> report = new List<RevenueSummary>();
            string query = "SELECT Status, COUNT(OrderID) as TotalOrders, SUM(TotalAmount) as TotalRevenue FROM Orders GROUP BY Status;";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    report.Add(new RevenueSummary
                    {
                        OrderStatus = reader["Status"].ToString(),
                        TotalOrders = Convert.ToInt32(reader["TotalOrders"]),
                        TotalRevenue = reader["TotalRevenue"] != DBNull.Value ? Convert.ToDecimal(reader["TotalRevenue"]) : 0
                    });
                }
            }
            return Ok(new { ReportName = "Revenue Summary by Order Status", Data = report });
        }

        
        // API 3: FLEET STATUS REPORT
        
        [HttpGet("fleet-status")]
        public IActionResult GetFleetStatusReport()
        {
            List<FleetStatus> report = new List<FleetStatus>();
            string query = "SELECT Status, COUNT(VehicleID) as VehicleCount FROM VEHICLE GROUP BY Status;";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    report.Add(new FleetStatus
                    {
                        VehicleStatus = reader["Status"].ToString(),
                        TotalVehicles = Convert.ToInt32(reader["VehicleCount"])
                    });
                }
            }
            return Ok(new { ReportName = "Fleet Status Distribution Report", Data = report });
        }

       
        // API 4: EMPLOYEE HEADCOUNT BY DEPARTMENT
        
        [HttpGet("employee-headcount")]
        public IActionResult GetEmployeeHeadcountReport()
        {
            List<DepartmentHead> report = new List<DepartmentHead>();
            string query = "SELECT Department, COUNT(EmployeeID) as EmployeeCount FROM EMPLOYEE GROUP BY Department;";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    report.Add(new DepartmentHead
                    {
                        DepartmentName = reader["Department"].ToString(),
                        TotalEmployees = Convert.ToInt32(reader["EmployeeCount"])
                    });
                }
            }
            return Ok(new { ReportName = "Department Headcount Report", Data = report });
        }

        
        // API 5: CRITICAL RISKS REPORT
   
        [HttpGet("critical-risks")]
        public IActionResult GetCriticalRisksReport()
        {
            List<CriticalRisk> report = new List<CriticalRisk>();
            
            string query = "SELECT RiskType, Description, ReportedDate, Status FROM RISK WHERE Severity = 'Critical' AND Status != 'Resolved';";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    report.Add(new CriticalRisk
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

    
  
   

 

   
}
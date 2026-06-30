namespace Logitrack_ERP.Models
{
    // (Keep your existing ReportApiResponse<T> and LowStockReportItem here)

    public class RevenueSummaryItem
    {
        public string OrderStatus { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class FleetStatusItem
    {
        public string VehicleStatus { get; set; }
        public int TotalVehicles { get; set; }
    }

    public class DepartmentHeadcountItem
    {
        public string DepartmentName { get; set; }
        public int TotalEmployees { get; set; }
    }

    public class CriticalRiskItem
    {
        public string RiskType { get; set; }
        public string Description { get; set; }
        public string ReportedDate { get; set; }
        public string MitigationStatus { get; set; }
    }

    public class ReportApiResponse<T>
    {
        public string ReportName { get; set; }
        public int TotalItems { get; set; }
        public List<T> Data { get; set; }
    }

    // 2. This matches the Low Stock data inside the 'Data' array
    public class LowStockReportItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int StockQuantity { get; set; }
        public string Unit { get; set; }
    }
}
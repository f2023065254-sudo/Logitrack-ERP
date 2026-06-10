namespace Logitrack_ERP.API.Models
{
    public class RevenueSummary
    {
        public string OrderStatus { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}

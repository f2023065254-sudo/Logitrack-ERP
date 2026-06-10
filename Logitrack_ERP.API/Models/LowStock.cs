namespace Logitrack_ERP.API.Models
{
    public class LowStock
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int StockQuantity { get; set; }
        public string Unit { get; set; }
    }

}
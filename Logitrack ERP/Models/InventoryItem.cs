namespace Logitrack_ERP.Models
{
    public class InventoryItem
    {
        public int InventoryID { get; set; } // Matches DB Primary Key
        public int WarehouseID { get; set; } // Matches DB Foreign Key
        public int ProductID { get; set; }   // Replaced SKU with ProductID
        public string Category { get; set; }
        public int StockLevel { get; set; }
        public string BatchNo { get; set; }
        public string ProductName { get; set; }
        public string LocationStorage { get; set; }
    }
}
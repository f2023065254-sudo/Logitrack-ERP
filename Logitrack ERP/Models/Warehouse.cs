namespace Logitrack_ERP.Models
{
    public class Warehouse
    {
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }

        public string Location { get; set; }
        public double Capacity { get; set; }

        public double CurrentStock { get; set; }


    }
}

namespace Logitrack_ERP.Models
{
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleType { get; set; }
        public decimal Capacity { get; set; }
        public string Status { get; set; }
        public string CurrentDriver { get; set; }
        public string NextMaintenanceDate { get; set; }
    }
}

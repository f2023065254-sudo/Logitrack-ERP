namespace Logitrack_ERP.Models
{
    public class Route
    {
        public int RouteID { get; set; }
        public string StartLocation { get; set; }
        public string Destination { get; set; }
        public decimal Distance { get; set; }
        public string EstimatedTime { get; set; }
        public string AssignedVehicle { get; set; }
    }
}

namespace Logitrack_ERP.Models
{
    public class Complaint
    {
        public int ComplaintID { get; set; }
        public int CustomerID { get; set; }
        public int OrderID { get; set; }
        public string ComplaintText { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }

        // Joined Display Fields
        public string CustomerName { get; set; }
    }
}
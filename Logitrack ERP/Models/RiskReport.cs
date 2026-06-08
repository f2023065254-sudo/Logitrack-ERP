namespace Logitrack_ERP.Models
{
    public class RiskReport
    {
        public int RiskID { get; set; }
        public int ComplaintID { get; set; }
        public string RiskType { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        public string ReportedDate { get; set; }
        public string Status { get; set; }
        public string ResolvedDate { get; set; }
    }
}
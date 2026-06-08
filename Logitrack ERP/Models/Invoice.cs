namespace Logitrack_ERP.Models
{
    public class Invoice
    {
        public int InvoiceID { get; set; }
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public string InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string DueDate { get; set; }
        public string Status { get; set; }
    }
}

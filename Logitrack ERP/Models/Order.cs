namespace Logitrack_ERP.Models
{

   public enum OrderStatus
    {
        Pending,
        In_Transit,
        Delivered
    }
    public class Order
    {
        public int OrderID { get; set; }

        public int CustomerID { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; }
        public double TotalAmount { get; set; }

        public string DeliveryAddress { get; set; }

        public string CustomerName { get; set; }
        

        public string ContactNo { get; set; }

        
        
    }
}

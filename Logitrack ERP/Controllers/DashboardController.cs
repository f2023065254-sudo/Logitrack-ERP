using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Logitrack_ERP.Models;
using Logitrack_ERP.Filters;
using System.Collections.Generic;
using System.Linq;

namespace Logitrack_ERP.Controllers
{
    public class DashboardController : Controller
    {
        private readonly string conn;
        private Order_DAL order_dal = new Order_DAL();
        private Invoice_DAL invoice_dal = new Invoice_DAL();
        private Driver_DAL driver_dal = new Driver_DAL();
        private Customer_DAL cust_dal = new Customer_DAL();
        private Vehicle_DAL vehicle_dal = new Vehicle_DAL();
        private Complaint_DAL comp_dal = new Complaint_DAL(); // <-- Naya: Complaints ke liye
        private Inventory_DAL inv_dal = new Inventory_DAL(); // <-- Naya: Warehouse Stock ke liye

        public DashboardController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        // ==========================================
        // 1. OWNER / ADMIN DASHBOARD
        // ==========================================
        [RoleAccess("Owner")]
        public IActionResult OwnerDashboard()
        {
            var allInvoices = invoice_dal.GetAllInvoices(conn) ?? new List<Invoice>();
            var allDrivers = driver_dal.GetAllDrivers(conn) ?? new List<Driver>();
            var allCustomers = cust_dal.GetAllCustomers(conn) ?? new List<Customer>();

            decimal totalRevenue = allInvoices.Sum(i => i.TotalAmount);
            ViewBag.TotalRevenue = "PKR " + totalRevenue.ToString("N0");
            ViewBag.ActiveDrivers = allDrivers.Count.ToString();
            ViewBag.TotalCustomers = allCustomers.Count.ToString();
            return View();
        }

        // ==========================================
        // 2. CUSTOMER DASHBOARD
        // ==========================================
        [RoleAccess("Customer")]
        public IActionResult CustomerDashboard()
        {
            var allOrders = order_dal.getallorders(conn) ?? new List<Order>();
            var allInvoices = invoice_dal.GetAllInvoices(conn) ?? new List<Invoice>();

            int myActiveOrders = allOrders.Count(o => o.Status.ToString() != "Delivered");
            decimal myTotalSpent = allInvoices.Sum(i => i.TotalAmount);

            ViewBag.MyOrders = $"{myActiveOrders} Active Orders";
            ViewBag.TotalSpent = "PKR " + myTotalSpent.ToString("N0");
            return View();
        }

        // ==========================================
        // 3. DRIVER DASHBOARD
        // ==========================================
        [RoleAccess("Driver")]
        public IActionResult DriverDashboard()
        {
            var allOrders = order_dal.getallorders(conn) ?? new List<Order>();
            int pendingDeliveries = allOrders.Count(o => o.Status.ToString() == "In_Transit" || o.Status.ToString() == "Pending");

            ViewBag.TodayRoute = "Lahore to Islamabad (GT Road)";
            ViewBag.VehicleNo = "Assigned Vehicle Pending";
            ViewBag.SalaryAmount = "PKR 45,000";
            ViewBag.SalaryStatus = "Active";
            ViewBag.PendingDeliveries = pendingDeliveries.ToString();
            return View();
        }

        // ==========================================
        // 4. MANAGER DASHBOARD (Yahan Updates Kiye Hain)
        // ==========================================
        [RoleAccess("Manager")]
        public IActionResult ManagerDashboard()
        {
            // Database se tables fetch karein
            var allOrders = order_dal.getallorders(conn) ?? new List<Order>();
            var allVehicles = vehicle_dal.GetAllVehicles(conn) ?? new List<Vehicle>();
            var allComplaints = comp_dal.GetAllComplaints(conn) ?? new List<Complaint>();
            var allInventory = inv_dal.GetAllInventory(conn) ?? new List<InventoryItem>();

            // 1. Orders
            int pendingApprovals = allOrders.Count(o => o.Status.ToString() == "Pending");
            int activeShipments = allOrders.Count(o => o.Status.ToString() == "In_Transit");

            // 2. Fleet & Vehicles
            int activeVehicles = allVehicles.Count(v => v.Status == "On Route" || v.Status == "Available");

            // 3. Risk & Complaints
            // Assuming "Pending" ya "Open" status ho complaints ka
            int pendingComplaints = allComplaints.Count;

            // 4. Warehouse Alerts (Items with low stock)
            int lowStockItems = allInventory.Count(i => i.StockLevel < 20); // 20 se kam wale

            // ViewBags for Manager
            ViewBag.PendingApprovals = $"{pendingApprovals} Orders Waiting";
            ViewBag.ActiveTasks = $"{activeShipments} Active Shipments";
            ViewBag.ActiveVehicles = $"{activeVehicles} Vehicles Ready/On-Route";
            ViewBag.PendingComplaints = $"{pendingComplaints} Unresolved Issues";
            ViewBag.LowStockAlerts = $"{lowStockItems} Items Running Low";

            return View();
        }

        // ==========================================
        // 5. EMPLOYEE / STAFF DASHBOARD
        // ==========================================
        [RoleAccess("Employee")]
        public IActionResult EmployeeDashboard()
        {
            var allOrders = order_dal.getallorders(conn) ?? new List<Order>();
            ViewBag.MyTasks = $"{allOrders.Count} Total System Orders";
            ViewBag.AttendanceStatus = "Checked In at " + System.DateTime.Now.ToString("hh:mm tt");
            return View();
        }
    }
}
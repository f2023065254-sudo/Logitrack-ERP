using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq; // Added for LINQ calculations (Sum, Count, Where)
using System;

namespace Logitrack_ERP.Controllers
{
    public class HomeController : BaseController
    {
        private readonly string conn;
        private Order_DAL order_dal = new Order_DAL();
        private Inventory_DAL inventory_dal = new Inventory_DAL();
        private Vehicle_DAL vehicle_dal = new Vehicle_DAL();
        private Invoice_DAL invoice_dal = new Invoice_DAL();

        public HomeController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            // 1. Fetch data from all relevant tables
            var allOrders = order_dal.getallorders(conn) ?? new List<Order>();
            var allInventory = inventory_dal.GetAllInventory(conn) ?? new List<InventoryItem>();
            var allVehicles = vehicle_dal.GetAllVehicles(conn) ?? new List<Vehicle>();
            var allInvoices = invoice_dal.GetAllInvoices(conn) ?? new List<Invoice>();

            // 2. Calculate Top Row Metrics
            ViewBag.TotalOrders = allOrders.Count;
            ViewBag.TotalStock = allInventory.Sum(i => i.StockLevel);
            // Count active vehicles (Available or On Route)
            ViewBag.ActiveVehicles = allVehicles.Count(v => v.Status == "Available" || v.Status == "On Route");
            // Calculate total revenue from paid invoices
            ViewBag.TotalRevenue = allInvoices.Where(i => i.Status == "Paid").Sum(i => i.TotalAmount);

            // 3. Calculate Order Status Distribution (Pie Chart labels)
            int totalOrderCount = allOrders.Count > 0 ? allOrders.Count : 1; // Prevent division by zero
            ViewBag.DeliveredPct = (allOrders.Count(o => o.Status == OrderStatus.Delivered) * 100) / totalOrderCount;
            ViewBag.CancelledPct = (allOrders.Count(o => o.Status == OrderStatus.Cancelled) * 100) / totalOrderCount;
            ViewBag.PendingPct = (allOrders.Count(o => o.Status == OrderStatus.Pending) * 100) / totalOrderCount;
            ViewBag.InTransitPct = (allOrders.Count(o => o.Status == OrderStatus.In_Transit) * 100) / totalOrderCount;

            // 4. Fetch the 4 most recent orders
            ViewBag.RecentOrders = allOrders.OrderByDescending(o => o.OrderID).Take(4).ToList();

            // 5. Generate Dynamic Alerts
            List<string> alerts = new List<string>();

            // Generate Low Stock Alerts (< 20 items)
            var lowStockItems = allInventory.Where(i => i.StockLevel < 20).ToList();
            foreach (var item in lowStockItems)
            {
                alerts.Add($"Low stock alert: {item.ProductName} (Quantity: {item.StockLevel})|warning");
            }

            // Generate Pending Payment Alerts
            var unpaidInvoices = allInvoices.Where(i => i.Status == "Unpaid").ToList();
            foreach (var inv in unpaidInvoices)
            {
                alerts.Add($"Payment pending for Invoice #INV-{inv.InvoiceID:D4} (PKR {inv.TotalAmount:N0})|danger");
            }

            // Show maximum of 5 recent alerts
            ViewBag.Alerts = alerts.Take(5).ToList();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
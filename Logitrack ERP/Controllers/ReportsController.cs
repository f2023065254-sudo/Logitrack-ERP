using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Logitrack_ERP.Models;
using Logitrack_ERP.Filters;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Logitrack_ERP.Controllers
{
    [RoleAccess("Owner", "Manager")] // Sirf authorized roles ke liye
    public class ReportsController : BaseController
    {
        private readonly string conn;
        private Order_DAL order_dal = new Order_DAL();
        private Invoice_DAL invoice_dal = new Invoice_DAL();

        public ReportsController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            var allOrders = order_dal.getallorders(conn) ?? new List<Order>();
            var allInvoices = invoice_dal.GetAllInvoices(conn) ?? new List<Invoice>();

            // 1. Top Cards Calculations (Real-Time)
            decimal totalRevenue = allInvoices.Sum(i => (decimal)i.TotalAmount);
            int totalOrders = allOrders.Count;

            // On-Time Delivery Logic (Delivered orders divided by Total orders)
            int deliveredOrders = allOrders.Count(o => o.Status.ToString() == "Delivered");
            double onTimePercentage = totalOrders > 0 ? ((double)deliveredOrders / totalOrders) * 100 : 0;

            // ViewBags mein data assign karein
            ViewBag.TotalRevenue = totalRevenue >= 1000000
                ? (totalRevenue / 1000000).ToString("0.##") + "M"
                : totalRevenue.ToString("N0");
            ViewBag.TotalOrders = totalOrders.ToString("N0");
            ViewBag.OnTimeDelivery = Math.Round(onTimePercentage, 1) + "%";
            ViewBag.EmployeeProductivity = "90%"; // Isay filhal static rakha hai, HR module se link kar sakte hain

            // 2. Chart.js ke liye Monthly Data (Current Year)
            int currentYear = DateTime.Now.Year;
            var monthlyOrders = allOrders
                .Where(o => o.OrderDate.Year == currentYear)
                .GroupBy(o => o.OrderDate.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToList();

            // 12 mahinon ka array banayein (Jan to Dec)
            int[] monthlyDataArray = new int[12];
            foreach (var item in monthlyOrders)
            {
                monthlyDataArray[item.Month - 1] = item.Count;
            }

            // Array ko string mein convert kar ke view mein bhejein (e.g., "10,25,0,50...")
            ViewBag.MonthlyChartData = string.Join(",", monthlyDataArray);

            return View();
        }

        // ==========================================
        // DOWNLOAD RAW DATA AS CSV
        // ==========================================
        [HttpGet]
        public IActionResult DownloadReportCsv()
        {
            var allOrders = order_dal.getallorders(conn) ?? new List<Order>();
            var allInvoices = invoice_dal.GetAllInvoices(conn) ?? new List<Invoice>();

            var builder = new System.Text.StringBuilder();
            builder.AppendLine("Metric,Value");
            builder.AppendLine($"Total Orders,{allOrders.Count}");
            builder.AppendLine($"Total Revenue (PKR),{allInvoices.Sum(i => i.TotalAmount)}");
            // 1. Calculation bahar nikal li (No syntax errors here)
            int deliveredCount = allOrders.Count(o => o.Status.ToString() == "Delivered");
            int pendingCount = allOrders.Count(o => o.Status.ToString() != "Delivered");

            // 2. Ab variables ko cleanly string mein add kar diya
            builder.AppendLine($"Delivered Orders,{deliveredCount}");
            builder.AppendLine($"Pending/In-Transit,{pendingCount}");

            return File(System.Text.Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "Logitrack_Analytics_Summary.csv");
        }
    }
}
             
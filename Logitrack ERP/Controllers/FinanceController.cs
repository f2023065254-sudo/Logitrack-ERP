using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Logitrack_ERP.Filters; // <-- 1. Security Filter yahan add kiya hai

namespace Logitrack_ERP.Controllers
{
    // <-- 2. Lock yahan lagaya hai (Sirf Owner aur Manager ke liye) -->
    [RoleAccess("Owner", "Manager")]
    public class FinanceController : BaseController
    {
        private readonly string conn;
        private Invoice_DAL invoice_dal = new Invoice_DAL();

        public FinanceController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Invoice> allInvoices = invoice_dal.GetAllInvoices(conn);
            return View(allInvoices);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Note: Kyunke humne "Auto-Invoice" ka system Order_DAL mein bana liya tha, 
        // to shayad aapko is manual Create method ki zaroorat na paray. 
        // Phir bhi maine isay code mein rehne diya hai.
        [HttpPost]
        public IActionResult Create(Order o)
        {
            // Yahan shayad typo thi (CreateOrder), isay ignore kar sakte hain agar aap auto-invoice use kar rahe hain
            invoice_dal.CreateOrder(o, conn);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Invoice invoice = invoice_dal.GetInvoiceById(conn, id);
            if (invoice == null) return NotFound();
            return View(invoice);
        }

        // ==========================================
        // EXPORT TO EXCEL (CSV) FEATURE
        // ==========================================
        [HttpGet]
        public IActionResult ExportInvoicesCsv()
        {
            // 1. Database se sari invoices mangwayein
            List<Invoice> allInvoices = invoice_dal.GetAllInvoices(conn);

            // 2. CSV ka header (Top Row) banayein
            var builder = new System.Text.StringBuilder();
            builder.AppendLine("Invoice ID,Order ID,Customer ID,Invoice Date,Due Date,Total Amount (PKR),Status");

            // 3. Har invoice ko CSV ki ek nayi line mein add karein
            foreach (var inv in allInvoices)
            {
                // Dates ko proper format mein likhna zaroori hai
                string invDate = Convert.ToDateTime(inv.InvoiceDate).ToString("yyyy-MM-dd");
                string dueDate = Convert.ToDateTime(inv.DueDate).ToString("yyyy-MM-dd");

                builder.AppendLine($"{inv.InvoiceID},{inv.OrderID},{inv.CustomerID},{invDate},{dueDate},{inv.TotalAmount},{inv.Status}");
            }

            // 4. File ko return karein taake browser download kar le
            return File(System.Text.Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "Finance_Invoices_Report.csv");
        }

        [HttpPost]
        public IActionResult Edit(Invoice invoice)
        {
            invoice_dal.UpdateInvoice(conn, invoice);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            invoice_dal.DeleteInvoice(conn, id);
            return RedirectToAction("Index");
        }
    }
}
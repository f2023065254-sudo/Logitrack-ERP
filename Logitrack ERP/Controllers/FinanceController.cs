using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Logitrack_ERP.Controllers
{
    public class FinanceController : Controller
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

        [HttpPost]
        public IActionResult Create(Invoice invoice)
        {
            invoice_dal.AddInvoice(conn, invoice);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Invoice invoice = invoice_dal.GetInvoiceById(conn, id);
            if (invoice == null) return NotFound();
            return View(invoice);
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
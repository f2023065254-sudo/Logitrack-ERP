using Microsoft.AspNetCore.Mvc;

namespace Logitrack_ERP.Controllers
{
    public class FinanceController : Controller
    {
        // 1. Invoices Tab (Main Page)
        public IActionResult Index()
        {
            return View();
        }

        // 2. Payments Tab
        public IActionResult Payments()
        {
            return View();
        }

        // 3. Create Invoice Page
        public IActionResult Create()
        {
            return View();
        }
        // Invoice View Details Page
        public IActionResult Details()
        {
            return View();
        }
        // 4. Edit Invoice Page
        public IActionResult Edit()
        {
            return View();
        }
    }
}
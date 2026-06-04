using Microsoft.AspNetCore.Mvc;

namespace Logitrack_ERP.Controllers
{
    public class HRMController : Controller
    {
        // 1. Employees Tab (Main Page)
        public IActionResult Index()
        {
            return View();
        }

        // 2. Payroll Tab
        public IActionResult Payroll()
        {
            return View();
        }

        // 3. Add Employee Page
        public IActionResult Create()
        {
            return View();
        }

        // 4. Edit Employee Page
        public IActionResult Edit()
        {
            return View();
        }
    }
}
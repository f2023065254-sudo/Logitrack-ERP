using Microsoft.AspNetCore.Mvc;

namespace Logitrack_ERP.Controllers
{
    public class FleetController : Controller
    {
        // 1. Vehicles Sub-tab (Main Page)
        public IActionResult Index()
        {
            return View();
        }

        // ➡️ 2. Drivers Sub-tab Page
        public IActionResult Drivers()
        {
            return View();
        }

        // ➡️ 3. Routes Sub-tab Page
        public IActionResult Routes()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateDriver()
        {
            return View();
        }
        public IActionResult CreateRoute()
        {
            return View();
        }

        public IActionResult EditRoute()
        {
            return View();
        }
        public IActionResult EditDriver()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
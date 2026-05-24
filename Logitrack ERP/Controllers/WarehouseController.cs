using Microsoft.AspNetCore.Mvc;

namespace Logitrack_ERP.Controllers
{
    public class WarehouseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateInventory()
        {
            return View();
        }

        public IActionResult EditInventory()
        {
            return View();
        }
        public IActionResult Inventory()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
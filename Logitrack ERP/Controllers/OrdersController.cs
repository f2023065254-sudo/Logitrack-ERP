using Microsoft.AspNetCore.Mvc;

namespace Logitrack_ERP.Controllers
{
    public class OrdersController : Controller
    {
        // Jab user link par click karega, yeh method khulega
        public IActionResult Index()
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
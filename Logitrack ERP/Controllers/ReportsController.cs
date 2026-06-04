using Microsoft.AspNetCore.Mvc;

namespace Logitrack_ERP.Controllers
{
    public class ReportsController : Controller
    {
        // Reports & Analytics Dashboard Main Page
        public IActionResult Index()
        {
            return View();
        }
    }
}
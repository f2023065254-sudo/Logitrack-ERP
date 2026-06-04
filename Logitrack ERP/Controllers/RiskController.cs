using Microsoft.AspNetCore.Mvc;

namespace Logitrack_ERP.Controllers
{
    [Route("Complaints")] // <--- Yeh line poore controller ka URL /Complaints par map kar degi
    public class RiskController : Controller
    {
        [HttpGet("")] // Default route yani /Complaints
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("RiskReports")] // Route banega: /Complaints/RiskReports
        public IActionResult RiskReports()
        {
            return View();
        }

        [HttpGet("Create")] // Route banega: /Complaints/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet("Edit")] // Route banega: /Complaints/Edit
        public IActionResult Edit()
        {
            return View();
        }

        [HttpGet("Details")] // Route banega: /Complaints/Details
        public IActionResult Details()
        {
            return View();
        }
    }
}
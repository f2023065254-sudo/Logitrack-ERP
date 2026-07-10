using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Logitrack_ERP.Controllers
{
    public class ManagerController : BaseController
    {
        private readonly string conn;
        private Manager_DAL manager_dal = new Manager_DAL();

        public ManagerController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Manager> allManagers = manager_dal.GetAllManagers(conn);
            return View(allManagers);
        }

        [HttpGet]
        public IActionResult Create() { return View(); }

        [HttpPost]
        public IActionResult Create(Manager manager)
        {
            manager_dal.AddManager(conn, manager);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Manager manager = manager_dal.GetManagerById(conn, id);
            if (manager == null) return NotFound();
            return View(manager);
        }

        [HttpPost]
        public IActionResult Edit(Manager manager)
        {
            manager_dal.UpdateManager(conn, manager);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            manager_dal.DeleteManager(conn, id);
            return RedirectToAction("Index");
        }
    }
}
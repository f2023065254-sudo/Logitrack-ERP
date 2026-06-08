using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Logitrack_ERP.Controllers
{
    public class HRMController : Controller
    {
        private readonly string conn;
        private Employee_DAL emp_dal = new Employee_DAL();

        public HRMController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Employee> allEmployees = emp_dal.GetAllEmployees(conn);
            return View(allEmployees);
        }

        [HttpGet]
        public IActionResult Payroll()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            emp_dal.AddEmployee(conn, emp);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee emp = emp_dal.GetEmployeeById(conn, id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        [HttpPost]
        public IActionResult Edit(Employee emp)
        {
            emp_dal.UpdateEmployee(conn, emp);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            emp_dal.DeleteEmployee(conn, id);
            return RedirectToAction("Index");
        }
    }
}
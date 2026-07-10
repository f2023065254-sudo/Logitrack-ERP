using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using Logitrack_ERP.Filters;

namespace Logitrack_ERP.Controllers
{
    [RoleAccess("Manager", "Owner")] // Sirf inko access hoga
    public class HRMController : BaseController
    {
        private readonly string conn;
        private Employee_DAL emp_dal = new Employee_DAL();
        private HRM_DAL hrm_dal = new HRM_DAL(); // <-- NAYA: Attendance ke liye DAL add kiya

        public HRMController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        // ===============================================
        //             EMPLOYEE MANAGEMENT
        // ===============================================

        [HttpGet]
        public IActionResult Index()
        {
            List<Employee> allEmployees = emp_dal.GetAllEmployees(conn);
            return View(allEmployees);
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

        // ===============================================
        //             PAYROLL MANAGEMENT
        // ===============================================

        [HttpGet]
        public IActionResult Payroll()
        {
            return View();
        }

        // ===============================================
        //             ATTENDANCE MANAGEMENT
        // ===============================================

        // 1. Daily Attendance Report View
        [HttpGet]
        public IActionResult Attendance()
        {
            // Aaj ki attendance fetch karo
            var allAttendance = hrm_dal.GetAllAttendance(conn);
            return View(allAttendance);
      
        }

        // 2. Mark Attendance Page (GET)
        [HttpGet]
        public IActionResult MarkAttendance()
        {
            // Database se sab Employees aur Drivers lao
            var staffList = hrm_dal.GetStaffForAttendance(conn);

            var viewModel = new AttendanceSubmitViewModel
            {
                AttendanceDate = DateTime.Today,
                AttendanceList = staffList
            };

            return View(viewModel); // Yeh wahi view hai jo pichle step mein banaya tha
        }

        // 3. Save Attendance (POST)
        [HttpPost]
        public IActionResult MarkAttendance(AttendanceSubmitViewModel model)
        {
            if (model.AttendanceList != null && model.AttendanceList.Count > 0)
            {
                hrm_dal.SaveBulkAttendance(conn, model.AttendanceList, model.AttendanceDate);
                TempData["SuccessMessage"] = "Attendance successfully marked for " + model.AttendanceDate.ToString("dd MMM yyyy");

                // Save hone ke baad wapis Attendance report par bhej do
                return RedirectToAction("Attendance");
            }
            return View(model);
        }
    }
}
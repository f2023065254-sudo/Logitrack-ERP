using Logitrack_ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text.Json;
using Logitrack_ERP.Filters; // <-- 1. Security Filter yahan add kiya hai

namespace Logitrack_ERP.Controllers
{
    // <-- 2. Lock yahan lagaya hai (Owner, Manager aur Employee ke liye) -->
    [RoleAccess("Owner", "Manager", "Employee")]
    public class CustomerController : BaseController
    {
        private readonly string conn;
        private Customer_DAL cust_dal = new Customer_DAL();

        public CustomerController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }


        [HttpGet]
        public IActionResult Index()
        {
            List<Customer> allCustomers = cust_dal.GetAllCustomers(conn);
            return View(allCustomers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Customer cust)
        {
            cust_dal.AddCustomer(conn, cust);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Customer cust = cust_dal.GetCustomerById(conn, id);
            if (cust == null) return NotFound();
            return View(cust);
        }

        [HttpPost]
        public IActionResult Edit(Customer cust)
        {
            cust_dal.UpdateCustomer(conn, cust);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            cust_dal.DeleteCustomer(conn, id);
            return RedirectToAction("Index");
        }

    }
}
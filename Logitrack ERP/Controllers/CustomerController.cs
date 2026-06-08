using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Logitrack_ERP.Controllers
{
    public class CustomerController : Controller
    {
        private readonly string conn;
        private Customer_DAL cust_dal = new Customer_DAL();

        public CustomerController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        // --- STANDARD CRUD ROUTES ---
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
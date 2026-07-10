using Logitrack_ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Logitrack_ERP.Filters;
using Microsoft.AspNetCore.Http; // Session read karne ke liye

namespace Logitrack_ERP.Controllers
{
    [RoleAccess("Owner", "Manager", "Driver", "Customer")]
    public class OrdersController : BaseController
    {
        private readonly string conn;
        private Order_DAL order_dal = new Order_DAL();

        public OrdersController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("Role");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            List<Order> allOrders;

            // Filter Logic
            if (userRole == "Customer" && !string.IsNullOrEmpty(userEmail))
            {
                allOrders = order_dal.GetOrdersByCustomerEmail(conn, userEmail);
            }
            else
            {
                allOrders = order_dal.getallorders(conn);
            }

            return View(allOrders);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            using var _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri("https://localhost:7286/");

            HttpResponseMessage response = await _httpClient.GetAsync("api/customers/dropdown");

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var customers = JsonSerializer.Deserialize<List<Customer>>(jsonString, options);
                ViewBag.CustomerList = new SelectList(customers, "CustomerID", "Name");
            }
            else
            {
                ViewBag.CustomerList = new SelectList(new List<Customer>());
            }

            return View();
        }
        [HttpPost]
        public IActionResult Create(Order order)
        {
            var userRole = HttpContext.Session.GetString("Role");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            // Agar Customer hai, to background se data set karo
            if (userRole == "Customer")
            {
                order.Status = OrderStatus.Pending; // Default status
                order.TotalAmount = 0; // Customer amount set nahi karega
                                       // Yahan CustomerID nikalne ke liye logic lagayein
            }

            order_dal.CreateOrder(order, conn);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Order order = order_dal.GetOrder(conn, id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(Order order)
        {
            order_dal.Update(order, conn, order.OrderID);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                order_dal.delete(conn, id);
                return RedirectToAction("Index");
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    TempData["ErrorMessage"] = "Cannot delete this order because it has linked invoices or shipments.";
                    return RedirectToAction("Index");
                }
                TempData["ErrorMessage"] = "Database error: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Logitrack_ERP.Controllers
{
    public class OrdersController : Controller
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
            List<Order> allOrders = order_dal.getallorders(conn);
            return View(allOrders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
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
            order_dal.delete(conn, id);
            return RedirectToAction("Index");
        }
    }
}
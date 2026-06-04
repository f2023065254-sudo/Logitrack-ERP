using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
namespace Logitrack_ERP.Controllers
{
    public class OrdersController : Controller


    {
        private IConfiguration configuration;
        private string? conn;
        Order_DAL dal = new Order_DAL();
        public OrdersController(IConfiguration config)
        {

            this.configuration = config;
            conn = config.GetConnectionString("DefaultConnection");
        }

        // Jab user link par click karega, yeh method khulega
        public IActionResult Index()
        {
            return View(dal.getallorders(conn));
           
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Order o)
        {
            dal.CreateOrder(o, conn);
            return RedirectToAction("Index");
            
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            return View(dal.GetOrder(conn,id));
        }

        [HttpPost]
        public IActionResult Edit(Order O,int id)
        {
            dal.Update(O,conn,id);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            dal.delete(conn, id);
            return RedirectToAction("Index");
        }


        public IActionResult Details(int id)
        {

            return View(dal.showdetails(id, conn));
            
        }

    }
}
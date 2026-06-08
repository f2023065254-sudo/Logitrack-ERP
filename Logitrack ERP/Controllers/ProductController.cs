using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Logitrack_ERP.Controllers
{
    public class ProductController : Controller
    {
        private readonly string conn;
        private Product_DAL product_dal = new Product_DAL();

        public ProductController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Product> products = product_dal.GetAllProducts(conn);
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            product_dal.AddProduct(conn, product);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Product product = product_dal.GetProductById(conn, id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            product_dal.UpdateProduct(conn, product);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            product_dal.DeleteProduct(conn, id);
            return RedirectToAction("Index");
        }
    }
}
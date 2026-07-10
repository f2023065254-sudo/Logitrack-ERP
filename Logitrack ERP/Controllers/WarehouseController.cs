using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
// NAYE USINGS (API CALLS KE LIYE)
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Logitrack_ERP.Filters; // <-- 1. Security Filter yahan add kiya hai

namespace Logitrack_ERP.Controllers
{
    // <-- 2. Lock yahan lagaya hai (Owner, Manager aur Employee ke liye) -->
    [RoleAccess("Owner", "Manager", "Employee")]
    public class WarehouseController : BaseController
    {
        private readonly string conn;
        private Warehouse_DAL war_dal = new Warehouse_DAL();
        private IConfiguration config;
        private Inventory_DAL inv_dal = new Inventory_DAL();

        public WarehouseController(IConfiguration config)
        {
            this.config = config;
            conn = config.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(war_dal.GetAllWarehouses(conn));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Warehouse warehouse)
        {
            war_dal.createWarehouse(conn, warehouse);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Warehouse warehouse = war_dal.GetWarehouseById(conn, id);
            if (warehouse == null)
            {
                return NotFound();
            }
            return View(warehouse);
        }

        [HttpPost]
        public IActionResult Edit(Warehouse warehouse)
        {
            war_dal.updateWarehouse(conn, warehouse);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            war_dal.deletewarehouse(conn, id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ShowWarehouseDetails(int id)
        {
            Warehouse warehouse = war_dal.GetWarehouseById(conn, id);
            return View(warehouse);
        }

        // ===============================================
        //             INVENTORY MANAGEMENT
        // ===============================================

        [HttpGet]
        public IActionResult Inventory()
        {
            List<InventoryItem> allInventory = inv_dal.GetAllInventory(conn);
            return View(allInventory);
        }

        [HttpGet]
        public async Task<IActionResult> CreateInventory()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

            using var _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri("https://localhost:7286/"); // Make sure ye port API ki hai

            // 1. Fetch Warehouses Dropdown Data (Safe method)
            try
            {
                var whResponse = await _httpClient.GetAsync("api/warehouses/dropdown");
                if (whResponse.IsSuccessStatusCode)
                {
                    string whJson = await whResponse.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var warehouses = JsonSerializer.Deserialize<List<Warehouse>>(whJson, options);
                    ViewBag.WarehouseList = new SelectList(warehouses, "WarehouseID", "WarehouseName");
                }
                else
                {
                    ViewBag.WarehouseList = new SelectList(new List<Warehouse>());
                }
            }
            catch
            {
                ViewBag.WarehouseList = new SelectList(new List<Warehouse>());
            }

            // 2. Fetch Products Dropdown Data (Safe method)
            try
            {
                var prodResponse = await _httpClient.GetAsync("api/products/dropdown");
                if (prodResponse.IsSuccessStatusCode)
                {
                    string prodJson = await prodResponse.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var products = JsonSerializer.Deserialize<List<Product>>(prodJson, options);
                    ViewBag.ProductList = new SelectList(products, "ProductID", "ProductName");
                }
                else
                {
                    ViewBag.ProductList = new SelectList(new List<Product>());
                }
            }
            catch
            {
                ViewBag.ProductList = new SelectList(new List<Product>());
            }

            return View();
        }

        [HttpPost]
        public IActionResult CreateInventory(InventoryItem item)
        {
            inv_dal.AddInventoryItem(conn, item);
            return RedirectToAction("Inventory");
        }

        [HttpGet]
        public IActionResult EditInventory(int id)
        {
            InventoryItem item = inv_dal.GetInventoryItemById(conn, id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public IActionResult EditInventory(InventoryItem item)
        {
            inv_dal.UpdateInventoryItem(conn, item);
            return RedirectToAction("Inventory");
        }

        [HttpGet]
        public IActionResult DeleteInventory(int id)
        {
            inv_dal.DeleteInventoryItem(conn, id);
            return RedirectToAction("Inventory");
        }
    }
}
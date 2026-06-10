using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Logitrack_ERP.Controllers
{
    public class WarehouseController : Controller
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

      
        //             INVENTORY MANAGEMENT
        
        [HttpGet]
        public IActionResult Inventory()
        {
            List<InventoryItem> allInventory = inv_dal.GetAllInventory(conn);
            return View(allInventory);
        }

        
        [HttpGet]
        public IActionResult CreateInventory()
        {
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
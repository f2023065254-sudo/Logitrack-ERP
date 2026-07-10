using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Route = Logitrack_ERP.Models.Route;
using Logitrack_ERP.Filters;

namespace Logitrack_ERP.Controllers
{
    [RoleAccess("Owner", "Manager", "Driver")]
    public class FleetController : BaseController
    {
        private readonly string conn;
        private Vehicle_DAL vehicle_dal = new Vehicle_DAL();
        private Driver_DAL driver_dal = new Driver_DAL();
        private Route_DAL route_dal = new Route_DAL();
        private Warehouse_DAL warehouse_dal = new Warehouse_DAL(); // Warehouse DAL add kar diya

        // Constructor hamesha variables ke foran baad aur methods se pehle aata hai
        public FleetController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        // ===============================================
        //             VEHICLE MANAGEMENT (Index)
        // ===============================================

        [HttpGet]
        public IActionResult Index()
        {
            List<Vehicle> allVehicles = vehicle_dal.GetAllVehicles(conn) ?? new List<Vehicle>();

            // Map ke liye Routes aur Warehouses ViewBag mein bhej rahe hain
            ViewBag.ActiveRoutes = route_dal.GetAllRoutes(conn) ?? new List<Route>();
            ViewBag.AllWarehouses = warehouse_dal.GetAllWarehouses(conn) ?? new List<Warehouse>();

            return View(allVehicles);
        }

        [HttpGet]
        public IActionResult Create() { return View(); }

        [HttpPost]
        public IActionResult Create(Vehicle vehicle)
        {
            vehicle_dal.AddVehicle(conn, vehicle);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Vehicle vehicle = vehicle_dal.GetVehicleById(conn, id);
            if (vehicle == null) return NotFound();
            return View(vehicle);
        }

        [HttpPost]
        public IActionResult Edit(Vehicle vehicle)
        {
            vehicle_dal.UpdateVehicle(conn, vehicle);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            vehicle_dal.DeleteVehicle(conn, id);
            return RedirectToAction("Index");
        }

        // ===============================================
        //             DRIVER MANAGEMENT
        // ===============================================

        [HttpGet]
        public IActionResult Drivers()
        {
            List<Driver> allDrivers = driver_dal.GetAllDrivers(conn);
            return View(allDrivers);
        }

        [HttpGet]
        public IActionResult CreateDriver()
        {
            List<Vehicle> vehicles = vehicle_dal.GetAllVehicles(conn);
            ViewBag.VehicleList = new SelectList(vehicles, "VehicleNumber", "VehicleNumber");
            return View();
        }

        [HttpPost]
        public IActionResult CreateDriver(Driver driver)
        {
            driver_dal.AddDriver(conn, driver);
            return RedirectToAction("Drivers");
        }

        [HttpPost]
        public IActionResult EditDriver(Driver driver)
        {
            driver_dal.UpdateDriver(conn, driver);
            return RedirectToAction("Drivers");
        }

        [HttpGet]
        public IActionResult DeleteDriver(int id)
        {
            driver_dal.DeleteDriver(conn, id);
            return RedirectToAction("Drivers");
        }

        // ===============================================
        //             ROUTE MANAGEMENT
        // ===============================================

        [HttpGet]
        public IActionResult Routes()
        {
            List<Route> allRoutes = route_dal.GetAllRoutes(conn) ?? new List<Route>();

            // Yahan hum Vehicles ko ViewBag mein bhej rahe hain taake Map par Trucks show ho sakein
            ViewBag.AllVehicles = vehicle_dal.GetAllVehicles(conn) ?? new List<Vehicle>();

            return View(allRoutes);
        }

        [HttpGet]
        public IActionResult CreateRoute() { return View(); }

        [HttpPost]
        public IActionResult CreateRoute(Route route)
        {
            route_dal.AddRoute(conn, route);
            return RedirectToAction("Routes");
        }

        [HttpGet]
        public IActionResult EditRoute(int id)
        {
            Route route = route_dal.GetRouteById(conn, id);
            if (route == null) return NotFound();
            return View(route);
        }

        [HttpPost]
        public IActionResult EditRoute(Route route)
        {
            route_dal.UpdateRoute(conn, route);
            return RedirectToAction("Routes");
        }

        [HttpGet]
        public IActionResult DeleteRoute(int id)
        {
            route_dal.DeleteRoute(conn, id);
            return RedirectToAction("Routes");
        }
    }
}
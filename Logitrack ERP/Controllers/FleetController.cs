using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Route = Logitrack_ERP.Models.Route;

namespace Logitrack_ERP.Controllers
{
    public class FleetController : Controller
    {
        private readonly string conn;
        private Vehicle_DAL vehicle_dal = new Vehicle_DAL();
        private Driver_DAL driver_dal = new Driver_DAL(); // Added Driver DAL
        private Route_DAL route_dal = new Route_DAL();
        public FleetController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        // ==========================================
        //             VEHICLE MANAGEMENT
        // ==========================================
        [HttpGet]
        public IActionResult Index()
        {
            List<Vehicle> allVehicles = vehicle_dal.GetAllVehicles(conn);
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

        // ==========================================
        //             DRIVER MANAGEMENT
        // ==========================================
        [HttpGet]
        public IActionResult Drivers()
        {
            List<Driver> allDrivers = driver_dal.GetAllDrivers(conn);
            return View(allDrivers);
        }

        [HttpGet]
        public IActionResult CreateDriver() { return View(); }

        [HttpPost]
        public IActionResult CreateDriver(Driver driver)
        {
            driver_dal.AddDriver(conn, driver);
            return RedirectToAction("Drivers"); // Redirects to Drivers tab
        }

        [HttpGet]
        public IActionResult EditDriver(int id)
        {
            Driver driver = driver_dal.GetDriverById(conn, id);
            if (driver == null) return NotFound();
            return View(driver);
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

        // ==========================================
        //             ROUTE MANAGEMENT
        // ==========================================
        [HttpGet]
        public IActionResult Routes()
        {
            List<Route> allRoutes = route_dal.GetAllRoutes(conn);
            return View(allRoutes);
        }

        [HttpGet]
        public IActionResult CreateRoute() { return View(); }

        [HttpPost]
        public IActionResult CreateRoute(Route route)
        {
            route_dal.AddRoute(conn, route);
            return RedirectToAction("Routes"); // Redirects to the Routes dashboard
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
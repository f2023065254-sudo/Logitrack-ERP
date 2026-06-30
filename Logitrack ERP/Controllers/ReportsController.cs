using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Logitrack_ERP.Models;
using System;

namespace Logitrack_ERP.Controllers
{
    public class ReportsController : Controller
    {
        private readonly string _conn;
        private readonly HttpClient _httpClient;

        // 1. Properly injecting IConfiguration
        public ReportsController(IConfiguration config)
        {
            // 2. Safely grabbing the connection string
            _conn = config.GetConnectionString("DefaultConnection");

            // 3. Pointing exactly to your API's port (7286 from your screenshot!)
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7286/");
        }


        public IActionResult Index()
        {
            return View();
        }


        // ==========================================
        // 1. LOW STOCK REPORT
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> LowStock()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/reports/low-stock");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var reportData = JsonSerializer.Deserialize<ReportApiResponse<LowStockReportItem>>(jsonString, options);
                return View(reportData);
            }
            ViewBag.ErrorMessage = "Failed to connect to the API. Is the Logitrack_API project running?";
            return View(null);
        }

        // ==========================================
        // 2. REVENUE SUMMARY
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> RevenueSummary()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/reports/revenue-summary");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var reportData = JsonSerializer.Deserialize<ReportApiResponse<RevenueSummaryItem>>(jsonString, options);
                return View(reportData);
            }
            ViewBag.ErrorMessage = "Failed to connect to the API.";
            return View(null);
        }

        // ==========================================
        // 3. FLEET STATUS
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> FleetStatus()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/reports/fleet-status");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var reportData = JsonSerializer.Deserialize<ReportApiResponse<FleetStatusItem>>(jsonString, options);
                return View(reportData);
            }
            ViewBag.ErrorMessage = "Failed to connect to the API.";
            return View(null);
        }

        // ==========================================
        // 4. EMPLOYEE HEADCOUNT
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> EmployeeHeadcount()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/reports/employee-headcount");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var reportData = JsonSerializer.Deserialize<ReportApiResponse<DepartmentHeadcountItem>>(jsonString, options);
                return View(reportData);
            }
            ViewBag.ErrorMessage = "Failed to connect to the API.";
            return View(null);
        }

        // ==========================================
        // 5. CRITICAL RISKS
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> CriticalRisks()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/reports/critical-risks");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var reportData = JsonSerializer.Deserialize<ReportApiResponse<CriticalRiskItem>>(jsonString, options);
                return View(reportData);
            }
            ViewBag.ErrorMessage = "Failed to connect to the API.";
            return View(null);
        }
    }
}
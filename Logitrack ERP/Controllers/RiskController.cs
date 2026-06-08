using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Logitrack_ERP.Controllers
{
    public class RiskController : Controller
    {
        private readonly string conn;
        private Complaint_DAL comp_dal = new Complaint_DAL();
        private RiskReport_DAL risk_dal = new RiskReport_DAL();

        public RiskController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        // --- READ ALL COMPLAINTS-- -
        [HttpGet]
        public IActionResult Index()
        {
            List<Complaint> allComplaints = comp_dal.GetAllComplaints(conn);
            return View(allComplaints);
        }

        // --- LOG NEW COMPLAINT ---
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Complaint comp)
        {
            comp_dal.AddComplaint(conn, comp);
            return RedirectToAction("Index");
        }

        // --- EDIT COMPLAINT ---
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Complaint comp = comp_dal.GetComplaintById(conn, id);
            if (comp == null) return NotFound();
            return View(comp);
        }

        [HttpPost]
        public IActionResult Edit(Complaint comp)
        {
            comp_dal.UpdateComplaint(conn, comp);
            return RedirectToAction("Index");
        }

        // --- COMPLAINT DETAILS ---
        [HttpGet]
        public IActionResult Details(int id)
        {
            Complaint comp = comp_dal.GetComplaintById(conn, id);
            if (comp == null) return NotFound();
            return View(comp);
        }

        // --- DELETE COMPLAINT ---
        [HttpGet]
        public IActionResult Delete(int id)
        {
            comp_dal.DeleteComplaint(conn, id);
            return RedirectToAction("Index");
        }


        // ==========================================
        //             RISK REPORTS
        // ==========================================
        [HttpGet]
        public IActionResult RiskReports()
        {
            List<RiskReport> allRisks = risk_dal.GetAllRiskReports(conn);
            return View(allRisks);
        }

        [HttpGet]
        public IActionResult CreateRisk()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateRisk(RiskReport risk)
        {
            risk_dal.AddRiskReport(conn, risk);
            return RedirectToAction("RiskReports");
        }

        [HttpGet]
        public IActionResult EditRisk(int id)
        {
            RiskReport risk = risk_dal.GetRiskReportById(conn, id);
            if (risk == null) return NotFound();
            return View(risk);
        }

        [HttpPost]
        public IActionResult EditRisk(RiskReport risk)
        {
            risk_dal.UpdateRiskReport(conn, risk);
            return RedirectToAction("RiskReports");
        }

        [HttpGet]
        public IActionResult DeleteRisk(int id)
        {
            risk_dal.DeleteRiskReport(conn, id);
            return RedirectToAction("RiskReports");
        }
    }
}
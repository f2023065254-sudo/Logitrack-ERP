using Microsoft.AspNetCore.Mvc;
using Logitrack_ERP.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Logitrack_ERP.Controllers
{
    public class AccountController : Controller
    {
        private readonly string conn;
        private User_DAL user_dal = new User_DAL();

        public AccountController(IConfiguration configuration)
        {
            conn = configuration.GetConnectionString("DefaultConnection");
        }

        // Security: Password ko encrypt karne ke liye
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // ================= SIGNUP =================
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Check if user already exists
                if (user_dal.CheckUserExists(conn, model.UserName, model.Email))
                {
                    ViewBag.ErrorMessage = "Username or Email already exists!";
                    return View(model);
                }

                // 2. Create User and Hash the password
                User newUser = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = HashPassword(model.Password), // Encrypted
                    Role = model.Role
                };

                // 3. Save to DB
                user_dal.RegisterUser(conn, newUser);
                TempData["SuccessMessage"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // ================= LOGIN =================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Encrypt entered password to match DB
                string hashedPassword = HashPassword(model.Password);
                User user = user_dal.AuthenticateUser(conn, model.Email, hashedPassword);

                if (user != null)
                {
                    // Setup Session Variables
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("Role", user.Role);

                    // Redirect based on Role
                    if (user.Role == "Owner" || user.Role == "Admin")
                        return RedirectToAction("OwnerDashboard", "Dashboard");
                    else if (user.Role == "Manager")
                        return RedirectToAction("ManagerDashboard", "Dashboard");
                    else if (user.Role == "Driver")
                        return RedirectToAction("DriverDashboard", "Dashboard");
                    else if (user.Role == "Customer")
                        return RedirectToAction("CustomerDashboard", "Dashboard");
                    else
                        return RedirectToAction("EmployeeDashboard", "Dashboard");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Email or Password!";
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
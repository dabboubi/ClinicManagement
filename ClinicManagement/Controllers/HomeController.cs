using ClinicManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ClinicManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region Dashboard Statistics
        public ActionResult TotalPatients()
        {
            var patients = _context.Patients.ToList();
            return Json(patients.Count());
        }
        //Today's patients
        public ActionResult TodaysPatients()
        {
            DateTime today = DateTime.Now.Date;
            var patients = _context.Patients.Where(p => p.DateTime >= today).ToList();
            return Json(patients.Count());
        }
        public ActionResult TotalAppointments()
        {
            var appointments = _context.Appointments.ToList();
            return Json(appointments.Count());
        }
        //Todays appointments
        public ActionResult TodaysAppointments()
        {
            DateTime today = DateTime.Now.Date;
            var appointments = _context.Appointments
                .Where(a => a.StartDateTime >= today)
                .ToList();
            return Json(appointments.Count());
        }
        public ActionResult TotalDoctors()
        {
            var doctors = _context.Doctors.ToList();
            return Json(doctors.Count());
        }
        //Available doctors
        public ActionResult AvailableDoctors()
        {
            var doctors = _context.Doctors
                .Where(d => d.IsAvailable)
                .ToList();
            return Json(doctors.Count());
        }
        public ActionResult TotalUsers()
        {
            var roles = _userManager.Users.ToList();
            return Json(roles.Count());
        }
     
        //Active Accounts
        public ActionResult ActiveAccounts()
        {
            var users = _context.Users
                .Where(u => u.IsActive == true)
                .ToList();
            return Json(users.Count());
        }
        #endregion
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

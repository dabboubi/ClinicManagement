using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Controllers
{
    public class ReportsController : Controller
    {
    
        
        public IActionResult Appointments()
        {
            return View();
        }
    }
}

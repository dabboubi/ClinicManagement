using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public string Roles { get; set; }
   
      
    }
}

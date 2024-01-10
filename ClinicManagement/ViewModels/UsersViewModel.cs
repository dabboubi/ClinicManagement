using ClinicManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.ViewModels
{
    public class UsersViewModel: BaseViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool? IsActive { get; set; }
    }
}

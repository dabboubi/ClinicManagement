using ClinicManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.ViewModels
{
    public class AppointmentsViewModel : BaseViewModel
    {
        public IEnumerable<Appointment> Appointments { get; set; }
    }
}

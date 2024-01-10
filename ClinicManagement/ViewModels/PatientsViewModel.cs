using ClinicManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.ViewModels
{
    public class PatientsViewModel : BaseViewModel
    {
        public IEnumerable<Patient> Patients { get; set; }
       
    }
}

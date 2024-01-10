using ClinicManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.ViewModels
{
    public class PatientEditViewModel: AddPatientViewModel
    {
        public string ExistingPhotoPath { get; set; }
    }
}

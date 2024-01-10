using ClinicManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.ViewModels
{
    public class AddDoctorViewModel :Doctor
    {
        public AddDoctorViewModel()
        {
            Specializations = new List<Specialization>();

        }
        public string Heading { get; set; }
        public IEnumerable<Specialization> Specializations { get; set; }
        //  a property to represent the selected pecialization
        [Required(ErrorMessage = "Specialization is required")]
        public int? SelectedSpecializationId { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }

    }
}

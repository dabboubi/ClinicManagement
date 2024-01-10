using ClinicManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.ViewModels
{
    public class AppointmentFormViewModel : Appointment
    {
        public AppointmentFormViewModel()
        {
            Doctors = new List<Doctor>();
        }
        public string Heading { get; set; }
        [Required(ErrorMessage = "Doctor is required")]
        public int SelectedDoctorId { get; set; }
        public IEnumerable<Doctor> Doctors { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Time { get; set; }
        [Required]
        public int SelectedPatientId { get; set; }
        public DateTime GetStartDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", Date, Time));
        }
    }
}

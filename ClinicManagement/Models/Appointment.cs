using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        [Required]
        public string Detail { get; set; }
        [Required]
        public bool Status { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public bool IsAvailable { get; set; }
        public string Address { get; set; }
        public int? SpecializationId { get; set; }
        public Specialization Specialization { get; set; }
        public string PhysicianId { get; set; }
        public ApplicationUser Physician { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public Doctor()
        {
            Appointments = new Collection<Appointment>();
           // Specializations = new Collection<Specialization>();

        }
    }
}

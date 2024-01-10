using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Token { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public Gender? Sex { get; set; }
        [Required(ErrorMessage = "Birth Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
      
        public byte? CityId { get; set; }
        public City City { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime DateTime { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public int? Age
        {
            get
            {

                var now = DateTime.Today;
                var age = now.Year - BirthDate.Value.Year;

                if (BirthDate.Value > now.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
        }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Attendance> Attendances { get; set; }

        public Patient()
        {
            Appointments = new Collection<Appointment>();
            Attendances = new Collection<Attendance>();
        }
    }
}

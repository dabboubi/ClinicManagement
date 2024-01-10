using ClinicManagement.Core.Helpers;
using ClinicManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.ViewModels
{
    public class AddPatientViewModel: Patient
    {
        public AddPatientViewModel()
        {
            Cities = new List<City>(); // Initialize with an empty list or retrieve from your data source
             //populating the Genders property correctly:
            Genders = Enum.GetValues(typeof(Gender))
                .Cast<Gender>()
                .Select(g => new SelectListItem
                {
                    Value = ((int)g).ToString(),
                    Text = g.GetDescription()
                }).ToList();
        }
        public string Heading { get; set; }
       
        public IEnumerable<City> Cities { get; set; }
        //  a property to represent the selected city
        [Required(ErrorMessage = "City is required")]
        public byte? SelectedCityId { get; set; }
        //  properties for gender selection
        public IEnumerable<SelectListItem> Genders { get; set; }
        [Required(ErrorMessage = "Sex is required")]
        public int? SelectedGenderId { get; set; }
      

    }
}

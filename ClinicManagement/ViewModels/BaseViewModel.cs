using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.ViewModels
{
    public class BaseViewModel
    {
        public int? SelectedEntries { get; set; }
        public int? CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
     
    }
}

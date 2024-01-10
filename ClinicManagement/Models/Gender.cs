using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Models
{
    public enum Gender
    {
        [Description("Male")]
        Male = 1,

        [Description("Female")]
        Female=2
    }
}

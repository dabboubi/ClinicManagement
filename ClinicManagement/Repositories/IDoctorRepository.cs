using ClinicManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Repositories
{
    public interface IDoctorRepository :IRepository<Doctor>
    {
        IEnumerable<Doctor> GetAvailableDoctors();
        Doctor GetProfile(string userId);
    }
}

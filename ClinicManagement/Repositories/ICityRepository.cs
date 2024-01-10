using ClinicManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Repositories
{
    public interface ICityRepository
    {
        IEnumerable<City> GetCities();
        City GetCity(int id);
    }
}

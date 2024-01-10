using ClinicManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly AppDbContext _context;

        public CityRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities.ToList();
        }

        public City GetCity(int id)
        {
            return _context.Cities.Find(id);
        }
    }
}

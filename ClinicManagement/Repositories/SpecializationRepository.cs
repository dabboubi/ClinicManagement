using ClinicManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly AppDbContext _context;
        public SpecializationRepository(AppDbContext context)
        {
            _context = context;
        }

        public Specialization GetSpecialization(int id)
        {
            return _context.Specializations.Find(id);
        }

        public IEnumerable<Specialization> GetSpecializations()
        {
            
            return _context.Specializations.ToList();
        }
    }
}

using ClinicManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Repositories
{
   public interface ISpecializationRepository
    {
        IEnumerable<Specialization> GetSpecializations();
        Specialization GetSpecialization(int id);
    }
}

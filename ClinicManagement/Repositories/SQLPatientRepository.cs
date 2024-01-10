using ClinicManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Models
{
    public class SQLPatientRepository : IPatientRepository
    {
        private readonly AppDbContext context;
        public SQLPatientRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Patient> GetAll()
        {
            // Include the City information in the query
            return context.Patients.Include(p => p.City).ToList();
        }
        public Patient Add(Patient patient)
        {
            context.Patients.Add(patient);
            context.SaveChanges();
            return patient;
        }
        public Patient Get(int id)
        {
          
            var patient = context.Patients
      .Include(c => c.City)
      .SingleOrDefault(p => p.Id == id);

            if (patient == null)
            {
                // Throw an exception or handle the case appropriately
                throw new InvalidOperationException($"Patient with id {id} not found.");
            }

            return patient;
        }
        public void Remove(int id)
        {
            var patient = context.Patients.Find(id);
            if (patient != null)
            {
                context.Patients.Remove(patient);
                context.SaveChanges();
            }
        }
        public Patient Update(Patient patientChanges)
        {
            var patient = context.Patients.Attach(patientChanges);
            patient.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return patientChanges;
        }
    
        // Implementation for searching patients by name
        public IEnumerable<Patient> GetByName(string name)
        {
            //return context.Patients.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            return context.Patients.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public IEnumerable<Patient> GetRecent()
        {
            return context.Patients
                .Where(a => EF.Functions.DateDiffDay(a.DateTime, DateTime.Now) == 0)
                .Include(c => c.City);

        }
    }
}

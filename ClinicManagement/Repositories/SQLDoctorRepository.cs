using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Repositories
{
    public class SQLDoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext context;
        public SQLDoctorRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Doctor Update(Doctor doctorChanges)
        {
            var doctor = context.Doctors.Attach(doctorChanges);
            doctor.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return doctorChanges;
        }
        public IEnumerable<Doctor> GetAll()
        {
            return context.Doctors.Include(p => p.Specialization).ToList();
        }
        public Doctor Get(int id)
        {
            var doctor = context.Doctors
             .Include(c => c.Specialization)
            .SingleOrDefault(p => p.Id == id);
            return doctor;
        }
        
        public Doctor Add(Doctor doctor)
        {
            context.Doctors.Add(doctor);
            context.SaveChanges();
            return doctor;
        }
        public void Remove(int id)
        {
            var doctor = context.Doctors.Find(id);
            if (doctor != null)
            {
                context.Doctors.Remove(doctor);
                context.SaveChanges();
            }
        }
        // Implementation for searching patients by name
        public IEnumerable<Doctor> GetByName(string name)
        {
            return context.Doctors.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public IEnumerable<Doctor> GetRecent()
        {
            throw new NotImplementedException();
        }
       
        public IEnumerable<Doctor> GetAvailableDoctors()
        {
            return context.Doctors
                .Where(a => a.IsAvailable == true)
                .Include(s => s.Specialization)
                .ToList();
        }

        public Doctor GetProfile(string userId)
        {
            return context.Doctors
                .Include(s => s.Specialization)
                .Include(u => u.Physician)
                .SingleOrDefault(d => d.PhysicianId == userId);
        }
    }
}

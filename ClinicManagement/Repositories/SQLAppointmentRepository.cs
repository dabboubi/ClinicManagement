using ClinicManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Repositories
{
    public class SQLAppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext context;
        public SQLAppointmentRepository(AppDbContext context)
        {
            this.context = context;
        }
        public void Add(Appointment appointment)
        {
            context.Appointments.Add(appointment);
            context.SaveChanges();

        }
     
  
        /// Validate appointment date and time
       
        public bool ValidateAppointment(DateTime appntDate, int id)
        {
            return context.Appointments.Any(a => a.StartDateTime == appntDate && a.DoctorId == id);
        }

        /// Get number of appointments for defined patient

        public int CountAppointments(int id)
        {
            return context.Appointments.Count(a => a.PatientId == id);
        }
        /// Get single appointment
        public Appointment GetAppointment(int id)
        {
            return context.Appointments.Find(id);
        }
        
        /// Get appointments for single doctor
        public IEnumerable<Appointment> GetAppointmentByDoctor(int id)
        {
            // return context.Appointments.Where(p => p.Doctor.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            return context.Appointments
                 .Where(d => d.DoctorId == id)
                 .Include(p => p.Patient)
                 .ToList();
        }
        /// Get appointments for single patient
        public IEnumerable<Appointment> GetAppointmentWithPatient(int id)
        {
           // return context.Appointments.Where(p => p.Patient.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            return context.Appointments
                .Where(p => p.PatientId == id)
                .Include(p => p.Patient)
                .Include(d => d.Doctor)
                .ToList();
        }
        /// Get all appointments
        public IEnumerable<Appointment> GetAppointments()
        {
            return context.Appointments
               .Include(p => p.Patient)
               .Include(d => d.Doctor)
               .ToList();
        }
        /// Get upcomming appointments for doctor - Admin section
        IEnumerable<Appointment> IAppointmentRepository.GetTodaysAppointments(int id)
        {
            DateTime today = DateTime.Now.Date;
            return context.Appointments
                .Where(d => d.DoctorId == id && d.StartDateTime >= today)
                .Include(p => p.Patient)
                .OrderBy(d => d.StartDateTime)
                .ToList();
        }
        /// Get upcomming appointments for specific doctor
        IEnumerable<Appointment> IAppointmentRepository.GetUpcommingAppointments(string userId)
        {
            DateTime today = DateTime.Now.Date;
            return context.Appointments
                .Where(d => d.Doctor.PhysicianId == userId && d.StartDateTime >= today && d.Status == true)
                .Include(p => p.Patient)
                .OrderBy(d => d.StartDateTime)
                .ToList();
        }
        /// Get Daily appointments
        IEnumerable<Appointment> IAppointmentRepository.GetDaillyAppointments(DateTime getDate)
        {
            return context.Appointments.Where(a => EF.Functions.DateDiffDay(a.StartDateTime, getDate) == 0)
              .Include(p => p.Patient)
              .Include(d => d.Doctor)
              .ToList();
        }
        public Appointment Update(Appointment appointmentChanges)
        {
            var appointment = context.Appointments.Attach(appointmentChanges);
            appointment.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return appointmentChanges;
        }
    }
}

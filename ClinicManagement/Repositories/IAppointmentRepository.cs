using ClinicManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Repositories
{
    public interface IAppointmentRepository 
    {
        IEnumerable<Appointment> GetAppointments();
        IEnumerable<Appointment> GetAppointmentWithPatient(int id);
        IEnumerable<Appointment> GetAppointmentByDoctor(int id);
        IEnumerable<Appointment> GetTodaysAppointments(int id);
        IEnumerable<Appointment> GetUpcommingAppointments(string userId);
        IEnumerable<Appointment> GetDaillyAppointments(DateTime getDate);
        //IQueryable<Appointment> FilterAppointments(AppointmentSearchVM searchModel);
         bool ValidateAppointment(DateTime appntDate, int id);
         int CountAppointments(int id);
         Appointment GetAppointment(int id);
        Appointment Update(Appointment appointmentChanges);
         void Add(Appointment appointment);
    }
}

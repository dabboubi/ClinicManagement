using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicManagement.Models
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        /*For the DbContext class to be able to do any useful work, it needs an instance of the DbContextOptions class.
        The DbContextOptions instance carries configuration information such as the connection string, database provider to use etc.
        To pass the DbContextOptions instance we use the constructor*/
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options)
        {
                
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}

using AppointmentService.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentService.Models
{
    public class AppointmentServiceContext: DbContext
    {
        public DbSet<Patient> Patient { get; set; }

        public DbSet<Doctor> Doctor { get; set; }

        public DbSet<AppointmentStatus> AppointmentStatus { get; set; }

        public DbSet<AppointmentHistory> AppointmentHistory { get; set; }

        public DbSet<Appointment> Appointment { get; set; }

        public AppointmentServiceContext(DbContextOptions<AppointmentServiceContext> options) : base(options) { }
    }
}

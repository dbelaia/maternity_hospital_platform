using Microsoft.EntityFrameworkCore;

namespace MaternityMicroservices.Models
{
    public class MaternityAppointmentServiceContext: DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctor { get; set; }

        public DbSet<AppointmentStatus> AppointmentStatus { get; set; }

        public DbSet<AppointmentHistory> AppointmentHistory { get; set; }

        public DbSet<AppointmentTypes>  AppointmentTypes { get; set; }

        public MaternityAppointmentServiceContext(DbContextOptions<MaternityAppointmentServiceContext> options) : base(options) { }

    }
}

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AnalyticsService.Models
{
    public class AnalyticsServiceContext : DbContext
    {
        public DbSet<Patient> Patient { get; set; }

        public DbSet<Doctor> Doctor { get; set; }

        public DbSet<Operation> Operation { get; set; }

        public DbSet<OperationHistory> OperationHistory { get; set; }

        public AnalyticsServiceContext(DbContextOptions<AnalyticsServiceContext> options) : base(options) { }

    }
}

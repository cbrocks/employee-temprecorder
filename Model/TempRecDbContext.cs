using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTempRecorder.Model
{
    public class TempRecDbContext : DbContext
    {
        public TempRecDbContext(DbContextOptions<TempRecDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AuditEntry> AuditEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace netlush.Models
{
    public class AppDbContext : DbContext
    {
        //public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<WorkerModel>()
            // .Property(w => w._worker_multiNationality).HasColumnName("worker_multiNationality");
            //modelBuilder.Entity<WorkerModel>()
            //    .Property(w => w._worker_area).HasColumnName("worker_area");
            //modelBuilder.Entity<WorkerModel>()
            //   .Property(w => w._worker_profession).HasColumnName("worker_profession");
            
        }

        // public DbSet<EmployeeModel> employees { get; set; }
    }
}

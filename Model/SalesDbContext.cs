using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Many_To_Many.Model;
namespace Many_To_Many.Model
{
    public class SalesDbContext:DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> option):base(option)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sales>()
                .HasOne(o => o.employee)
                .WithMany(i => i.salesE)
                .HasForeignKey(i => i.EmployeeId);

            modelBuilder.Entity<Sales>()
                .HasOne(o => o.product)
                .WithMany(i => i.salesP)
                .HasForeignKey(i => i.ProductId);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Sales> Sales { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Minimal.Api.Models;

namespace Minimal.Api.Data
{
    public class MinimalContextDb : DbContext
    {
        public MinimalContextDb(DbContextOptions<MinimalContextDb> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasKey(p => p.Id)
                .HasName("id");

            modelBuilder.Entity<Customer>()
                .Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(200)")
                .HasColumnName("name");

            modelBuilder.Entity<Customer>()
                .Property(p => p.Document)
                .IsRequired()
                .HasColumnType("varchar(14)")
                .HasColumnName("document");

            modelBuilder.Entity<Customer>()
                .Property(p => p.Active)
                .IsRequired()
                .HasColumnName("active");

            modelBuilder.Entity<Customer>()
                .ToTable("customers");

            base.OnModelCreating(modelBuilder);
        }
    }
}
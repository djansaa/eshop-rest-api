using eshop_rest_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eshop_rest_api.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Product>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Id).ValueGeneratedNever();
                e.Property(p => p.Name).IsRequired();
                e.Property(p => p.ImgUri).IsRequired();
                e.Property(p => p.Price).IsRequired();
            });

            // initial db seed
            mb.Entity<Product>().HasData(DbSeed.Items);
        }
    }
}

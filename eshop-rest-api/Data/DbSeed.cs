using eshop_rest_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eshop_rest_api.Data
{
    public static class DbSeed
    {
        public static async Task RunAsync(AppDbContext db)
        {
            if (db.Database.IsSqlite()) await db.Database.EnsureCreatedAsync();
            else await db.Database.MigrateAsync();

            if (await db.Products.AnyAsync()) return;

            var items = new[]
            {
            new Product { Id=101,   Name="Tea",    ImgUri="https://placeholder.com/101", Price=49.90m, Description="Green tea" },
            new Product { Id=102,   Name="Coffee", ImgUri="https://placeholder.com/102", Price=89.00m, Description="Arabica" },
            new Product { Id=103,   Name="Milk",   ImgUri="https://placeholder.com/103", Price=29.50m },
            new Product { Id=104,   Name="Bread",  ImgUri="https://placeholder.com/104", Price=39.00m }
        };

            await db.Products.AddRangeAsync(items);
            await db.SaveChangesAsync();
        }
    }
}

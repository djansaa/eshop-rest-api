using eshop_rest_api.Data;
using eshop_rest_api.Models;
using Microsoft.EntityFrameworkCore;

namespace eshop_rest_api.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        public Task<List<Product>> GetAllAsync()
        {
            return _db.Products.AsNoTracking().ToListAsync();
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            return _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> UpdateDescriptionAsync(int id, string description)
        {
            var p = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (p is null) return null;

            p.Description = description;
            await _db.SaveChangesAsync();

            return p;

        }
    }
}

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

        public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _db.Products.AsNoTracking().ToListAsync(ct);

            return list;
        }

        public Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<bool> UpdateDescriptionAsync(int id, string description, CancellationToken ct = default)
        {
            var affected = await _db.Products.Where(p => p.Id == id).ExecuteUpdateAsync(id => id.SetProperty(p => p.Description, description), ct);

            return affected == 1;
        }

        public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPageAsync(int page, int pageSize, CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 1;

            var query = _db.Products.AsNoTracking();

            var total = await query.CountAsync(ct);

            // paging in db
            var items = await query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, total);
        }
    }
}

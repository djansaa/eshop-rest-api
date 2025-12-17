using eshop_rest_api.Data;
using eshop_rest_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace eshop_rest_api.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;
        private readonly HybridCache _cache;

        public ProductService(AppDbContext db, HybridCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _db.Products.AsNoTracking().ToListAsync(ct);

            return list;
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _cache.GetOrCreateAsync(
                key: $"products:id:{id}",
                factory: async token => await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct),
                options: new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(10),
                    LocalCacheExpiration = TimeSpan.FromMinutes(2),
                },
                tags: new[] { "products", $"products:{id}" },
                cancellationToken: ct);
        }

        public async Task<bool> UpdateDescriptionAsync(int id, string description, CancellationToken ct = default)
        {
            var affected = await _db.Products.Where(p => p.Id == id).ExecuteUpdateAsync(id => id.SetProperty(p => p.Description, description), ct);

            // invalidate updated product in cache
            await _cache.RemoveAsync($"products:id:{id}", ct);
            await _cache.RemoveByTagAsync("products:list", ct);

            return affected == 1;
        }

        public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPageAsync(int page, int pageSize, CancellationToken ct = default)
        {
            // normalize page
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 1;

            return await _cache.GetOrCreateAsync(
                key: $"products:list:{page}",
                factory: async token =>
                {
                    var query = _db.Products.AsNoTracking();

                    var total = await query.CountAsync(token);

                    var items = await query
                        .OrderBy(p => p.Id)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync(token);

                    return ((IReadOnlyList<Product>)items, total);
                },
                options: new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(10),
                    LocalCacheExpiration = TimeSpan.FromMinutes(2),
                },
                tags: new[] { "products", "products:list" },
                cancellationToken: ct);
        }
    }
}

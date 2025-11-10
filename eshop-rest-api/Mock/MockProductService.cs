using eshop_rest_api.Models;
using eshop_rest_api.Services;

namespace eshop_rest_api.Mock
{
    public class MockProductService : IProductService
    {
        private readonly List<Product> _db;

        public MockProductService()
        {
            _db = Enumerable.Range(1, 25).Select(i => new Product { Id = i, Name = $"P{i}", ImgUri = $"img{i}.png", Price = i * 10m, Description = $"D{i}" }).ToList();
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default) => await Task.FromResult(_db.ToList());
        public Task<Product?> GetByIdAsync(int id, CancellationToken ct = default) => Task.FromResult(_db.FirstOrDefault(p => p.Id == id));

        public Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPageAsync(int page, int pageSize, CancellationToken ct)
        {
            if (ct.IsCancellationRequested) return Task.FromCanceled<(IReadOnlyList<Product> Items, int TotalCount)>(ct);

            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 1;

            var ordered = _db.OrderBy(p => p.Id);
            var total = ordered.Count();

            var items = ordered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Task.FromResult(((IReadOnlyList<Product>)items, total));
        }

        public Task<Product?> UpdateDescriptionAsync(int id, string? description, CancellationToken ct = default)
        {
            var p = _db.FirstOrDefault(x => x.Id == id);
            if (p is null) return Task.FromResult<Product?>(null);
            p.Description = description;
            return Task.FromResult<Product?>(p);
        }
    }
}

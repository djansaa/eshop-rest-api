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

        public Task<List<Product>> GetAllAsync() => Task.FromResult(_db.ToList());
        public Task<Product?> GetByIdAsync(int id) => Task.FromResult(_db.FirstOrDefault(p => p.Id == id));
        public Task<Product?> UpdateDescriptionAsync(int id, string? description)
        {
            var p = _db.FirstOrDefault(x => x.Id == id);
            if (p is null) return Task.FromResult<Product?>(null);
            p.Description = description;
            return Task.FromResult<Product?>(p);
        }
    }
}

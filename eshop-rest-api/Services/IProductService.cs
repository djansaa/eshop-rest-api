using eshop_rest_api.Models;

namespace eshop_rest_api.Services
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct);
        Task<Product?> GetByIdAsync(int id, CancellationToken ct);
        Task<bool> UpdateDescriptionAsync(int id, string? description, CancellationToken ct);
        Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPageAsync(int page, int pageSize, CancellationToken ct);
    }
}

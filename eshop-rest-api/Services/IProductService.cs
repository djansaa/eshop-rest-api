using eshop_rest_api.Models;

namespace eshop_rest_api.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> UpdateDescriptionAsync(int id, string? description);
    }
}

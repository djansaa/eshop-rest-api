
namespace eshop_rest_api.DTOs
{
    public record ProductDTO(int Id, string Name, string ImgUri, decimal Price, string? Description);
    public record UpdateProductDescriptionDTO(string Description);
}

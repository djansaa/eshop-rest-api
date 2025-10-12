
using System.ComponentModel.DataAnnotations;

namespace eshop_rest_api.DTOs
{
    /// <summary>
    /// Represents a data transfer object for a product, containing essential product details.
    /// </summary>
    /// <remarks>This record is typically used to transfer product information between application layers. All
    /// required properties must be provided when creating an instance of this record.</remarks>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="ImgUri"></param>
    /// <param name="Price"></param>
    /// <param name="Description"></param>
    public record ProductDTO([property: Required] int Id, [property: Required] string Name, [property: Required] string ImgUri, [property: Required] decimal Price, string? Description);

    /// <summary>
    /// Represents the data transfer object for updating a product's description.
    /// </summary>
    /// <remarks>This DTO is used to encapsulate the new description of a product when performing an update
    /// operation. The description can be null to indicate that the product's description should be cleared.</remarks>
    /// <param name="Description">The new description for the product. Can be null to remove the existing description.</param>
    public record UpdateProductDescriptionDTO(string? Description);
}

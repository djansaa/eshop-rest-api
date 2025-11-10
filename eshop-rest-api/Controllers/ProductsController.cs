using Asp.Versioning;
using eshop_rest_api.DTOs;
using eshop_rest_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace eshop_rest_api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _svc;

        public ProductsController(ILogger<ProductsController> logger, IProductService svc)
        {
            _logger = logger;
            _svc = svc;
        }

        /// <summary>
        /// Retrieves a list of all products.
        /// </summary>
        /// <remarks>This method returns all available products as a collection of <see
        /// cref="ProductDTO"/> objects. Each product includes its ID, name, image URI, price, and
        /// description.</remarks>
        /// <returns>A collection of <see cref="ProductDTO"/> objects representing the available products.</returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDTO>))]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllV1(CancellationToken ct)
        {
            var products = await _svc.GetAllAsync(ct);

            _logger.LogInformation("Returned {Count} products", products.Count);

            return Ok(products.Select(p => new ProductDTO(p.Id, p.Name, p.ImgUri, p.Price, p.Description)));
        }

        /// <summary>
        /// Retrieves a paginated list of products.
        /// </summary>
        /// <remarks>This method supports pagination by accepting query parameters for the page number and
        /// page size.  If the specified page exceeds the total number of pages, the last page is returned. Pagination
        /// metadata includes links to navigate between pages.</remarks>
        /// <param name="page">The page number to retrieve. Must be 1 or greater. Defaults to 1.</param>
        /// <param name="pageSize">The number of items per page. Must be 1 or greater. Defaults to 10.</param>
        /// <returns>A <see cref="PagedDTO{T}"/> containing a collection of <see cref="ProductDTO"/> objects and pagination
        /// metadata.</returns>
        [HttpGet]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(200, Type = typeof(PagedDTO<ProductDTO>))]
        public async Task<ActionResult<PagedDTO<ProductDTO>>> GetAllV2(CancellationToken ct, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (items, totalCount) = await _svc.GetPageAsync(page, pageSize, ct);

            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
            if (page > totalPages) page = totalPages;

            var data = items.Select(p => new ProductDTO(p.Id, p.Name, p.ImgUri, p.Price, p.Description));

            string BasePath() => $"{Request.Scheme}://{Request.Host}{Request.Path}";
            string Link(int p) => QueryHelpers.AddQueryString(BasePath(), new Dictionary<string, string?>
            {
                ["page"] = p.ToString(),
                ["pageSize"] = pageSize.ToString()
            });

            var links = new PaginationLinksDTO(
                Self: Link(page),
                First: Link(1),
                Previous: page > 1 ? Link(page - 1) : null,
                Next: page < totalPages ? Link(page + 1) : null,
                Last: Link(totalPages)
            );

            var meta = new PaginationDTO(page, pageSize, totalPages, links);

            _logger.LogInformation("Returned {Count} products via page {Page} of {Pages}", items.Count, page, totalPages);
            return Ok(new PagedDTO<ProductDTO>(data, meta));
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <remarks>This method returns a 200 OK response with the product details if the product exists,
        /// or a 404 Not Found response if no product with the specified <paramref name="id"/> is found.</remarks>
        /// <param name="id">The unique identifier of the product to retrieve. Must be a positive integer.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="ProductDTO"/> if the product is found; otherwise, a
        /// 404 Not Found response.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type = typeof(ProductDTO))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDTO>> GetById(int id, CancellationToken ct)
        {
            var product = await _svc.GetByIdAsync(id, ct);

            if (product is null)
            {
                _logger.LogWarning("Product {ProductId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Product {ProductId} fetched", id);
            return Ok(new ProductDTO(product.Id, product.Name, product.ImgUri, product.Price, product.Description));
        }

        /// <summary>
        /// Updates the description of an existing product.
        /// </summary>
        /// <remarks>This method performs a partial update on the product's description. If the product is
        /// not found, a 404 response is returned. On success, a 200 response is returned with the updated product
        /// details.</remarks>
        /// <param name="id">The unique identifier of the product to update. Must be a positive integer.</param>
        /// <param name="dto">An object containing the new description for the product.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the updated <see cref="ProductDTO"/> if the operation succeeds, 
        /// or a <see cref="NotFoundResult"/> if no product with the specified <paramref name="id"/> exists.</returns>
        [HttpPatch("{id:int}/description")]
        [ProducesResponseType(200, Type = typeof(ProductDTO))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDTO>> UpdateDescription(int id, [FromBody] UpdateProductDescriptionDTO dto, CancellationToken ct)
        {
            var updatedProduct = await _svc.UpdateDescriptionAsync(id, dto.Description, ct);

            if (updatedProduct is null)
            {
                _logger.LogWarning("UpdateDescription failed: product {ProductId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Description updated for product {ProductId}", id);
            return Ok(new ProductDTO(updatedProduct.Id, updatedProduct.Name, updatedProduct.ImgUri, updatedProduct.Price, updatedProduct.Description));
        }
    }
}

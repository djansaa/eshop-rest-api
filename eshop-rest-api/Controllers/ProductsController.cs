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

        [HttpGet]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200, Type=typeof(IEnumerable<ProductDTO>))]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetV1()
        {
            var products = await _svc.GetAllAsync();

            _logger.LogInformation("Returned {Count} products", products.Count);

            return Ok(products.Select(p => new ProductDTO(p.Id, p.Name, p.ImgUri, p.Price, p.Description)));
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        [ProducesResponseType(200, Type=typeof(IEnumerable<ProductDTO>))]
        public async Task<ActionResult<PagedDTO<ProductDTO>>> GetV2([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // check query filters
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 1;

            // get data
            var products = await _svc.GetAllAsync();

            // calculate total pages
            var totalPages = Math.Max(1, (int)Math.Ceiling(products.Count / (double)pageSize));
            if (page > totalPages) page = totalPages;

            // apply pagination
            var data = products.Skip((page - 1) * pageSize).Take(pageSize).Select(p => new ProductDTO(p.Id, p.Name, p.ImgUri, p.Price, p.Description));

            // pagination link local methods
            string BasePath() => $"{Request.Scheme}://{Request.Host}{Request.Path}";
            string Link(int p) => QueryHelpers.AddQueryString(BasePath(), new Dictionary<string, string?>
            {
                ["page"] = p.ToString(),
                ["pageSize"] = pageSize.ToString()
            });

            // prepare metadata
            var links = new PaginationLinksDTO(
                Self: Link(page),
                First: Link(1),
                Previous: page > 1 ? Link(page - 1) : null,
                Next: page < totalPages ? Link(page + 1) : null,
                Last: Link(totalPages)
            );

            var meta = new PaginationDTO(page, pageSize, totalPages, links);

            _logger.LogInformation("Returned {Count} products through pagination of {Page}. page out of {Pages} pages", pageSize, page, totalPages);

            return Ok(new PagedDTO<ProductDTO>(data, meta));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200, Type=typeof(ProductDTO))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await _svc.GetByIdAsync(id);

            if (product is null)
            {
                _logger.LogWarning("Product {ProductId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Product {ProductId} fetched", id);
            return Ok(new ProductDTO(product.Id, product.Name, product.ImgUri, product.Price, product.Description));
        }

        [HttpPatch("{id:int}/description")]
        [ProducesResponseType(200, Type=typeof(ProductDTO))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDTO>> UpdateDescription(int id, [FromBody] UpdateProductDescriptionDTO dto)
        {
            var updatedProduct = await _svc.UpdateDescriptionAsync(id, dto.Description);

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

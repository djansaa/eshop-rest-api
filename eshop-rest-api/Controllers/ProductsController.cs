using eshop_rest_api.DTOs;
using eshop_rest_api.Models;
using eshop_rest_api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eshop_rest_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            return Ok(await _svc.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var p = await _svc.GetByIdAsync(id);
            return p is null ? NotFound() : Ok(p);
        }

        [HttpPatch("{id:int}/description")]
        public async Task<ActionResult<Product>> UpdateDescription(int id, [FromBody] UpdateProductDescriptionDto dto)
        {
            var updated = await _svc.UpdateDescriptionAsync(id, dto.Description);
            return updated is null ? NotFound() : Ok(updated);
        }
    }
}

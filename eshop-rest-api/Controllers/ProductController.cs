using eshop_rest_api.DTOs;
using eshop_rest_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace eshop_rest_api.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : ApiBaseController<ProductController>
    {
        public ProductController(ILogger<ProductController> logger) : base(logger) { }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    ImgUri = "https://via.placeholder.com/150",
                    Price = 9.99M,
                    Description = "This is product 1"
                },
                new Product
                {
                    Id = 2,
                    Name = "Product 2",
                    ImgUri = "https://via.placeholder.com/150",
                    Price = 19.99M,
                    Description = "This is product 2"
                },
                new Product
                {
                    Id = 3,
                    Name = "Product 3",
                    ImgUri = "https://via.placeholder.com/150",
                    Price = 29.99M,
                    Description = "This is product 3"
                }
            };

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Product> GetById(int id)
        {
            return Ok(
                new Product
                {
                    Id = id,
                    Name = $"Product {id}",
                    ImgUri = "https://via.placeholder.com/150",
                    Price = 9.99M * id,
                    Description = $"This is product {id}"
                }
                );
        }

        [HttpPatch("{id:int}/description")]
        public ActionResult<Product> UpdateDescription(int id, [FromBody] UpdateProductDescriptionDto dto)
        {
            var p = new Product
            {
                Id = id,
                Name = $"Product {id}",
                ImgUri = "https://via.placeholder.com/150",
                Price = 9.99M * id,
                Description = null
            };

            p.Description = dto.Description;
            return Ok(p);
        }
    }
}

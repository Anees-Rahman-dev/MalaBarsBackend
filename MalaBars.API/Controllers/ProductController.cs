using MalaBars.Application.DTO_s;
using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MalaBars.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            return Ok(products);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var FoundProduct = await _productService.GetByIdAsync(id);

            if (FoundProduct == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(FoundProduct);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductDto product)
        {
            var CreatedProduct = await _productService.AddAsync(product);

            return CreatedAtAction(nameof(GetById), new { id = CreatedProduct.ProductId },
                CreatedProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto product)
        {
            var updated = await _productService.UpdateAsync(id, product);

            if (!updated)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }
    }
}

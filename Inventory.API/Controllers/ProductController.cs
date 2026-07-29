using Asp.Versioning;
using Inventory.API.DTOs.Product;
using Inventory.API.Helpers;
using Inventory.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Inventory.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [EnableRateLimiting("GlobalPolicy")]
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet("low-stock")]
        //public async Task<IActionResult> GetLowStock()
        //{
        //    var products = await _service.GetAllAsync(new QueryParameters());

        //    var result = products
        //        .Where(x => x.StockQuantity <= x.MinimumStock);

        //    return Ok(result);
        //}

        // GET: api/Product
        [EnableRateLimiting("ReadPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameters query)
        {
            var products = await _service.GetAllAsync(query);

            return Ok(products);
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);

            if (product == null)
                return NotFound("Product not found.");

            return Ok(product);
        }

        // POST: api/Product
        [Authorize(Roles = Roles.Admin+","+Roles.Manager)]
        [HttpPost]
        [EnableRateLimiting("WritePolicy")]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(dto);

            if (!result)
                return BadRequest("Product already exists or Category not found.");

            return Ok("Product created successfully.");
        }

        // PUT: api/Product/5
        [Authorize(Roles = Roles.Admin+","+Roles.Manager)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateAsync(id, dto);

            if (!result)
                return NotFound("Product not found.");

            return Ok("Product updated successfully.");
        }

        // DELETE: api/Product/5
        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound("Product not found.");

            return Ok("Product deleted successfully.");
        }

    }
}
using Inventory.API.DTOs.Supplier;
using Inventory.API.Helpers;
using Inventory.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _service;

        public SupplierController(ISupplierService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? search, int pageNumber = 1, int pageSize = 10)
        {
            return Ok(await _service.GetAllAsync(search,pageNumber,pageSize));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = await _service.GetByIdAsync(id);

            if (supplier == null)
                return NotFound();

            return Ok(supplier);
        }

        [Authorize(Roles = Roles.Admin+","+Roles.Manager)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateSupplierDto dto)
        {
            var result = await _service.CreateAsync(dto);

            if (!result)
                return BadRequest("Supplier already exists.");

            return Ok("Supplier created successfully.");
        }

        [Authorize(Roles = Roles.Admin+","+Roles.Manager)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateSupplierDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (!result)
                return NotFound();

            return Ok("Supplier updated successfully.");
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound();

            return Ok("Supplier deleted successfully.");
        }
    }
}
using Inventory.API.DTOs.Purchase;
using Inventory.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _service;

        public PurchaseController(IPurchaseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? search = "", int pageNumber = 1, int pageSize = 10)
        {
            var result = await _service.GetAllAsync(
                search,
                pageNumber,
                pageSize);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var purchase = await _service.GetByIdAsync(id);

            if (purchase == null)
                return NotFound();

            return Ok(purchase);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin+","+Roles.Manager)]
        public async Task<IActionResult> Create(CreatePurchaseDto dto)
        {
            var result = await _service.CreatePurchaseAsync(dto);

            if (!result)
            {
                return BadRequest("Supplier not found.");
            }

            return Ok("Purchase created successfully.");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
        public async Task<IActionResult> Update(int id, UpdatePurchaseDto dto)
        {
            var result = await _service.UpdatePurchaseAsync(id, dto);

            if (!result)
                return BadRequest();

            return Ok("Purchase updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeletePurchaseAsync(id);

            if (!result)
                return BadRequest();

            return Ok("Purchase deleted successfully.");
        }
    }
}
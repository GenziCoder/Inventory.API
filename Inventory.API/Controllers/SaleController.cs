using Inventory.API.DTOs.Sale;
using Inventory.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _service;

        public SaleController(ISaleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            string? search = "",
            int pageNumber = 1,
            int pageSize = 10)
        {
            return Ok(await _service.GetAllAsync(
                search,
                pageNumber,
                pageSize));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sale = await _service.GetByIdAsync(id);

            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
        public async Task<IActionResult> Create(CreateSaleDto dto)
        {
            await _service.CreateSaleAsync(dto);

            return Ok("Sale created successfully.");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
        public async Task<IActionResult> Update(
            int id,
            UpdateSaleDto dto)
        {
            await _service.UpdateSaleAsync(id, dto);

            return Ok("Sale updated successfully.");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteSaleAsync(id);

            return Ok("Sale deleted successfully.");
        }
    }
}
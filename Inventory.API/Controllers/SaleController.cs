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

        [HttpPost]
        [Authorize(Roles = Roles.Admin+","+Roles.Manager+","+Roles.Employee)]
        public async Task<IActionResult> Create(CreateSaleDto dto)
        {
            var result = await _service.CreateSaleAsync(dto);

            if (!result)
                return BadRequest();

            return Ok(new
            {
                Success = true,
                Message = "Sale completed successfully."
            });
        }
    }
}
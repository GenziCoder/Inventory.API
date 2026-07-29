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
    }
}
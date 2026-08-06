using Inventory.API.DTOs.Dashboard;
using Inventory.API.Helpers;
using Inventory.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboard()
        {
            var response = await _service.GetDashboardSummaryAsync();
            return Ok(response);
        }
    }
}
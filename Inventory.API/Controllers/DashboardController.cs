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

        [HttpGet("recent-transactions")]
        public async Task<IActionResult> GetRecentTransactions(int count = 5)
        {
            var response = await _service.GetRecentTransactionsAsync(count);

            return Ok(response);
        }

        [HttpGet("monthly-chart")]
        public async Task<IActionResult> GetMonthlyChart()
        {
            return Ok(await _service.GetMonthlyChartAsync());
        }

        [HttpGet("top-selling-products")]
        public async Task<IActionResult> GetTopSellingProducts(int count = 10)
        {
            var response = await _service.GetTopSellingProductsAsync(count);

            return Ok(response);
        }

        [HttpGet("top-purchased-products")]
        public async Task<IActionResult> GetTopPurchasedProducts(int count = 10)
        {
            return Ok(await _service.GetTopPurchasedProductsAsync(count));
        }

    }
}
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
            var result = await _service.GetDashboardAsync();
            var response = new ApiResponse<object>()
            {
                Success = true,
                Message = " Dashboard summary",
                Data = new DashboardDto()
                {
                    TotalProducts=result.TotalProducts,
                    TotalCustomers=result.TotalCustomers,
                    TotalSuppliers=result.TotalSuppliers,
                    TotalCategories=result.TotalCategories,
                    TodayPurchases=result.TodayPurchases,
                    TodaySales=result.TodaySales,
                    MonthlyPurchases=result.MonthlyPurchases,
                    MonthlySales=result.MonthlySales,
                    LowStockProducts=result.LowStockProducts,
                    OutOfStockProducts=result.OutOfStockProducts
                }
            };

            return Ok(response);
        }
    }
}
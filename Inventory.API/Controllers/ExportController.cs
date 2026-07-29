using Inventory.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExportController : ControllerBase
    {
        private readonly IExportService _service;

        public ExportController(IExportService service)
        {
            _service = service;
        }

        [HttpGet("products")]
        public async Task<IActionResult> ExportProducts()
        {
            var file = await _service.ExportProductsAsync();

            return File(
                file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Products_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }

        [HttpGet("customers")]
        public async Task<IActionResult> ExportCustomers()
        {
            var file = await _service.ExportCustomersAsync();

            return File(file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Customers_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }

        [HttpGet("suppliers")]
        public async Task<IActionResult> ExportSuppliers()
        {
            var file = await _service.ExportSuppliersAsync();

            return File(file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Suppliers_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }

        [HttpGet("sales")]
        public async Task<IActionResult> ExportSales()
        {
            var file = await _service.ExportSalesAsync();

            return File(file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Sales_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }

        [HttpGet("purchases")]
        public async Task<IActionResult> ExportPurchases()
        {
            var file = await _service.ExportPurchasesAsync();

            return File(file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Purchases_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }
        // pdf related endpoint
        [HttpGet("products/pdf")]
        public async Task<IActionResult> ExportProductsPdf()
        {
            var file = await _service.ExportProductsPdfAsync();

            return File(
                file,
                "application/pdf",
                $"Products_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }
    }
}
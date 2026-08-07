using Inventory.API.Common;
using Inventory.API.DTOs.Report;
using Inventory.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service)
        {
            _service = service;
        }

        [HttpGet("sales")]
        public async Task<IActionResult> GetSalesReport([FromQuery] SalesReportFilterDto filter)
        {
            var result = await _service.GetSalesReportAsync(filter);

            return Ok(result);
        }

        [HttpGet("purchases")]
        public async Task<IActionResult> GetPurchaseReport([FromQuery] PurchaseReportFilterDto filter)
        {
            return Ok(await _service.GetPurchaseReportAsync(filter));

        }
        [HttpGet("stock")]
        public async Task<IActionResult> GetStockReport([FromQuery] StockReportFilterDto filter)
        {
            return Ok(await _service.GetStockReportAsync(filter));
        }

        [HttpGet("stock-ledger/{productId}")]
        public async Task<IActionResult> GetStockLedger(int productId)
        {
            return Ok(await _service.GetStockLedgerAsync(productId));

        }

        [HttpGet("profit")]
        public async Task<IActionResult> GetProfitReport([FromQuery] ProfitReportFilterDto filter)
        {
            return Ok(await _service.GetProfitReportAsync(filter));

        }
        [HttpGet("customer-sales")]
        public async Task<IActionResult> GetCustomerSalesReport([FromQuery] CustomerSalesReportFilterDto filter)
        {
            return Ok(await _service.GetCustomerSalesReportAsync(filter));
        }

        [HttpGet("supplier-purchases")]
        public async Task<IActionResult> GetSupplierPurchaseReport(
            [FromQuery] SupplierPurchaseReportFilterDto filter)
        {
            return Ok(await _service.GetSupplierPurchaseReportAsync(filter));
        }

    }
}
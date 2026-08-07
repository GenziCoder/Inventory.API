namespace Inventory.API.DTOs.Report
{
    public class StockReportFilterDto
    {
        public string? Search { get; set; }

        public int? CategoryId { get; set; }

        public bool LowStockOnly { get; set; }

        public bool OutOfStockOnly { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
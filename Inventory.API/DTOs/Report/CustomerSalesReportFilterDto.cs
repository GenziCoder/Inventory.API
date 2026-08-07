namespace Inventory.API.DTOs.Report
{
    public class CustomerSalesReportFilterDto
    {
        public int? CustomerId { get; set; }

        public string? Search { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
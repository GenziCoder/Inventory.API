namespace Inventory.API.DTOs.Report
{
    public class SalesReportFilterDto
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? CustomerId { get; set; }

        public string? InvoiceNumber { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
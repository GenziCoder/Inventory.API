namespace Inventory.API.DTOs.Report
{
    public class PurchaseReportFilterDto
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? SupplierId { get; set; }

        public string? PurchaseNumber { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
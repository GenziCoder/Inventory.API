namespace Inventory.API.DTOs.Report
{
    public class SupplierPurchaseReportFilterDto
    {
        public int? SupplierId { get; set; }

        public string? Search { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
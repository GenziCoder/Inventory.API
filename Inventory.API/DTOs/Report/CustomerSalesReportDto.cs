namespace Inventory.API.DTOs.Report
{
    public class CustomerSalesReportDto
    {
        public int CustomerId { get; set; }

        public string CustomerCode { get; set; } = string.Empty;

        public string CustomerName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public int TotalOrders { get; set; }

        public decimal TotalSales { get; set; }

        public decimal AverageOrderValue { get; set; }

        public DateTime? LastPurchaseDate { get; set; }
    }
}
namespace Inventory.API.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalCategories { get; set; }

        public int TotalProducts { get; set; }

        public int TotalSuppliers { get; set; }

        public int TotalCustomers { get; set; }

        public int TotalPurchases { get; set; }

        public int TotalSales { get; set; }

        public int LowStockProducts { get; set; }

        public int OutOfStockProducts { get; set; }

        public int CurrentStockQuantity { get; set; }

        public decimal CurrentStockValue { get; set; }

        public decimal TodaySales { get; set; }

        public decimal TodayPurchases { get; set; }

        public decimal MonthlySales { get; set; }

        public decimal MonthlyPurchases { get; set; }

        public decimal TotalSalesAmount { get; set; }

        public decimal TotalPurchaseAmount { get; set; }
    }
}
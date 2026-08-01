using System.ComponentModel.DataAnnotations;

namespace Inventory.API.DTOs.Dashboard
{
    public class DashboardDto
    {
        public int TotalCategories { get; set; }

        public int TotalProducts { get; set; }

        public int TotalSuppliers { get; set; }

        public int TotalCustomers { get; set; }

        public int LowStockProducts { get; set; }

        public int OutOfStockProducts { get; set; }

        public decimal TodaySales { get; set; }
        public decimal TodayPurchases { get; set; }
        [DisplayFormat(DataFormatString ="{0:F2}")]
        public decimal MonthlySales { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public decimal MonthlyPurchases { get; set; }
    }
}
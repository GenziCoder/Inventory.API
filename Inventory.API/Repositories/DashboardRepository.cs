using Inventory.API.Data;
using Inventory.API.DTOs.Dashboard;
using Inventory.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Repositories
{
    public class DashboardRepository : IDashboardRepository 
    {
        private readonly ApplicationDbContext _context;
        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            var today = DateTime.Today;

            var month = today.Month;

            var year = today.Year;

            return new DashboardSummaryDto
            {
                TotalCategories =
                    await _context.Categories.CountAsync(),

                TotalProducts =
                    await _context.Products.CountAsync(),

                TotalSuppliers =
                    await _context.Suppliers.CountAsync(),

                TotalCustomers =
                    await _context.Customers.CountAsync(),

                TotalPurchases =
                    await _context.Purchases.CountAsync(),

                TotalSales =
                    await _context.Sales.CountAsync(),

                LowStockProducts =
                    await _context.Products.CountAsync(x =>
                        x.StockQuantity <= x.MinimumStock &&
                        x.StockQuantity > 0),

                OutOfStockProducts =
                    await _context.Products.CountAsync(x =>
                        x.StockQuantity == 0),

                CurrentStockQuantity =
                    await _context.Products.SumAsync(x =>
                        x.StockQuantity),

                CurrentStockValue =
                    await _context.Products.SumAsync(x =>
                        x.StockQuantity * x.PurchasePrice),

                TodaySales =
                    await _context.Sales
                        .Where(x => x.SaleDate.Date == today)
                        .SumAsync(x => (decimal?)x.TotalAmount) ?? 0,

                TodayPurchases =
                    await _context.Purchases
                        .Where(x => x.PurchaseDate.Date == today)
                        .SumAsync(x => (decimal?)x.TotalAmount) ?? 0,

                MonthlySales =
                    await _context.Sales
                        .Where(x =>
                            x.SaleDate.Month == month &&
                            x.SaleDate.Year == year)
                        .SumAsync(x => (decimal?)x.TotalAmount) ?? 0,

                MonthlyPurchases =
                    await _context.Purchases
                        .Where(x =>
                            x.PurchaseDate.Month == month &&
                            x.PurchaseDate.Year == year)
                        .SumAsync(x => (decimal?)x.TotalAmount) ?? 0,

                TotalSalesAmount =
                    await _context.Sales
                        .SumAsync(x => (decimal?)x.TotalAmount) ?? 0,

                TotalPurchaseAmount =
                    await _context.Purchases
                        .SumAsync(x => (decimal?)x.TotalAmount) ?? 0
            };
        }

    }
}

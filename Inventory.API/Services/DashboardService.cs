using Inventory.API.Data;
using Inventory.API.DTOs.Dashboard;
using Inventory.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetDashboardAsync()
        {
            var today = DateTime.Today;

            var firstDayOfMonth = new DateTime(
                today.Year,
                today.Month,
                1);

            return new DashboardDto
            {
                TotalCategories = await _context.Categories.CountAsync(),

                TotalProducts = await _context.Products.CountAsync(),

                TotalSuppliers = await _context.Suppliers.CountAsync(),

                TotalCustomers = await _context.Customers.CountAsync(),

                LowStockProducts = await _context.Products
                    .CountAsync(x => x.StockQuantity <= x.MinimumStock
                                  && x.StockQuantity > 0),

                OutOfStockProducts = await _context.Products
                    .CountAsync(x => x.StockQuantity == 0),

                TodaySales = await _context.Sales
                    .Where(x => x.SaleDate.Date == today)
                    .SumAsync(x => (decimal?)x.TotalAmount) ?? 0,

                TodayPurchases = await _context.Purchases
                    .Where(x => x.PurchaseDate.Date == today)
                    .SumAsync(x => (decimal?)x.TotalAmount) ?? 0,

                MonthlySales = await _context.Sales
                    .Where(x => x.SaleDate >= firstDayOfMonth)
                    .SumAsync(x => (decimal?)x.TotalAmount) ?? 0,

                MonthlyPurchases = await _context.Purchases
                    .Where(x => x.PurchaseDate >= firstDayOfMonth)
                    .SumAsync(x => (decimal?)x.TotalAmount) ?? 0
            };
        }
    }
}
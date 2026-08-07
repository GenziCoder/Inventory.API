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

        public async Task<RecentTransactionDto> GetRecentTransactionsAsync(int count)
        {
            var purchases = await _context.Purchases
                          .Include(x => x.Supplier)
                          .OrderByDescending(x => x.PurchaseDate)
                          .Take(count)
                          .Select(x => new TransactionDto
                          {
                              Id = x.Id,
                              Number = x.PurchaseNumber,
                              Name = x.Supplier.CompanyName,
                              Date = x.PurchaseDate,
                              Amount = x.TotalAmount
                          })
                          .ToListAsync();

            var sales = await _context.Sales
                    .Include(x => x.Customer)
                    .OrderByDescending(x => x.SaleDate)
                    .Take(count)
                    .Select(x => new TransactionDto
                    {
                        Id = x.Id,
                        Number = x.InvoiceNumber,
                        Name = x.Customer.FirstName + " " + x.Customer.LastName,
                        Date = x.SaleDate,
                        Amount = x.TotalAmount
                    })
                    .ToListAsync();

            return new RecentTransactionDto
            {
                RecentPurchases = purchases,
                RecentSales = sales

            };
        }

        public async Task<List<MonthlyChartDto>> GetMonthlyChartAsync()
        {
            var year = DateTime.Today.Year;

            var purchases = await _context.Purchases
                .Where(x => x.PurchaseDate.Year == year)
                .GroupBy(x => x.PurchaseDate.Month)
                .Select(x => new
                {
                    Month = x.Key,
                    Amount = x.Sum(y => y.TotalAmount)
                })
                .ToListAsync();

            var sales = await _context.Sales
                .Where(x => x.SaleDate.Year == year)
                .GroupBy(x => x.SaleDate.Month)
                .Select(x => new
                {
                    Month = x.Key,
                    Amount = x.Sum(y => y.TotalAmount)
                })
                .ToListAsync();

            var result = new List<MonthlyChartDto>();

            for (int month = 1; month <= 12; month++)
            {
                result.Add(new MonthlyChartDto
                {
                    Month = new DateTime(year, month, 1).ToString("MMM"),

                    Purchase = purchases
                        .FirstOrDefault(x => x.Month == month)?.Amount ?? 0,

                    Sale = sales
                        .FirstOrDefault(x => x.Month == month)?.Amount ?? 0
                });
            }

            return result;
        }

        public async Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(int count = 10)
        {
            return await _context.SaleDetails
                .Include(x => x.Product)
                .GroupBy(x => new
                {
                    x.ProductId,
                    x.Product.Name
                })
                .Select(x => new TopSellingProductDto
                {
                    ProductId = x.Key.ProductId,

                    ProductName = x.Key.Name,

                    QuantitySold = x.Sum(y => y.Quantity),

                    SalesAmount = x.Sum(y => y.TotalPrice)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<TopPurchasedProductDto>> GetTopPurchasedProductsAsync(int count = 10)
        {
            return await _context.PurchaseDetails
                .Include(x => x.Product)
                .GroupBy(x => new
                {
                    x.ProductId,
                    x.Product.Name
                })
                .Select(x => new TopPurchasedProductDto
                {
                    ProductId = x.Key.ProductId,

                    ProductName = x.Key.Name,

                    QuantityPurchased = x.Sum(y => y.Quantity),

                    PurchaseAmount = x.Sum(y => y.TotalPrice)
                })
                .OrderByDescending(x => x.QuantityPurchased)
                .Take(count)
                .ToListAsync();
        }


    }
}

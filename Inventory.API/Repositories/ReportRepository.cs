using Inventory.API.Common;
using Inventory.API.Data;
using Inventory.API.DTOs.Report;
using Inventory.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<SalesReportDto>> GetSalesReportAsync(SalesReportFilterDto filter)
        {
            var query = _context.Sales
                .AsNoTracking()
                .Include(x => x.Customer)
                .AsQueryable();

            if (filter.FromDate.HasValue)
            {
                query = query.Where(x =>
                    x.SaleDate.Date >= filter.FromDate.Value.Date);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(x =>
                    x.SaleDate.Date <= filter.ToDate.Value.Date);
            }

            if (filter.CustomerId.HasValue)
            {
                query = query.Where(x =>
                    x.CustomerId == filter.CustomerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.InvoiceNumber))
            {
                query = query.Where(x =>
                    x.InvoiceNumber.Contains(filter.InvoiceNumber));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.SaleDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new SalesReportDto
                {
                    Id = x.Id,
                    InvoiceNumber = x.InvoiceNumber,
                    CustomerName = x.Customer.FirstName + " " + x.Customer.LastName,
                    SaleDate = x.SaleDate,
                    TotalAmount = x.TotalAmount,
                    Remarks = x.Remarks
                })
                .ToListAsync();

            return new PagedResponse<SalesReportDto>(
                data,
                totalRecords,
                filter.PageNumber,
                filter.PageSize);
        }

        public async Task<PagedResponse<PurchaseReportDto>> GetPurchaseReportAsync(PurchaseReportFilterDto filter)
        {
            var query = _context.Purchases
                .AsNoTracking()
                .Include(x => x.Supplier)
                .AsQueryable();

            if (filter.FromDate.HasValue)
            {
                query = query.Where(x =>
                    x.PurchaseDate.Date >= filter.FromDate.Value.Date);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(x =>
                    x.PurchaseDate.Date <= filter.ToDate.Value.Date);
            }

            if (filter.SupplierId.HasValue)
            {
                query = query.Where(x =>
                    x.SupplierId == filter.SupplierId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.PurchaseNumber))
            {
                query = query.Where(x =>
                    x.PurchaseNumber.Contains(filter.PurchaseNumber));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.PurchaseDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new PurchaseReportDto
                {
                    Id = x.Id,
                    PurchaseNumber = x.PurchaseNumber,
                    SupplierName = x.Supplier.CompanyName,
                    PurchaseDate = x.PurchaseDate,
                    TotalAmount = x.TotalAmount,
                    Remarks = x.Remarks
                })
                .ToListAsync();

            return new PagedResponse<PurchaseReportDto>(
                data,
                totalRecords,
                filter.PageNumber,
                filter.PageSize);
            }

        public async Task<PagedResponse<StockReportDto>> GetStockReportAsync(StockReportFilterDto filter)
        {
            var query = _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(x =>
                    x.Name.Contains(filter.Search) ||
                    x.ProductCode.Contains(filter.Search));
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(x =>
                    x.CategoryId == filter.CategoryId.Value);
            }

            if (filter.LowStockOnly)
            {
                query = query.Where(x =>
                    x.StockQuantity <= x.MinimumStock &&
                    x.StockQuantity > 0);
            }

            if (filter.OutOfStockOnly)
            {
                query = query.Where(x =>
                    x.StockQuantity == 0);
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.Name)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new StockReportDto
                {
                    ProductId = x.Id,
                    ProductCode = x.ProductCode,
                    ProductName = x.Name,
                    CategoryName = x.Category.Name,
                    PurchasePrice = x.PurchasePrice,
                    SellingPrice = x.SellingPrice,
                    StockQuantity = x.StockQuantity,
                    MinimumStock = x.MinimumStock,
                    StockValue = x.StockQuantity * x.PurchasePrice,
                    IsLowStock = x.StockQuantity <= x.MinimumStock && x.StockQuantity > 0,
                    IsOutOfStock = x.StockQuantity == 0
                })
                .ToListAsync();

            return new PagedResponse<StockReportDto>(
                data,
                totalRecords,
                filter.PageNumber,
                filter.PageSize);
        }

        public async Task<List<StockLedgerDto>> GetStockLedgerAsync(int productId)
        {
            // load all purchases for the product
            var purchases = await _context.PurchaseDetails
                        .Include(x => x.Purchase)
                        .Where(x => x.ProductId == productId)
                        .Select(x => new
                        {
                            Date = x.Purchase.PurchaseDate,
                            Type = "Purchase",
                            Number = x.Purchase.PurchaseNumber,
                            In = x.Quantity,
                            Out = 0
                        })
                        .ToListAsync();

            // load all sales for the product
            var sales = await _context.SaleDetails
                        .Include(x => x.Sale)
                        .Where(x => x.ProductId == productId)
                        .Select(x => new
                        {
                            Date = x.Sale.SaleDate,
                            Type = "Sale",
                            Number = x.Sale.InvoiceNumber,
                            In = 0,
                            Out = x.Quantity
                        })
                        .ToListAsync();
            // merged purchases and sales into a single list and order by date
            var transactions = purchases
                        .Concat(sales)
                        .OrderBy(x => x.Date)
                        .ToList();

            // calculate the running balance and create the ledger
            var balance = 0;

            var ledger = new List<StockLedgerDto>();

            foreach (var transaction in transactions)
            {
                balance += transaction.In;
                balance -= transaction.Out;

                ledger.Add(new StockLedgerDto
                {
                    Date = transaction.Date,
                    TransactionType = transaction.Type,
                    ReferenceNumber = transaction.Number,
                    StockIn = transaction.In,
                    StockOut = transaction.Out,
                    Balance = balance
                });
            }

            return ledger;

        }

        public async Task<PagedResponse<ProfitReportDto>> GetProfitReportAsync(ProfitReportFilterDto filter)
        {
            var query = _context.Sales
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.SaleDetails)
                    .ThenInclude(x => x.Product)
                .AsQueryable();

            if (filter.FromDate.HasValue)
                query = query.Where(x =>
                    x.SaleDate >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(x =>
                    x.SaleDate <= filter.ToDate.Value);

            if (filter.CustomerId.HasValue)
                query = query.Where(x =>
                    x.CustomerId == filter.CustomerId);

            if (filter.ProductId.HasValue)
                query = query.Where(x =>
                    x.SaleDetails.Any(d =>
                        d.ProductId == filter.ProductId));

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.SaleDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new ProfitReportDto
                {
                    SaleId = x.Id,

                    InvoiceNumber = x.InvoiceNumber,

                    SaleDate = x.SaleDate,

                    CustomerName = x.Customer.FirstName + " " + x.Customer.LastName,

                    SalesAmount = x.TotalAmount,

                    PurchaseCost = x.SaleDetails.Sum(d =>
                        d.Quantity * d.Product.PurchasePrice),

                    GrossProfit =
                        x.TotalAmount -
                        x.SaleDetails.Sum(d =>
                            d.Quantity * d.Product.PurchasePrice),

                    ProfitPercentage =
                        x.TotalAmount == 0
                            ? 0
                            : ((x.TotalAmount -
                                x.SaleDetails.Sum(d =>
                                    d.Quantity * d.Product.PurchasePrice))
                               / x.TotalAmount) * 100
                })
                .ToListAsync();

            return new PagedResponse<ProfitReportDto>(
                data,
                totalRecords,
                filter.PageNumber,
                filter.PageSize);
        }

        public async Task<PagedResponse<CustomerSalesReportDto>> GetCustomerSalesReportAsync(CustomerSalesReportFilterDto filter)
        {
            var query = _context.Customers
                .AsNoTracking()
                .Include(x => x.Sales)
                .AsQueryable();

            if (filter.CustomerId.HasValue)
            {
                query = query.Where(x =>
                    x.Id == filter.CustomerId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(x =>
                    x.FirstName.Contains(filter.Search) ||
                    x.LastName.Contains(filter.Search) ||
                    x.CustomerCode.Contains(filter.Search));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.FirstName)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new CustomerSalesReportDto
                {
                    CustomerId = x.Id,

                    CustomerCode = x.CustomerCode,

                    CustomerName = x.FirstName + " " + x.LastName,

                    Phone = x.Phone,

                    TotalOrders = x.Sales.Count,

                    TotalSales = x.Sales.Sum(s => (decimal?)s.TotalAmount) ?? 0,

                    AverageOrderValue =
                        x.Sales.Any()
                            ? x.Sales.Average(s => s.TotalAmount)
                            : 0,

                    LastPurchaseDate =
                        x.Sales
                            .OrderByDescending(s => s.SaleDate)
                            .Select(s => (DateTime?)s.SaleDate)
                            .FirstOrDefault()
                })
                .ToListAsync();

            return new PagedResponse<CustomerSalesReportDto>(
                data,
                totalRecords,
                filter.PageNumber,
                filter.PageSize);
        }

        public async Task<PagedResponse<SupplierPurchaseReportDto>> GetSupplierPurchaseReportAsync(SupplierPurchaseReportFilterDto filter)
        {
            var query = _context.Suppliers
                .AsNoTracking()
                .Include(x => x.Purchases)
                .AsQueryable();

            if (filter.SupplierId.HasValue)
            {
                query = query.Where(x => x.Id == filter.SupplierId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(x =>
                    x.CompanyName.Contains(filter.Search) ||
                    x.SupplierCode.Contains(filter.Search) ||
                    x.ContactPerson.Contains(filter.Search));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.CompanyName)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new SupplierPurchaseReportDto
                {
                    SupplierId = x.Id,

                    SupplierCode = x.SupplierCode,

                    SupplierName = x.CompanyName,

                    ContactPerson = x.ContactPerson,

                    Phone = x.Phone,

                    TotalPurchases = x.Purchases.Count,

                    TotalPurchaseAmount =
                        x.Purchases.Sum(p => (decimal?)p.TotalAmount) ?? 0,

                    AveragePurchaseAmount =
                        x.Purchases.Any()
                            ? x.Purchases.Average(p => p.TotalAmount)
                            : 0,

                    LastPurchaseDate =
                        x.Purchases
                            .OrderByDescending(p => p.PurchaseDate)
                            .Select(p => (DateTime?)p.PurchaseDate)
                            .FirstOrDefault()
                })
                .ToListAsync();

            return new PagedResponse<SupplierPurchaseReportDto>(
                data,
                totalRecords,
                filter.PageNumber,
                filter.PageSize);
        }
    }
}

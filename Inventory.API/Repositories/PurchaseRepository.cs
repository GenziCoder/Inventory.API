using Inventory.API.Common;
using Inventory.API.Data;
using Inventory.API.Entities;
using Inventory.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<Purchase>> GetAllAsync(
            string? search,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Purchases
                .Include(x => x.Supplier)
                .Include(x=>x.PurchaseDetails)
                .ThenInclude(pd=>pd.Product)
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.PurchaseNumber.Contains(search) ||
                    x.Supplier != null && x.Supplier.CompanyName.Contains(search));
            }

            var totalRecords = await query.CountAsync();

            var purchases = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<Purchase>(purchases, totalRecords, pageNumber, pageSize);
           
        }

        public async Task<Purchase?> GetByIdAsync(int id)
        {
            return await _context.Purchases
                .Include(x => x.Supplier)
                .Include(x => x.PurchaseDetails)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    !x.IsDeleted);
        }

        public async Task AddPurchaseAsync(Purchase purchase)
        {
            await _context.Purchases.AddAsync(purchase);
        }

        public async Task<Product?> GetProductAsync(int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(x =>
                    x.Id == productId &&
                    !x.IsDeleted);
        }

        public async Task<Supplier?> GetSupplierAsync(int supplierId)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(x =>
                    x.Id == supplierId &&
                    !x.IsDeleted);
        }

        public Task UpdatePurchaseAsync(Purchase purchase)
        {
            _context.Purchases.Update(purchase);

            return Task.CompletedTask;
        }

        public Task DeletePurchaseAsync(Purchase purchase)
        {
            purchase.IsDeleted = true;

            _context.Purchases.Update(purchase);

            return Task.CompletedTask;
        }
        public Task RemovePurchaseDetailsAsync(IEnumerable<PurchaseDetail> details)
        {
            _context.PurchaseDetails.RemoveRange(details);
            return Task.CompletedTask;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
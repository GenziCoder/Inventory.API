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

        public async Task AddPurchaseAsync(Purchase purchase)
        {
            await _context.Purchases.AddAsync(purchase);
        }

        public async Task<Product?> GetProductAsync(int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<Supplier?> GetSupplierAsync(int supplierId)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(x => x.Id == supplierId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
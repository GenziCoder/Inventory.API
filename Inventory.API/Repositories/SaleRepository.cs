using Inventory.API.Data;
using Inventory.API.Entities;
using Inventory.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductAsync(int productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(x => x.Id == productId);
        }

        public async Task AddSaleAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
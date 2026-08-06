using Inventory.API.Common;
using Inventory.API.Data;
using Inventory.API.Entities;
using Inventory.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Inventory.API.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<Sale>> GetAllAsync(string? search, int pageNumber, int pageSize)
        {
            IQueryable<Sale> query = _context.Sales
            .Include(x => x.Customer)
            .Include(x => x.SaleDetails)
                .ThenInclude(x => x.Product);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.InvoiceNumber.Contains(search) ||
                    x.Customer.FirstName.Contains(search));
            }
            var totalRecords = await query.CountAsync();

            var data = await query
           .OrderByDescending(x => x.Id)
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();

            return new PagedResponse<Sale>(data, pageNumber, pageSize, totalRecords);

        }

        public async Task<Sale?> GetByIdAsync(int id)
        {
            return await _context.Sales
                .Include(x => x.Customer)
                .Include(x => x.SaleDetails)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Sales
                .AnyAsync(x => x.Id == id);
        }

        public async Task AddAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
        }

        public Task UpdateAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Sale sale)
        {
            _context.Sales.Remove(sale);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RemoveSaleDetailsAsync(IEnumerable<SaleDetail> saleDetails)
        {
            _context.SaleDetails.RemoveRange(saleDetails);
            await Task.CompletedTask;
        }

    }
}
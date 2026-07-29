using Inventory.API.Data;
using Inventory.API.Entities;
using Inventory.API.Helpers;
using Inventory.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync(QueryParameters query)
        {
            IQueryable<Supplier> suppliers = _context.Suppliers;

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                suppliers = suppliers.Where(x =>
                    x.CompanyName.Contains(query.Search) ||
                    x.SupplierCode.Contains(query.Search));
            }

            suppliers = query.SortBy.ToLower() switch
            {
                "companyname" => query.Descending
                    ? suppliers.OrderByDescending(x => x.CompanyName)
                    : suppliers.OrderBy(x => x.CompanyName),

                "city" => query.Descending
                    ? suppliers.OrderByDescending(x => x.City)
                    : suppliers.OrderBy(x => x.City),

                _ => query.Descending
                    ? suppliers.OrderByDescending(x => x.Id)
                    : suppliers.OrderBy(x => x.Id)
            };

            suppliers = suppliers
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize);

            return await suppliers.ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<Supplier?> GetBySupplierCodeAsync(string supplierCode)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(x => x.SupplierCode == supplierCode);
        }

        public async Task AddAsync(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
        }

        public void Update(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
        }

        public void Delete(Supplier supplier)
        {
            _context.Suppliers.Remove(supplier);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
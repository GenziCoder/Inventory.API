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

        public async Task<IEnumerable<Supplier>> GetAllAsync(string? search, int pageNumber, int pageSize)
        {
            IQueryable<Supplier> query = _context.Suppliers.Where(x=>x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.CompanyName.Contains(search) ||
                    x.SupplierCode.Contains(search));
            }

            //suppliers = query.SortBy.ToLower() switch
            //{
            //    "companyname" => query.Descending
            //        ? suppliers.OrderByDescending(x => x.CompanyName)
            //        : suppliers.OrderBy(x => x.CompanyName),

            //    "city" => query.Descending
            //        ? suppliers.OrderByDescending(x => x.City)
            //        : suppliers.OrderBy(x => x.City),

            //    _ => query.Descending
            //        ? suppliers.OrderByDescending(x => x.Id)
            //        : suppliers.OrderBy(x => x.Id)
            //};

            //suppliers = suppliers
            //    .Skip((query.PageNumber - 1) * query.PageSize)
            //    .Take(query.PageSize);

                return await query
                .Where(x => x.IsActive)
                .OrderBy(x => x.CompanyName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(string? search)
        {
            IQueryable<Supplier> query = _context.Suppliers.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.CompanyName.Contains(search) ||
                    x.SupplierCode.Contains(search));
            }

            return await query.CountAsync();
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
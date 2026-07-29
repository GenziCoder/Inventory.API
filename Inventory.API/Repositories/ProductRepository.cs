using Inventory.API.Common;
using Inventory.API.Data;
using Inventory.API.Entities;
using Inventory.API.Helpers;
using Inventory.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Product>> GetAllAsync(QueryParameters query)
        {
            IQueryable<Product> products = _context.Products
                .Include(p => p.Category);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                products = products.Where(x =>
                    x.Name.Contains(query.Search) ||
                    x.ProductCode.Contains(query.Search));
            }

            var totalRecords = await products.CountAsync();

            products = query.SortBy.ToLower() switch
            {
                "price" => query.Descending
                    ? products.OrderByDescending(x => x.SellingPrice)
                    : products.OrderBy(x => x.SellingPrice),

                "stock" => query.Descending
                    ? products.OrderByDescending(x => x.StockQuantity)
                    : products.OrderBy(x => x.StockQuantity),

                _ => query.Descending
                    ? products.OrderByDescending(x => x.Name)
                    : products.OrderBy(x => x.Name)
            };

            var items = await products
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Product>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / query.PageSize)
            };
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product?> GetByProductCodeAsync(string productCode)
        {
            return await _context.Products
                .FirstOrDefaultAsync(x => x.ProductCode == productCode);
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
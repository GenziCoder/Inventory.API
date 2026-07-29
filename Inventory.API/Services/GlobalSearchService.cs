using Inventory.API.Data;
using Inventory.API.DTOs.Search;
using Inventory.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Services
{
    public class GlobalSearchService : IGlobalSearchService
    {
        private readonly ApplicationDbContext _context;

        public GlobalSearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GlobalSearchResponseDto> SearchAsync(string keyword)
        {
            keyword = keyword.Trim();

            var response = new GlobalSearchResponseDto();

            // Products
            var products = await _context.Products
                .Where(x =>
                    x.Name.Contains(keyword) ||
                    x.ProductCode.Contains(keyword))
                .Select(x => new SearchResultDto
                {
                    Module = "Product",
                    Id = x.Id,
                    Title = x.Name,
                    Description = x.ProductCode
                })
                .ToListAsync();

            // Categories
            var categories = await _context.Categories
                .Where(x => x.Name.Contains(keyword))
                .Select(x => new SearchResultDto
                {
                    Module = "Category",
                    Id = x.Id,
                    Title = x.Name,
                    Description = x.Description
                })
                .ToListAsync();

            // Customers
            var customers = await _context.Customers
                .Where(x =>
                    x.FirstName.Contains(keyword) ||
                    x.LastName.Contains(keyword) ||
                    x.CustomerCode.Contains(keyword))
                .Select(x => new SearchResultDto
                {
                    Module = "Customer",
                    Id = x.Id,
                    Title = x.FirstName + " " + x.LastName,
                    Description = x.CustomerCode
                })
                .ToListAsync();

            // Suppliers
            var suppliers = await _context.Suppliers
                .Where(x =>
                    x.CompanyName.Contains(keyword) ||
                    x.SupplierCode.Contains(keyword))
                .Select(x => new SearchResultDto
                {
                    Module = "Supplier",
                    Id = x.Id,
                    Title = x.CompanyName,
                    Description = x.SupplierCode
                })
                .ToListAsync();

            response.Results.AddRange(products);
            response.Results.AddRange(categories);
            response.Results.AddRange(customers);
            response.Results.AddRange(suppliers);

            return response;
        }
    }
}
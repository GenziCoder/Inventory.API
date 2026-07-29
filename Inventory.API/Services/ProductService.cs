using Inventory.API.Common;
using Inventory.API.DTOs.Product;
using Inventory.API.Entities;
using Inventory.API.Helpers;
using Inventory.API.Interfaces;

namespace Inventory.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<ProductDto>> GetAllAsync(QueryParameters query)
        {
            var result = await _repository.GetAllAsync(query);

            return new PagedResult<ProductDto>
            {
                Items = result.Items.Select(p => new ProductDto
                {
                    Id = p.Id,
                    ProductCode = p.ProductCode,
                    Name = p.Name,
                    Description = p.Description,
                    PurchasePrice = p.PurchasePrice,
                    SellingPrice = p.SellingPrice,
                    StockQuantity = p.StockQuantity,
                    MinimumStock = p.MinimumStock,
                    Barcode = p.Barcode,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    IsActive = p.IsActive
                }),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                TotalRecords = result.TotalRecords
            };
        }
        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                ProductCode = product.ProductCode,
                Name = product.Name,
                Description = product.Description,
                PurchasePrice = product.PurchasePrice,
                SellingPrice = product.SellingPrice,
                StockQuantity = product.StockQuantity,
                MinimumStock = product.MinimumStock,
                Barcode = product.Barcode,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name
            };
        }

        public async Task<bool> CreateAsync(CreateProductDto dto)
        {
            if (await _repository.GetByProductCodeAsync(dto.ProductCode) != null)
                return false;

            if (!await _repository.CategoryExistsAsync(dto.CategoryId))
                return false;

            var product = new Product
            {
                ProductCode = dto.ProductCode,
                Name = dto.Name,
                Description = dto.Description,
                PurchasePrice = dto.PurchasePrice,
                SellingPrice = dto.SellingPrice,
                StockQuantity = dto.StockQuantity,
                MinimumStock = dto.MinimumStock,
                Barcode = dto.Barcode,
                CategoryId = dto.CategoryId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return false;

            if (!await _repository.CategoryExistsAsync(dto.CategoryId))
                return false;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.PurchasePrice = dto.PurchasePrice;
            product.SellingPrice = dto.SellingPrice;
            product.StockQuantity = dto.StockQuantity;
            product.MinimumStock = dto.MinimumStock;
            product.Barcode = dto.Barcode;
            product.CategoryId = dto.CategoryId;
            product.IsActive = dto.IsActive;

            _repository.Update(product);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return false;

            _repository.Delete(product);
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
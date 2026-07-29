using Inventory.API.Common;
using Inventory.API.DTOs.Product;
using Inventory.API.Helpers;

namespace Inventory.API.Interfaces
{
    public interface IProductService
    {
        Task<PagedResult<ProductDto>> GetAllAsync(QueryParameters query);

        Task<ProductDto?> GetByIdAsync(int id);

        Task<bool> CreateAsync(CreateProductDto dto);

        Task<bool> UpdateAsync(int id, UpdateProductDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
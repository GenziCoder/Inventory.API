using Inventory.API.Common;
using Inventory.API.Entities;
using Inventory.API.Helpers;

namespace Inventory.API.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedResult<Product>> GetAllAsync(QueryParameters query);

        Task<Product?> GetByIdAsync(int id);

        Task<Product?> GetByProductCodeAsync(string productCode);

        Task AddAsync(Product product);

        void Update(Product product);

        void Delete(Product product);

        Task<bool> CategoryExistsAsync(int categoryId);

        Task SaveChangesAsync();
    }
}
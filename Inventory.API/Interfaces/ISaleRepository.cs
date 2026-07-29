using Inventory.API.Entities;

namespace Inventory.API.Interfaces
{
    public interface ISaleRepository
    {
        Task<Product?> GetProductAsync(int productId);

        Task AddSaleAsync(Sale sale);

        Task SaveChangesAsync();
    }
}
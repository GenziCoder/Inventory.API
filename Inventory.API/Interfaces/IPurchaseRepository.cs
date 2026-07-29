using Inventory.API.Entities;

namespace Inventory.API.Interfaces
{
    public interface IPurchaseRepository
    {
        Task AddPurchaseAsync(Purchase purchase);

        Task<Product?> GetProductAsync(int productId);

        Task<Supplier?> GetSupplierAsync(int supplierId);

        Task SaveChangesAsync();
    }
}
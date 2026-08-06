using Inventory.API.Common;
using Inventory.API.Entities;

namespace Inventory.API.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<PagedResponse<Purchase>> GetAllAsync(
            string? search,
            int pageNumber,
            int pageSize);

        Task<Purchase?> GetByIdAsync(int id);

        Task AddPurchaseAsync(Purchase purchase);

        Task<Product?> GetProductAsync(int productId);

        Task<Supplier?> GetSupplierAsync(int supplierId);

        Task UpdatePurchaseAsync(Purchase purchase);

        Task DeletePurchaseAsync(Purchase purchase);
        Task RemovePurchaseDetailsAsync(IEnumerable<PurchaseDetail> details);

        Task SaveChangesAsync();
    }
}
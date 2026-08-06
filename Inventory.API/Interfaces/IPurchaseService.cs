using Inventory.API.Common;
using Inventory.API.DTOs.Purchase;

namespace Inventory.API.Interfaces
{
    public interface IPurchaseService
    {
        Task<PagedResponse<PurchaseDto>> GetAllAsync(string? search, int pageNumber, int pageSize);
        Task<PurchaseDto?> GetByIdAsync(int id);
        Task<bool> CreatePurchaseAsync(CreatePurchaseDto dto);
        Task<bool> UpdatePurchaseAsync(int id, UpdatePurchaseDto dto);
        Task<bool> DeletePurchaseAsync(int id);
    }
}
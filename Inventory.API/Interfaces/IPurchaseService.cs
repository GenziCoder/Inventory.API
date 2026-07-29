using Inventory.API.DTOs.Purchase;

namespace Inventory.API.Interfaces
{
    public interface IPurchaseService
    {
        Task<bool> CreatePurchaseAsync(CreatePurchaseDto dto);
    }
}
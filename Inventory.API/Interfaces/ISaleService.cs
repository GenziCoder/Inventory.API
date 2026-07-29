using Inventory.API.DTOs.Sale;

namespace Inventory.API.Interfaces
{
    public interface ISaleService
    {
        Task<bool> CreateSaleAsync(CreateSaleDto dto);
    }
}
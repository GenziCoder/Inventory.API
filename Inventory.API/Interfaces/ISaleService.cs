using Inventory.API.Common;
using Inventory.API.DTOs.Purchase;
using Inventory.API.DTOs.Sale;

namespace Inventory.API.Interfaces
{
    public interface ISaleService
    {
        Task<PagedResponse<SaleDto>> GetAllAsync(string? search, int pageNumber, int pageSize);
        Task<SaleDto?> GetByIdAsync(int id);

        Task<bool> CreateSaleAsync(CreateSaleDto dto);
        Task<bool> UpdateSaleAsync(int id, UpdateSaleDto dto);

        Task<bool> DeleteSaleAsync(int id);




    }
}
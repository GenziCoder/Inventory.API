using Inventory.API.Common;
using Inventory.API.DTOs.Supplier;
using Inventory.API.Helpers;

namespace Inventory.API.Interfaces
{
    public interface ISupplierService
    {
        Task<PagedResponse<SupplierDto>> GetAllAsync( string? search, int pageNumber, int pageSize);
        Task<SupplierDto?> GetByIdAsync(int id);

        Task<bool> CreateAsync(CreateSupplierDto dto);

        Task<bool> UpdateAsync(int id, UpdateSupplierDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
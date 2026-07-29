using Inventory.API.DTOs.Supplier;
using Inventory.API.Helpers;

namespace Inventory.API.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllAsync(QueryParameters query);

        Task<SupplierDto?> GetByIdAsync(int id);

        Task<bool> CreateAsync(CreateSupplierDto dto);

        Task<bool> UpdateAsync(int id, UpdateSupplierDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
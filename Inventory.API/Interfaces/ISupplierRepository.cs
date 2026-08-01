using Inventory.API.Entities;
using Inventory.API.Helpers;

namespace Inventory.API.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetAllAsync(string? search, int pageNumber, int pageSize);

        Task<int> GetTotalCountAsync(string? search);
        Task<Supplier?> GetByIdAsync(int id);

        Task<Supplier?> GetBySupplierCodeAsync(string supplierCode);

        Task AddAsync(Supplier supplier);

        void Update(Supplier supplier);

        void Delete(Supplier supplier);

        Task SaveChangesAsync();
    }
}
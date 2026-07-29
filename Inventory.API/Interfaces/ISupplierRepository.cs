using Inventory.API.Entities;
using Inventory.API.Helpers;

namespace Inventory.API.Interfaces
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<Supplier>> GetAllAsync(QueryParameters query);

        Task<Supplier?> GetByIdAsync(int id);

        Task<Supplier?> GetBySupplierCodeAsync(string supplierCode);

        Task AddAsync(Supplier supplier);

        void Update(Supplier supplier);

        void Delete(Supplier supplier);

        Task SaveChangesAsync();
    }
}
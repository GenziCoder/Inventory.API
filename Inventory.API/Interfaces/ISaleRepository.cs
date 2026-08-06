using Inventory.API.Common;
using Inventory.API.Entities;

namespace Inventory.API.Interfaces
{
    public interface ISaleRepository
    {
        Task<PagedResponse<Sale>> GetAllAsync(string? search, int pageNumber, int pageSize);

        Task<Sale?> GetByIdAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task AddAsync(Sale sale);

        Task UpdateAsync(Sale sale);

        Task DeleteAsync(Sale sale);

        Task SaveChangesAsync();
        Task RemoveSaleDetailsAsync(IEnumerable<SaleDetail> saleDetails);
    }
}
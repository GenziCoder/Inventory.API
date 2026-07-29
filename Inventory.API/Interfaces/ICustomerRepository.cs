using Inventory.API.Entities;

namespace Inventory.API.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync(
            string? search,
            int pageNumber,
            int pageSize);

        Task<int> GetTotalCountAsync(string? search);

        Task<Customer?> GetByIdAsync(int id);

        Task<Customer?> GetByCodeAsync(string customerCode);

        Task AddAsync(Customer customer);

        void Update(Customer customer);

        Task SaveChangesAsync();
    }
}
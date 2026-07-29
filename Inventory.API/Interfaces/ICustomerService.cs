using Inventory.API.Common;
using Inventory.API.DTOs.Customer;

namespace Inventory.API.Interfaces
{
    public interface ICustomerService
    {
        Task<PagedResponse<CustomerDto>> GetAllAsync(
            string? search,
            int pageNumber,
            int pageSize);

        Task<CustomerDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateCustomerDto dto);

        Task UpdateAsync(int id, UpdateCustomerDto dto);

        Task DeleteAsync(int id);
    }
}
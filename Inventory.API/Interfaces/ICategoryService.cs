using Inventory.API.DTOs.Category;

namespace Inventory.API.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();

        Task<CategoryDto?> GetByIdAsync(int id);

        Task<bool> CreateAsync(CreateCategoryDto dto);

        Task<bool> UpdateAsync(int Id, UpdateCategoryDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
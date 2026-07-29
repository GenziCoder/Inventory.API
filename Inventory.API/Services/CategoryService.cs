using AutoMapper;
using Inventory.API.DTOs.Category;
using Inventory.API.Entities;
using Inventory.API.Interfaces;
using Microsoft.Extensions.Logging;

namespace Inventory.API.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository repository, IMapper mapper, ILogger<CategoryService> logger  )
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            _logger.LogInformation("Getting all categories.");
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                return null;

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> CreateAsync(CreateCategoryDto dto)
        {
            var existingCategory =
                await _repository.GetByNameAsync(dto.Name);

            if (existingCategory != null)
            {
                _logger.LogWarning("Category already exists: {CategoryName}", dto.Name);
                return false;
            }
            var category = _mapper.Map<Category>(dto);
            await _repository.AddAsync(category);

            await _repository.SaveChangesAsync();
            _logger.LogInformation("Creating category: {CategoryName}", dto.Name);
            return true;
        }

        public async Task<bool> UpdateAsync(UpdateCategoryDto dto)
        {
            var category =
                await _repository.GetByIdAsync(dto.Id);

            if (category == null)
                return false;

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.IsActive = dto.IsActive;
            category.UpdatedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(category);

            await _repository.SaveChangesAsync();
            _logger.LogInformation("Updating category Id: {CategoryId}", dto.Id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category =
                await _repository.GetByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning("Category id does not exists: {CategoryName}", id);
                return false;
            }
              

            await _repository.DeleteAsync(category);

            await _repository.SaveChangesAsync();
            _logger.LogInformation("Deleting category Id: {CategoryId}", id);
            return true;
        }
    }
}
using AutoMapper;
using Inventory.API.DTOs.Category;
using Inventory.API.Entities;

namespace Inventory.API.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // Entity -> DTO
            CreateMap<Category, CategoryDto>();

            // DTO -> Entity
            CreateMap<CreateCategoryDto, Category>();

            // DTO -> Entity
            CreateMap<UpdateCategoryDto, Category>();

            // Entity -> DTO
            CreateMap<Category, UpdateCategoryDto>();
        }
    }
}
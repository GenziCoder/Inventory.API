using Inventory.API.DTOs.Search;

namespace Inventory.API.Interfaces
{
    public interface IGlobalSearchService
    {
        Task<GlobalSearchResponseDto> SearchAsync(string keyword);
    }
}
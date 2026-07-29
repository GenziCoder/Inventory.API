namespace Inventory.API.DTOs.Search
{
    public class GlobalSearchResponseDto
    {
        public List<SearchResultDto> Results { get; set; } = new();
    }
}
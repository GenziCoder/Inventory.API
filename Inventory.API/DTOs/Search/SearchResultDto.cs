namespace Inventory.API.DTOs.Search
{
    public class SearchResultDto
    {
        public string Module { get; set; } = string.Empty;

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
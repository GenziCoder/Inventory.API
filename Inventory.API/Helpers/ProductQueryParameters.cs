namespace Inventory.API.Helpers
{
    public class ProductQueryParameters
    {
        public string? Search { get; set; }

        public int? CategoryId { get; set; }

        public string SortBy { get; set; } = "Name";

        public bool Descending { get; set; } = false;

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
namespace Inventory.API.Helpers
{
    public class QueryParameters
    {
        private const int MaxPageSize = 100;

        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public string SortBy { get; set; } = "Id";

        public bool Descending { get; set; } = false;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
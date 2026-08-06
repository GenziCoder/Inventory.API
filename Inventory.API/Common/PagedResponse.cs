namespace Inventory.API.Common
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
        public string? Message { get; set; }
        public bool? Success { get; set; }

        //public PagedResponse()
        //{

        //}
        public PagedResponse(
            IEnumerable<T> data,
            int totalRecords,
            int pageNumber,
            int pageSize)
        {
            Data = data;
            TotalRecords = totalRecords;
            PageNumber = pageNumber;
            PageSize = pageSize;

            TotalPages = (int)Math.Ceiling(
                totalRecords / (double)pageSize);
        }
    }
}
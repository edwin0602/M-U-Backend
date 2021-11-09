namespace RestBackend.Core.Resources.Pagination
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string OrderBy { get; set; }

        public bool IsAscending { get; set; } = true;

        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 500 ? 500 : pageSize;
        }
    }
}

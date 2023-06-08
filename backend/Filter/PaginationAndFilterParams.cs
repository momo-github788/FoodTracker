namespace backend.Filter {
    public class PaginationAndFilterParams
    {
        const int maxPageSize = 20;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string FoodCategory { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string SearchQuery { get; set; } = string.Empty;
        public string SortDir { get; set; } = "asc";
        public string SortBy { get; set; } = "";



        public PaginationAndFilterParams()
        {
            this.PageNumber = 1;
            this.PageSize = maxPageSize;
        }

        public PaginationAndFilterParams(int pageNumber, int pageSize, string foodCategory, string name, string sortDir, string sortBy) {
            // Don't allow page number < 1 if requested
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            // Don't allow page size > 10 if requested
            this.PageSize = pageSize > maxPageSize ? maxPageSize : pageSize;
            this.FoodCategory = foodCategory;
            this.Name = name;
            this.SortDir = sortDir;
            this.SortBy = sortBy;
        }
    }
}

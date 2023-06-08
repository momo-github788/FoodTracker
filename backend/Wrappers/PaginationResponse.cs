namespace backend.Wrappers
{
    public class PaginationResponse<T> 
    {

        public T Data { get; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
  
        
        public PaginationResponse(T data, int pageNumber, int pageSize, int totalRecords) {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.TotalRecords = totalRecords;
        }

    }
}

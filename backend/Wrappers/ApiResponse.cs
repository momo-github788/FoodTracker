namespace backend.Wrappers {
    public class ApiResponse<T> {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; } = null;
        public string Message { get; set; } = string.Empty;
    }
}

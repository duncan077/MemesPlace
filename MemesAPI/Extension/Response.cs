namespace MemesAPI.Extension
{
    public class Response<T>
    {
        public string Message { get; set; }=string.Empty;
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }=false;
        public string Error { get; set; }=string.Empty;

    }
}

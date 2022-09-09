namespace MemesPlaceWeb.Services.Base
{
    public class Response<T>
    {
        public string Message { get; set; }
        public T Result { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }

    }
}

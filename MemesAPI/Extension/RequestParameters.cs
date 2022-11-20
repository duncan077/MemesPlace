namespace MemesAPI.Extension
{
    public abstract class RequestParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value > 0)
                    _pageSize = (value > maxPageSize) ? maxPageSize : value;
                else
                    _pageSize = 1;
            }
        }
    }
    public class MemeParameters : RequestParameters
    {
public string name { get; set; } = string.Empty;
        public string tag { get; set; } = string.Empty;
        public bool popular { get; set; } = false;
    }
}

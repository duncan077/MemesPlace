namespace MemesAPI.Models.Meme
{
    public class ImageFile
    {
        public byte[] data { get; set; }
        public string format { get; set; } =string.Empty;
        public long Size { get; set; }
    }
}

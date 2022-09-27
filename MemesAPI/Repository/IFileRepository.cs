using MemesAPI.Models.Meme;

namespace MemesAPI.Repository
{
    public interface IFileRepository
    {
        public  Task<string> UploadFile(ImageFile files);
    }
}

using MemesAPI.Extension;
using MemesAPI.Models.Meme;

namespace MemesAPI.Repository.Interface
{
    public interface IFileRepository
    {
        public Task<Response<UploadResult>> UploadFile(ImageFile files);
    }
}

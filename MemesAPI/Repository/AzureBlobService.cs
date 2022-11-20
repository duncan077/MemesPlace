
using MemesAPI.Extension;
using MemesAPI.Models.Meme;
using MemesAPI.Repository.Interface;

namespace MemesAPI.Repository
{
    public class AzureBlobService : IFileRepository
    {
        public Task<Response<UploadResult>> UploadFile(ImageFile files)
        {
            throw new NotImplementedException();
        }
    }
}

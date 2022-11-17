
using MemesAPI.Extension;
using MemesAPI.Models.Meme;
using MemesAPI.Repository.Interface;

namespace MemesAPI.Repository
{
    public class AzureBlobService : IFileRepository
    {
     

     
        Task<Response<string>> IFileRepository.UploadFile(ImageFile files)
        {
            throw new NotImplementedException();
        }
    }
}

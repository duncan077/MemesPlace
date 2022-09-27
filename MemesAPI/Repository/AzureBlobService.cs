using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using MemesAPI.Models.Meme;

namespace MemesAPI.Repository
{
    public class AzureBlobService : IFileRepository
    {
     

        public Task<string> UploadFile(ImageFile files)
        {
            throw new NotImplementedException();
        }
    }
}

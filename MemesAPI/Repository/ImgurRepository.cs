using Imgur;
using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using MemesAPI.Models.Meme;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.IO;
using System.IO.Pipes;
using System.Net.Http;
using static System.Net.Mime.MediaTypeNames;

namespace MemesAPI.Repository
{
    public class ImgurRepository:IFileRepository
    {
       private readonly HttpClient httpClient = new HttpClient();
        private readonly ApiClient apiClient = new ApiClient("5705e4a50de1ff4");
        private string[] img = { "image/png", "image/jpeg", "image/gif", "image/tiff", "image/webp" };
        private string[] vid = { "video/x-msvideo", "video/mp4", "video/mpeg", "video/ogg", "video/mp2t", "video/webm", "video/3gpp" };
        ImageEndpoint imageEndpoint;
        public ImgurRepository()
        {
            imageEndpoint = new ImageEndpoint(apiClient, httpClient);
        }
        public async Task<string> UploadFile(ImageFile file)
        { 
          
           


                if (img.Contains(file.format))
                {
                    var imageUpload = await imageEndpoint.UploadImageAsync(new MemoryStream(file.data));
                return (imageUpload.Link);
                }
                if (vid.Contains(file.format))
            {
                    var imageUpload = await imageEndpoint.UploadVideoAsync(new MemoryStream(file.data));
                return (imageUpload.Link);
                }
            return "";
              

            
         
        
        }

    }
}

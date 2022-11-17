using Imgur;
using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using MemesAPI.Extension;
using MemesAPI.Models.Meme;
using MemesAPI.Repository.Interface;
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
        public async Task<Response<string>> UploadFile(ImageFile file)
        {



            try
            {
                if (img.Contains(file.format))
                {
                    var imageUpload = await imageEndpoint.UploadImageAsync(new MemoryStream(file.data));
                    return (new Response<string>() {
                    Data=imageUpload.Link,Message="Success",IsSuccess=true} );
                }
                if (vid.Contains(file.format))
                {
                    var imageUpload = await imageEndpoint.UploadVideoAsync(new MemoryStream(file.data));
                    return (new Response<string>()
                    {
                        Data = imageUpload.Link,
                        Message = "Success",
                        IsSuccess = true
                    });
                }
                return (new Response<string>() { Message = "Error Uploading" });
            }
            catch (Exception ex)
            {

                return (new Response<string>() { Message = "Error Uploading", Error = ex.Message });
            }
               
              

            
         
        
        }

    }
}

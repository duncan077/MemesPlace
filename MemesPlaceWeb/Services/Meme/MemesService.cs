using Blazored.LocalStorage;
using MemesPlaceWeb.Services.Base;

namespace MemesPlaceWeb.Services.Meme
{
    public class MemesService : BaseHttpService, IMemesService
    {
        private readonly IClient client;

        public MemesService(IClient client, ILocalStorageService localStorage) : base(client, localStorage)
        {
            this.client = client;
        }
        public async Task<Response<List<MemeDTO>>> GetMemes()
        {
            Response<List<MemeDTO>> response;
            try
            {
                await GetBearerToken();
                var data = await client.MemesAllAsync(0, "", 10);
                response = new Response<List<MemeDTO>> { 
                    Result=data.ToList(),
                    IsSuccess=true
                };
            }
            catch (ApiException ex)
            {

                response = ConvertApiExceptions<List<MemeDTO>>(ex);    
            }
            return response;
        }
        public async Task<Response<MemeDTO>> GetMeme(int id)
        {
            Response<MemeDTO> response;
            try
            {
                await GetBearerToken();
                var data = await client.MemesGETAsync(id);
                response = new Response<MemeDTO>
                {
                    Result = data,
                    IsSuccess = true
                };
            }
            catch (ApiException ex)
            {

                response = ConvertApiExceptions<MemeDTO>(ex);
            }
            return response;
        }
        public async Task<Response<MemeDTO>> AddMeme(MemeAddDTO memeAddDTO)
        {
            Response<MemeDTO> response;
            try
            {
                await GetBearerToken();
                var data = await client.UploadAsync(memeAddDTO);
                response = new Response<MemeDTO>
                {
                    Result = data,
                    IsSuccess = true
                };
            }
            catch (ApiException ex)
            {

                response = ConvertApiExceptions<MemeDTO>(ex);
            }
            return response;
        }
    }
}

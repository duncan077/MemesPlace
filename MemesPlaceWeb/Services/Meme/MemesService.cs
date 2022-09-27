using Blazored.LocalStorage;
using MemesPlaceWeb.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Security.Claims;
namespace MemesPlaceWeb.Services.Meme
{
    public class MemesService : BaseHttpService, IMemesService
    {
        private readonly IClient client;
        private readonly AuthenticationStateProvider authenticationState;
        public MemesService(IClient client, ILocalStorageService localStorage, AuthenticationStateProvider authenticationState) : base(client, localStorage)
        {
            this.client = client;
            this.authenticationState = authenticationState;


        }
        public async Task<Response<List<MemeDTO>>> GetMemes()
        {
            Response<List<MemeDTO>> response;
            try
            {


                var auth = await authenticationState.GetAuthenticationStateAsync();
                  if (auth.User.Identity.IsAuthenticated) {
                      await GetBearerToken();
                      var data = await client.AuthAllAsync(1, "", 10);
                      response = new Response<List<MemeDTO>>
                      {
                          Result = data.ToList(),
                          IsSuccess = true
                      };
                  }
                  else
                  {
                      var data = await client.MemesAllAsync(1, "", 10);
                      response = new Response<List<MemeDTO>>
                      {
                          Result = data.ToList(),
                          IsSuccess = true
                      };
                  }
               

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
                var auth = await authenticationState.GetAuthenticationStateAsync();
                if (auth.User.Identity.IsAuthenticated)
                {
                    await GetBearerToken();
                    var data = await client.AuthAsync(id);
                    response = new Response<MemeDTO>
                    {
                        Result = data,
                        IsSuccess = true
                    };
                }
                else
                {
                    var data = await client.MemesGETAsync(id);
                    response = new Response<MemeDTO>
                    {
                        Result = data,
                        IsSuccess = true
                    };
                }
            }
            catch (ApiException ex)
            {

                response = ConvertApiExceptions<MemeDTO>(ex);
            }
            return response;
        }
        public async Task<List<Response<MemeDTO>>> AddMeme(List<MemeAddDTO> memeAddDTO)
        {
            List<Response<MemeDTO>> response = new List<Response<MemeDTO>>();
            try
            {
                await GetBearerToken();
                var data = await client.UploadAsync(memeAddDTO);

                foreach (var item in data)
                {
                    response.Add(new Response<MemeDTO> { IsSuccess = true, Result = item });
                }

            }
            catch (ApiException ex)
            {

                response.Add(ConvertApiExceptions<MemeDTO>(ex));
            }
            return response;
        }
        public async Task<Response<bool>> Like(int meme)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                await GetBearerToken();
                await client.LikeAsync(meme);
                response.IsSuccess = true;
             }
            catch (ApiException ex)
            {
                response.IsSuccess = false;

            }
        return response;
        }
    
    }
}

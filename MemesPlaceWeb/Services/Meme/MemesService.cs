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
        public async Task<List<Response<MemeDTO>>> GetMemes(int page, string tag)
        {
            List<Response<MemeDTO>> response;
            try
            {


                var auth = await authenticationState.GetAuthenticationStateAsync();
                  if (auth.User.Identity.IsAuthenticated) {
                      await GetBearerToken();
                      var data = await client.AuthAllAsync(page, tag, 10);
                    response = data.ToList();
                  }
                  else
                  {
                      var data = await client.MemesAllAsync(page, tag, 10);
                    response = data.ToList();
                      
                  }
                return response;

            }
            catch (ApiException ex)
            {
                var error= new List<Response<MemeDTO>>()
                {
                new Response<MemeDTO>{ Error = ex.Message }
                };
              
                return error;
            }
           
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
                    response= data;
                }
                else
                {
                    var data = await client.MemesGETAsync(id);
                    response = data;
                }
                return response;
            }
            catch (ApiException ex)
            {

                response = ConvertApiExceptions<MemeDTO>(ex);
                return response;
            }
           
        }
        public async Task<List<Response<MemeDTO>>> AddMeme(List<MemeAddDTO> memeAddDTO)
        {
            List<Response<MemeDTO>> response = new List<Response<MemeDTO>>();
            try
            {
                await GetBearerToken();
                var data = await client.UploadAsync(memeAddDTO);

               
                
                    response=data.ToList();
                

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
               
                response = await client.LikeAsync(meme);
             }
            catch (ApiException ex)
            {
                response.Error = ex.Message;
                response.IsSuccess = false;

            }
        return response;
        }
        public async Task<Response<ProfileDTO>> GetProfile(string user)
        {
            Response<ProfileDTO> response = new Response<ProfileDTO>();
            try
            {
                

                response = await client.UserAsync(user);
            }
            catch (ApiException ex)
            {
                response.Error = ex.Message;
                response.IsSuccess = false;

            }
            return response;
        }
        public async Task<Response<bool>> ChangePassword(PasswordChange passwordChange)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                await GetBearerToken();

                response = await client.ChangepasswordAsync(passwordChange);
            }
            catch (ApiException ex)
            {
                response.Error = ex.Message;
                response.IsSuccess = false;

            }
            return response;
        }
        public async Task<Response<bool>> ChangeProfile(ProfileChange profile)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                await GetBearerToken();

                response = await client.ChangeprofileAsync(profile);
            }
            catch (ApiException ex)
            {
                response.Error = ex.Message;
                response.IsSuccess = false;

            }
            return response;
        }
    }
}

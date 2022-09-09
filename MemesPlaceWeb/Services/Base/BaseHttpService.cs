using Blazored.LocalStorage;

namespace MemesPlaceWeb.Services.Base
{
    public class BaseHttpService
    {
        private readonly IClient client;
        private readonly ILocalStorageService localStorage;

        public BaseHttpService(IClient client, ILocalStorageService localStorage)
        {
            this.client = client;
            this.localStorage = localStorage;
        }
        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException apiException)
        {
            if(apiException.StatusCode==400)
            {
                return new Response<Guid>() { IsSuccess = false, Message="Validation Errors", Error=apiException.Response };
            }
            if (apiException.StatusCode == 404)
            {
                return new Response<Guid>() { IsSuccess = false, Message = "Not Found", Error = apiException.Response };
            }
            if (apiException.StatusCode == 401)
            {
                return new Response<Guid>() { IsSuccess = false, Message = "Not Authorized", Error = apiException.Response };
            }
            if (apiException.StatusCode >=200&&apiException.StatusCode<=299)
            {
                return new Response<Guid>() { IsSuccess = true, Message = "Success" };
            }
            return new Response<Guid>() { IsSuccess = false, Message = "Something went worng", Error = apiException.Response };
        }
        protected async Task GetBearerToken()
        {
            var token = await localStorage.GetItemAsync<string>("accessToken");
            if(token!=null)
            {
                client.HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

        }
    }
}

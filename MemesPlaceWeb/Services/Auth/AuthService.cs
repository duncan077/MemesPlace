using MemesPlaceWeb.Services.Base;
using Blazored.LocalStorage;
using MemesPlaceWeb.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace MemesPlaceWeb.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IClient client;
        private readonly ILocalStorageService localStorageService;
        private readonly AuthenticationStateProvider authenticationState;

        public AuthService(IClient client, ILocalStorageService localStorageService,AuthenticationStateProvider  authenticationState )
        {
            this.client = client;
            this.localStorageService = localStorageService;
            this.authenticationState = authenticationState;
          
        }

        public async Task<bool> AuthAsync(LoginUserDTO loginUserDTO)
        {
            var response = await client.LoginAsync(loginUserDTO);
            await localStorageService.SetItemAsync("accessToken", response.Token);
            await ((ApiAuthStateProvider)authenticationState).Loggedin();
            return true;
        }
        public async Task<bool> AuthGoogleAsync(string token)
        { ExternalAuthDto externalAuthDto = new ExternalAuthDto();
            externalAuthDto.Provider = "GoogleProvider";
            externalAuthDto.IdToken = token;
            try
            {
    var response = await client.ExternalloginAsync(externalAuthDto);
                await localStorageService.SetItemAsync("accessToken", response.Token);
                await ((ApiAuthStateProvider)authenticationState).Loggedin();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task Logout()
        {
            await((ApiAuthStateProvider)authenticationState).LoggedOut();
        }
    }
}

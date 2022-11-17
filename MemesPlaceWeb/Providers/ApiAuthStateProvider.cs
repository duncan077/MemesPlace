using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace MemesPlaceWeb.Providers
{
    public class ApiAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorage;
        private readonly JwtSecurityTokenHandler jwtSecurityTokenHandler; 
   

        public ApiAuthStateProvider(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
            this.jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
           
            
            
            return new AuthenticationState(new ClaimsPrincipal(await GetClaims()));
        }
        public async Task Loggedin()
        {
           
            var user = new ClaimsPrincipal(await GetClaims());
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        private async Task<ClaimsIdentity> GetClaims()
        {
            var savedToken = await localStorage.GetItemAsync<string>("accessToken");
            if (savedToken == null)
                return new ClaimsIdentity();
            var tokenConten = jwtSecurityTokenHandler.ReadJwtToken(savedToken);
          
          if(tokenConten.ValidTo<= DateTime.UtcNow)
                return new ClaimsIdentity();
            
            var claims = new List<Claim>();
            claims= tokenConten.Claims.ToList();
            
            
          
            var claimsIdentity = new ClaimsIdentity(claims, "JWT");
          
            return claimsIdentity;
        }

        public async Task LoggedOut()
        {
           await localStorage.RemoveItemAsync("accessToken");
            var user = new ClaimsPrincipal();
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}

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
           
            List<Claim> claims = await GetClaims();
            
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity( claims,"jwt")));
        }
        public async Task Loggedin()
        {
            List<Claim> claims = await GetClaims();
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var savedToken = await localStorage.GetItemAsync<string>("accessToken");
            if (savedToken == null)
                return new List<Claim>();
            var tokenConten = jwtSecurityTokenHandler.ReadJwtToken(savedToken);
            if(tokenConten.ValidTo< DateTime.UtcNow)
                return new List<Claim>();

            var claims = tokenConten.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name,tokenConten.Subject));
            return claims;
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

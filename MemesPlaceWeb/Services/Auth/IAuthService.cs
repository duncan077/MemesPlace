using MemesPlaceWeb.Services.Base;

namespace MemesPlaceWeb.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> AuthAsync(LoginUserDTO loginUserDTO);
        Task<bool> AuthGoogleAsync(string token);
        public Task Logout();
    }
}

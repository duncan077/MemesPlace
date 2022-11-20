using MemesPlaceWeb.Services.Base;

namespace MemesPlaceWeb.Services.Meme
{
    public interface IMemesService
    {
        Task<List<Response<MemeDTO>>> GetMemes(int page, string tag, bool trend, string name);
        Task<Response<MemeDTO>> GetMeme(int id);
        Task<List<Response<MemeDTO>>> AddMeme(List<MemeAddDTO> memeAddDTO);
        Task<Response<bool>> Like(int meme);
        Task<Response<ProfileDTO>> GetProfile(string user);
        Task<Response<bool>> ChangePassword(PasswordChange passwordChange);
        Task<Response<bool>> ChangeProfile(ProfileChange profile);
    }
}
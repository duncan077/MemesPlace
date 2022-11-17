using MemesPlaceWeb.Services.Base;

namespace MemesPlaceWeb.Services.Meme
{
    public interface IMemesService
    {
        Task<List<Response<MemeDTO>>> GetMemes();
        Task<Response<MemeDTO>> GetMeme(int id);
        Task<List<Response<MemeDTO>>> AddMeme(List<MemeAddDTO> memeAddDTO);
        Task<Response<bool>> Like(int meme);
    }
}
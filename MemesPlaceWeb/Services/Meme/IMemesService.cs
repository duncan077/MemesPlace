using MemesPlaceWeb.Services.Base;

namespace MemesPlaceWeb.Services.Meme
{
    public interface IMemesService
    {
        Task<Response<List<MemeDTO>>> GetMemes();
        Task<Response<MemeDTO>> GetMeme(int id);
        Task<Response<MemeDTO>> AddMeme(MemeAddDTO memeAddDTO);
    }
}
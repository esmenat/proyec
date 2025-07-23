using RaymiMusic.Modelos;

namespace RaymiMusic.AppWeb.Services
{
    public interface IAlbumsService
    {
         Task<IEnumerable<Album>> GetAlbumsAsync();
        Task<IEnumerable<Album>> GetAlbumsSearchAsync(string? query);
    }
}

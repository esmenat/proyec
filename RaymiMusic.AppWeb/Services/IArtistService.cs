// RaymiMusic.AppWeb/Services/IArtistService.cs
using System.Threading.Tasks;
using RaymiMusic.AppWeb.Models;
using RaymiMusic.Modelos;

namespace RaymiMusic.AppWeb.Services
{
    public interface IArtistService
    {
        Task<ArtistDashboardVM?> GetDashboardAsync(string userId);
        Task<IEnumerable<Artista>> GetAllArtistasSearchAsync(string? query);
        Task<Artista> GetArtistaAsync(Guid id);
    }
}

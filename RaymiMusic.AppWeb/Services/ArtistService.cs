// RaymiMusic.AppWeb/Services/ArtistService.cs
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using RaymiMusic.AppWeb.Models;
using RaymiMusic.Modelos;

namespace RaymiMusic.AppWeb.Services
{
    public class ArtistService : IArtistService
    {
        private readonly HttpClient _http;
        public ArtistService(HttpClient http) => _http = http;

        public async Task<ArtistDashboardVM?> GetDashboardAsync(string userId)
        {
            // 1) Obtener todos los artistas
            var artistas = await _http.GetFromJsonAsync<Artista[]>("api/Artistas");
            if (artistas == null)
                return null;

            // 2) Encontrar al artista que coincide con el userId
            var artista = artistas.FirstOrDefault(a => a.Id.ToString() == userId);
            if (artista == null)
                return null;

            // 3) Obtener canciones del artista
            var allSongs = await _http.GetFromJsonAsync<Cancion[]>("api/Canciones");
            var canciones = allSongs?
                .Where(c => c.ArtistaId == artista.Id)
                .OrderByDescending(c => c.Id)
                .ToArray()
                ?? Array.Empty<Cancion>();

            // 4) Obtener álbumes del artista
            var allAlbums = await _http.GetFromJsonAsync<Album[]>("api/Albumes");
            var albumes = allAlbums?
                .Where(al => al.ArtistaId == artista.Id)
                .ToArray()
                ?? Array.Empty<Album>();

            // 5) Obtener playlists propias del artista
            var listas = await _http.GetFromJsonAsync<ListaReproduccion[]>("api/ListasReproduccion");
            var propias = listas?
                .Where(l => l.UsuarioId.ToString() == userId)
                .ToArray()
                ?? Array.Empty<ListaReproduccion>();

            // 6) Agregar cantidad de descargas por canción
            var cancionesConDescargas = new List<SongDTO>();
            foreach (var c in canciones)
            {
                int totalDescargas = 0;
                try
                {
                    totalDescargas = await _http.GetFromJsonAsync<int>($"api/Descargas/total/{c.Id}");
                }
                catch
                {
                    totalDescargas = 0; // Fallback en caso de error
                }

                cancionesConDescargas.Add(new SongDTO
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    AlbumNombre = c.Album?.Titulo ?? "—",
                    ArtistaNombre = artista.NombreArtistico,
                    Duracion = c.Duracion,
                    RutaArchivo = c.RutaArchivo,
                    TotalDescargas = totalDescargas
                });
            }

            // 7) Retornar el ViewModel completo
            return new ArtistDashboardVM
            {
                NombreArtista = artista.NombreArtistico,
                TotalCanciones = canciones.Length,
                TotalAlbumes = albumes.Length,
                TotalPlaylists = propias.Length,
                UltimasCanciones = cancionesConDescargas.Take(5),
                CancionesConDescargas = cancionesConDescargas
            };
        }
        public async Task<Artista> GetArtistaAsync(Guid id)
        {
            var artista = await _http.GetFromJsonAsync<Artista>($"api/Artistas/{id}");
            return artista ?? throw new Exception("Artista no encontrado");
        }

        public async Task<IEnumerable<Artista>> GetAllArtistasSearchAsync(string? query)
        {
            string url = string.IsNullOrWhiteSpace(query)
                ? "api/Artistas/search"
                : $"api/Artistas/search?query={Uri.EscapeDataString(query)}";

            var artistas = await _http.GetFromJsonAsync<Artista[]>(url);
            return artistas ?? Array.Empty<Artista>();
        }

    }
}

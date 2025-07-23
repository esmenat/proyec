using RaymiMusic.Modelos;

namespace RaymiMusic.AppWeb.Services
{
    public class AlbumsService : IAlbumsService
    {
        private readonly HttpClient _http;
        public AlbumsService(HttpClient http) => _http = http;
        public async Task<IEnumerable<Album>> GetAlbumsAsync()
        {
            var albums = await _http.GetFromJsonAsync<IEnumerable<Album>>("api/Albumes");
            return albums ?? throw new Exception("No se encontraron álbumes");
        }

    
        public async Task<IEnumerable<Album>> GetAlbumsSearchAsync(string? query)
        {
            var albums = await _http.GetFromJsonAsync<IEnumerable<Album>>($"api/Albumes/search?query={query}");
            return albums ?? throw new Exception("No se encontraron álbumes");
        }
    }
}

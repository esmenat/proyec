using RaymiMusic.Modelos;
using System.Net.Http.Json;

namespace RaymiMusic.AppWeb.Services
{
    public class FollowService : IFollowService
    {
        private readonly HttpClient _http;

        public FollowService(HttpClient http)
        {
            _http = http;
        }
        public async Task CreateFollowAsync(Follow follow)
        {
         
            var response = await _http.PostAsJsonAsync("api/follows", follow);
     
            if (!response.IsSuccessStatusCode)
            {
                
                throw new Exception("Error al crear el Follow");
            }
        }

        public async Task DeleteFollowAsync(int followId)
        {

            var response = await _http.DeleteAsync($"api/follows/{followId}");
            
            if (!response.IsSuccessStatusCode)
            {
                // Manejar el error según sea necesario (lanzar excepción, registrar, etc.)
                throw new Exception("Error al eliminar el Follow");
            }
        }
        public async Task<Follow> GetFollowByUserAndArtistAsync(Guid userId, Guid artistId)
        {
            // Construir la URL con los parámetros proporcionados
            var url = $"api/follows/usuario/{userId}/artista/{artistId}";
            try
            {
                // Realizar la solicitud GET para obtener el Follow
                var response = await _http.GetFromJsonAsync<Follow>(url);
                // Verificar que se obtuvo el Follow correctamente
                if (response != null)
                {
                    return response;
                }

            }
            catch
            {
                return null;
            }
           
            // Si no se encuentra el Follow, devolver null o manejar el error
            return null;
        }
    }
}



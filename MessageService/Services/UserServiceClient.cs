using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using MessageService.Models;

namespace MessageService.Services
{
    public class UserServiceClient
    {
        private readonly HttpClient _http;

        public UserServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<PublicUserDto?> GetUserByIdAsync (int id, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"api/users/find/by-id/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;    
            }

            return await response.Content.ReadFromJsonAsync<PublicUserDto>();
    
        }
    }
}

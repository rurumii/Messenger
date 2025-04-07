using System.Net.Http;
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

        public async Task<UserDto?> GetUserByIdAsync (int id)
        {
            var response = await _http.GetAsync($"api/users/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;    
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions
                {
                PropertyNameCaseInsensitive = true
                 });
                
        }
    }
}

using System.Net.Http.Json;
using MessengerClient.Models;

namespace MessengerClient.Services
{
    public class UserApiService
    {
        private readonly HttpClient _http;

        public UserApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> RegisterAsync(RegistrationDTO registration)
        {
            var response = await _http.PostAsJsonAsync("https://localhost:7202/api/users/register", registration);
            return response.IsSuccessStatusCode;
        }
        public async Task<LoginResponseDto?> LoginAsync(LoginDTO login)
        {
            var response = await _http.PostAsJsonAsync("https://localhost:7202/api/users/login", login);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            }
            return null;
        }
        public async Task<FoundUserDTO?> GetUserByQueryAsync(string query)
        {
            try
            {
                return await _http.GetFromJsonAsync<FoundUserDTO>($"https://localhost:7202/api/users/find/by-query/{query}");
            }
            catch
            {
                return null;
            }
        }
        public async Task<FoundUserDTO?> GetUserByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<FoundUserDTO>($"https://localhost:7202/api/users/find/by-id/{id}");
        }
    }
}

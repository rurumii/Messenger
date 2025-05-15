using MessengerClient.Models;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MessengerClient.Services
{
    public class MessageApiService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _jsRuntime;
        public MessageApiService(HttpClient http, IJSRuntime jSRuntime)
        {
            _http = http;
            _jsRuntime = jSRuntime;
        }

        // https://localhost:7130/

        public async Task<List<ChatDTO>> GetUserChatsAsync(int userId)
        {
            return await _http.GetFromJsonAsync<List<ChatDTO>>($"https://localhost:7130/api/chats/user/{userId}");
        }
        public async Task<bool> CreateChatAsync(CreateChatDTO create)
        {
            var response = await _http.PostAsJsonAsync("https://localhost:7130/api/chats/create", create);
            return response.IsSuccessStatusCode;
        }
        public async Task<List<MessageDTO>> GetMessagesByChatAsync(int chatId)
        {
            return await _http.GetFromJsonAsync<List<MessageDTO>>($"https://localhost:7130/api/messages/chat/{chatId}");
        }
        public async Task<bool> SendMessageAsync(SendMessageDTO message)
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            Console.WriteLine("Send TOKEN: " + token);
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7130/api/messages/send")
            {
                Content = JsonContent.Create(message)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

         public async Task<bool> DeleteMessageAsync (int id)
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            Console.WriteLine("Delete TOKEN:" + token);
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7130/api/messages/delete/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateMessageAsync (int id, UpdateMessageDto dto)
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            Console.WriteLine("Edit TOKEN:" + token);
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:7130/api/messages/update/{id}")
            {
                Content = JsonContent.Create(dto) //  передаём DTO как тело запроса
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}

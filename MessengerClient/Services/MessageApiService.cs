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

        public async Task<List<ChatDTO>> GetUserChatsAsync(int userId)
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7130/api/chats/user/{userId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<ChatDTO>>();

        }
        public async Task<bool> CreateChatAsync(CreateChatDTO create)
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, $"https://localhost:7130/api/chats/create")
            {
                Content = JsonContent.Create(create)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);
            return response.IsSuccessStatusCode;

        }
        public async Task<List<MessageDTO>> GetMessagesByChatAsync(int chatId)
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7130/api/messages/chat/{chatId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<List<MessageDTO>>();
                
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

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace MessengerClient.Services
{
    // сообщает Blazor-у авторизован ли пользователь
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;

        public CustomAuthStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        //  Blazor вызывает этот метод, чтобы спросить кто сейчас залогинен
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            //  если токена нет возвращаем анонимного пользователя (не авторизован)
            if (string.IsNullOrEmpty(token)) 
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            
            // если токен есть - создаем "личность" пользователя (не парсим токен)
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "User")
            }, "jwt");

            // оборачиваем в ClaimsPrincipal
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public void NotifyUserLogout()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }

        public void NotifyUserLogin()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}

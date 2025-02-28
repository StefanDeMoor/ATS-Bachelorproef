using ATS.Models;
using System.Net.Http.Json;
using System.Text.Json;
using Shared.DTOs;

namespace ATS.Client.Services
{
    public class ClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task Register(RegisterModel model)
        {
            var httpClient = _httpClientFactory.CreateClient("custom-httpclient");

            var requestDto = new RegisterRequestDto
            {
                Email = model.Email,
                Password = model.Password
            };

            var result = await httpClient.PostAsJsonAsync("/api/auth/register", requestDto);
            result.EnsureSuccessStatusCode(); 
        }

        public async Task Login(LoginModel model)
        {
            var httpClient = _httpClientFactory.CreateClient("custom-httpclient");

            var requestDto = new LoginRequestDto
            {
                Email = model.Email,
                Password = model.Password
            };

            var result = await httpClient.PostAsJsonAsync("/api/auth/login", requestDto);
            var response = await result.Content.ReadFromJsonAsync<LoginResponseDto>();

            if (response is not null)
            {
                var serializeResponse = JsonSerializer.Serialize(response);
                await SecureStorage.Default.SetAsync("Authentication", serializeResponse);
            }
        }
    }
}

using ATS.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace ATS.Services
{

    public interface IAuthService
    {
        Task<bool> isUserAuthenticated();
    }
    public class AuthService : IAuthService
    {
        public readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> isUserAuthenticated()
        {
            var serializeData = await SecureStorage.Default.GetAsync(AppConstants.AuthStorageKeyName);
            return !string.IsNullOrWhiteSpace(serializeData);

        }

        public async Task<string?> LoginAsync(LoginRequestDto dto)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.PostAsJsonAsync<LoginRequestDto>("api/auth/login", dto);

            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ApiResponse<AuthResponseDto> authResponse =
                    JsonSerializer.Deserialize<ApiResponse<AuthResponseDto>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                if(authResponse.Status)
                {
                    var serializedData = JsonSerializer.Serialize(authResponse.Data);
                    await SecureStorage.Default.SetAsync(AppConstants.AuthStorageKeyName, serializedData);
                } 
                else
                {
                    return authResponse.Errors.FirstOrDefault();
                }
            }
            else
            {
                return "Error is logging in";
            }
            return null;
        }

        public void Logout() => SecureStorage.Default.Remove(AppConstants.AuthStorageKeyName);

    }
}

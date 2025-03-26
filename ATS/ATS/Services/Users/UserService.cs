using ATS.Models;
using System.Text.Json;

namespace ATS.Services
{
    public class UserService
    {
        private const string AuthenticationKey = "Authentication";
        private LoginResponse? _currentUser;
        public async Task<LoginResponse?> GetCurrentUserAsync()
        {
            if (_currentUser != null) return _currentUser;

            var serializedLoginResponseInStorage = await SecureStorage.Default.GetAsync(AuthenticationKey);
            if (serializedLoginResponseInStorage != null)
            {
                _currentUser = JsonSerializer.Deserialize<LoginResponse>(serializedLoginResponseInStorage);
            }

            return _currentUser;
        }

        public async Task SetCurrentUserAsync(LoginResponse loginResponse)
        {
            _currentUser = loginResponse;
            var serializedLoginResponse = JsonSerializer.Serialize(loginResponse);
            await SecureStorage.Default.SetAsync(AuthenticationKey, serializedLoginResponse);
        }

        public async Task<string> GetUserRoleAsync()
        {
            var user = await GetCurrentUserAsync();
            return user?.Role ?? string.Empty;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var user = await GetCurrentUserAsync();
            return user != null;
        }

        public void Logout()
        {
            SecureStorage.Default.Remove(AuthenticationKey);
            _currentUser = null;
        }
    }
}

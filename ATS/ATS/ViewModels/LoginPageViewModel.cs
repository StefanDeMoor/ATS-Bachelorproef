using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ATS.Models;
using System.Text.Json;
using ATS.Views;
using ATS.Client.Services;
namespace ATS.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private RegisterModel registerModel;

        [ObservableProperty]
        private LoginModel loginModel;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private bool _isErrorVisible;

        [ObservableProperty]
        private bool isAuthenticated;

        private readonly ClientService clientService;

        public LoginPageViewModel (ClientService clientService)
        {
            this.clientService = clientService;
            RegisterModel = new();
            LoginModel = new();
            IsAuthenticated = false;
            //IsErrorVisible = false;
            //GetUserNameFromSecuredStorage();
        }

        [RelayCommand]
        private async Task Register()
        {
            await clientService.Register(RegisterModel);
        }

        [RelayCommand]
        private async Task Login()
        {
            bool loginSuccess = await clientService.Login(LoginModel);

            if (loginSuccess)
            {
                await GetUserNameFromSecuredStorage();

                if (IsAuthenticated && UserName != "Guest" && !string.IsNullOrEmpty(UserName))
                {
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
                else
                {
                    // Handle invalid login (e.g., show error message)
                   // IsErrorVisible = true;
                    //_errorMessage = "Login failed. Please try again.";
                }
            }
            else
            {
                // Show error message if login fails
                //IsErrorVisible = true;
                //_errorMessage = "Invalid credentials.";
            }
        }

        [RelayCommand]
        private async Task Logout()
        {
            SecureStorage.Default.Remove("Authentication");
            IsAuthenticated = false;
            UserName = "Guest";
            await Shell.Current.GoToAsync("..");
        }


        private async Task GetUserNameFromSecuredStorage()
        {
            UserName = "Guest";
            IsAuthenticated = false;

            var serializedLoginResponseInStorage = await SecureStorage.Default.GetAsync("Authentication");

            if (serializedLoginResponseInStorage != null)
            {
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(serializedLoginResponseInStorage);

                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.UserName))
                {
                    UserName = loginResponse.UserName;
                    IsAuthenticated = true;
                }
            }
        }

        partial void OnUserNameChanged(string value) => LoginCommand.NotifyCanExecuteChanged();
        //partial void OnPasswordChanged(string value) => LoginCommand.NotifyCanExecuteChanged();
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ATS.Models;
using ATS.Services;
using System.Text.Json;
using ATS.Views;
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
            GetUserNameFromSecuredStorage();
        }

        [RelayCommand]
        private async Task Register()
        {
            await clientService.Register(RegisterModel);
        }

        [RelayCommand]
        private async Task Login()
        {
            await clientService.Login(LoginModel);
            GetUserNameFromSecuredStorage();
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }

        [RelayCommand]
        private async Task Logout()
        {
            SecureStorage.Default.Remove("Authentication");
            IsAuthenticated = false;
            UserName = "Guest";
            await Shell.Current.GoToAsync("..");
        }


        private async void GetUserNameFromSecuredStorage()
        {
            if (!string.IsNullOrEmpty(UserName) && UserName! != "Guest")
            {
                IsAuthenticated = true;
                return;
            }
            var serializedLoginResponseInStorage = await SecureStorage.Default.GetAsync("Authentication");
            if (serializedLoginResponseInStorage != null)
            {
                UserName = JsonSerializer.Deserialize<LoginResponse>(serializedLoginResponseInStorage)!.UserName!;
                IsAuthenticated = true;
                return;
            }
            UserName = "Guest";
            IsAuthenticated = false;
        }

        partial void OnUserNameChanged(string value) => LoginCommand.NotifyCanExecuteChanged();
        //partial void OnPasswordChanged(string value) => LoginCommand.NotifyCanExecuteChanged();
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ATS.Models;
using System.Text.Json;
using ATS.Views;
using ATS.Client.Services;
using ATS.Services;
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
        private readonly UserService userService;

        public LoginPageViewModel (ClientService clientService, UserService userService)
        {
            this.clientService = clientService;
            this.userService = userService;
            RegisterModel = new();
            LoginModel = new();
            IsAuthenticated = false;
            //IsErrorVisible = false;
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
                var user = await userService.GetCurrentUserAsync();
                if (user != null && !string.IsNullOrEmpty(user.UserName))
                {
                    await userService.SetCurrentUserAsync(user);
                    IsAuthenticated = true;
                    UserName = user.UserName;
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                }
                else
                {
                    ShowError("Invalid login credentials.");
                }
            }
            else
            {
                ShowError("Login failed. Please try again.");
            }
        }

        [RelayCommand]
        private async Task Logout()
        {
            userService.Logout();
            IsAuthenticated = false;
            UserName = string.Empty;
            await Shell.Current.GoToAsync("..");
        }

        private void ShowError(string message)
        {
            IsErrorVisible = true;
            ErrorMessage = message;
        }

        partial void OnUserNameChanged(string value) => LoginCommand.NotifyCanExecuteChanged();
        //partial void OnPasswordChanged(string value) => LoginCommand.NotifyCanExecuteChanged();
    }
}
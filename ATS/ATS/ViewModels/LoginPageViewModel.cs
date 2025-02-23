using ATS.Services;
using ATS.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ATS.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private bool _isErrorVisible;

        public IRelayCommand LoginCommand { get; }

        public LoginPageViewModel(IAuthService authService)
        {
            IsErrorVisible = false;
            LoginCommand = new RelayCommand(Login, CanLogIn);
            _authService = authService;
        }

        private async void Login()
        {
            if (CanLogIn() && await _authService.isUserAuthenticated())
            {
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            else
            {
                return;
            }
            
        }

        private bool CanLogIn()
        {
            bool canLogin = !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password);
            if (!canLogin)
            {
                ErrorMessage = "Username and password cannot be empty.";
                IsErrorVisible = true;
            }
            else
            {
                ErrorMessage = string.Empty;
                IsErrorVisible = false;
            }
            return canLogin;
        }

        partial void OnUserNameChanged(string value) => LoginCommand.NotifyCanExecuteChanged();
        partial void OnPasswordChanged(string value) => LoginCommand.NotifyCanExecuteChanged();
    }
}

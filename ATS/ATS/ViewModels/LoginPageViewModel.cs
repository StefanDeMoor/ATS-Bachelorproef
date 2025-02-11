using ATS.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ATS.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private bool _isErrorVisible;

        public IRelayCommand LoginCommand { get; }

        public LoginPageViewModel()
        {
            IsErrorVisible = false;
            LoginCommand = new RelayCommand(Inloggen, CanLogIn);
        }

        private async void Inloggen()
        {
            if (!CanLogIn())
            {
                return;
            }
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
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

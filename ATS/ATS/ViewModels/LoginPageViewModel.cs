using ATS.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.ViewModels
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _password;

        public IRelayCommand LoginCommand { get; }

        public LoginPageViewModel()
        {
            LoginCommand = new RelayCommand(Inloggen);
        }

        private async void Inloggen()
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
    }
}

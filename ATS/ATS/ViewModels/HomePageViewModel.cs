using ATS.Models;
using ATS.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json;
using ATS.Client.Services;
using ATS.Services;
using Shared.Enums;

namespace ATS.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private string _userRole;

        public HomePageViewModel(UserService userService)
        {
            _userService = userService;
            InitializeUserRole();
        }

        public bool IsButtonVisible => !string.IsNullOrEmpty(_userRole) && _userRole != "Guest";

        [RelayCommand]
        private async Task GoToData()
        {
            await Shell.Current.GoToAsync($"//{nameof(DataPage)}");
        }

        private async Task InitializeUserRole()
        {
            _userRole = await _userService.GetUserRoleAsync();
            OnPropertyChanged(nameof(IsButtonVisible));
        }
    }
}

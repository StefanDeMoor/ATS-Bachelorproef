using ATS.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using ATS.Models;
using System.Threading.Tasks;
using Shared.Enums;

namespace ATS.ViewModels
{
    public partial class DataPageViewModel : ObservableObject
    {
        private readonly UserService _userService;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string userRole;

        public DataPageViewModel(UserService userService)
        {
            _userService = userService;
            LoadUserData();
        }

        private async Task LoadUserData()
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user != null)
            {
                UserName = user.UserName!.ToLower();
                UserRole = user.Role!.ToLower();
            }
            else
            {
                UserName = "Guest";
                UserRole = "No Role Assigned";
            }
        }
    }
}

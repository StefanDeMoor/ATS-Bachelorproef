using ATS.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ATS.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        public HomePageViewModel ()
        {
           
        }

        [RelayCommand]
        private async Task GoToData()
        {
            await Shell.Current.GoToAsync($"//{nameof(DataPage)}");
        }

    }
}

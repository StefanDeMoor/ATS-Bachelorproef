using ATS.ViewModels;

namespace ATS.Views
{
    public partial class DataPage : ContentPage
    {
        public DataPage(DataPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel; 
        }
    }
}

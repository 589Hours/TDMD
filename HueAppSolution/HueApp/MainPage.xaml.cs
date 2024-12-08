using HueApp.ViewModels;

namespace HueApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();
            BindingContext = mainPageViewModel;
        }

        private void CheckBoxIsLocalhostChanged(object sender, CheckedChangedEventArgs e)
        {
            if (BindingContext is MainPageViewModel viewModel)
            {
                viewModel.IsCheckBoxChanged();
            }
        }
    }
}
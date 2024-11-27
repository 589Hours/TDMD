using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HueApp.Domain.Clients;
namespace HueApp.ViewModels
{
    // All the code in this file is included in all platforms.
    public partial class MainPageViewModel : ObservableObject
    {
        private IPhilipsHueApiClient client;
        private IPreferences preferences;
        public MainPageViewModel(IPreferences preferences, IPhilipsHueApiClient client) 
        {
            this.preferences = preferences;
            this.client = client;
        }

        [ObservableProperty]
        private string entryUsername;

        [ObservableProperty]
        private bool labelBridgeEnabled;

        [ObservableProperty]
        private bool entryBridgeEnabled;

        [ObservableProperty]
        private string entryBridgeText;

        [ObservableProperty]
        private bool labelPortEnabled;

        [ObservableProperty]
        private bool entryPortEnabled;

        [ObservableProperty]
        private string entryPortText;

        private bool CheckedState = true;

        [RelayCommand]
        public async Task CheckBoxLocalHostChanged()
        {
            CheckedState = !CheckedState;
            if (CheckedState)
            {
                CheckedState = !CheckedState;
                entryBridgeEnabled = CheckedState;
                labelBridgeEnabled = CheckedState;
                entryBridgeText = "";
                entryPortEnabled = !CheckedState;
                labelPortEnabled = !CheckedState;
            }
            else
            {
                CheckedState = !CheckedState;
                entryBridgeEnabled = CheckedState;
                labelBridgeEnabled = CheckedState;
                entryPortEnabled = !CheckedState;
                labelPortEnabled = !CheckedState;
                entryPortText = "";
            }
        }

        [RelayCommand]
        public async Task ButtonSubmitClicked()
        {
            string username = entryUsername;

        }
    }
}

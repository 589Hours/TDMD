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
        private bool checkBoxLocalChecked;

        [ObservableProperty]
        private bool checkedValue = true;

        [ObservableProperty]
        private bool checkedValueInverse;

        [ObservableProperty]
        private string entryUsername;

        [ObservableProperty]
        private string entryBridgeText;

        [ObservableProperty]
        private string entryPortText;

        [RelayCommand]
        public void IsCheckBoxChanged()
        {
            if (CheckBoxLocalChecked == true)
            {
                CheckedValue = false;
                CheckedValueInverse = true;
                EntryBridgeText = "";
            }
            else
            {
                CheckedValue = true;
                CheckedValueInverse = false;
                EntryPortText = "";
            }
        }

        [RelayCommand]
        public async Task ButtonSubmitClicked()
        {
            // Get Username and Bridge IP-adress or Port number
            string username = entryUsername;
            string bridgeText = entryBridgeText;
            string portText = entryPortText;


        }
    }
}
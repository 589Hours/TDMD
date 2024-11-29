using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HueApp.Domain.Clients;
using System.Diagnostics;
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

        [RelayCommand]
        public void IsCheckBoxChanged()
        {
            if (CheckBoxLocalChecked == true)
            {
                CheckedValue = false;
                CheckedValueInverse = !CheckedValue;
                EntryBridgeText = "";
            }
            else
            {
                CheckedValue = true;
                CheckedValueInverse = !CheckedValue;
            }
        }

        [RelayCommand]
        public async Task ButtonSubmitClicked()
        {
            //TODO: Handle case where username is empty (To display error messages: Handle event the same way as the checkbox?)

            string username = EntryUsername;
            if (CheckedValue == false)
            {
                string url = $"http://localhost/api";
                client.SetBaseUrl(url);
            }
            else
            {
                string url = $"http://{EntryBridgeText}/api";
                client.SetBaseUrl(url);
            }
            var usernameFromLink = await client.Link(username, DeviceInfo.Platform.ToString());

            // If there is an error, or something else went wrong: Prevent logging in
            if (usernameFromLink == "")
                return;

            preferences.Set("username", usernameFromLink);

            //TODO: Create and navigate to LightPage

        }
    }
}
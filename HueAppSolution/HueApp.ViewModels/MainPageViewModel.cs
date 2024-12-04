using CommunityToolkit.Maui.Alerts;
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
        private ISecureStorage secureStorage;
        public MainPageViewModel(ISecureStorage secureStorage, IPhilipsHueApiClient client)
        {
            this.secureStorage = secureStorage;
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
            // Get username and set base url
            string username = EntryUsername;
            string url;
            if (CheckedValue == false)
            {
                url = $"http://localhost/api/";
            }
            else
            {
                url = $"http://{EntryBridgeText}/api/";   
            }

            //once chosen a way to connect, set the base address
            var usernameFromLink = await client.Link(url, username, DeviceInfo.Platform.ToString());

            // If there is an error, or something else went wrong: Prevent logging in
            if (usernameFromLink == "")
            {
                return;
            }

            // Create username in securestorage
            await secureStorage.SetAsync("authorisedUrl", url+usernameFromLink);
            
            // Navigate to LightPage
            await Shell.Current.GoToAsync("//LightPage");
        }
    }
}
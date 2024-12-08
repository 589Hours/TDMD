using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HueApp.Domain.Clients;
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
            // Get current device
            string device = DeviceInfo.Platform.ToString();
            string url;
            if (CheckedValue == false)
            {
                url = $"http://localhost/api/";
                // Differentiate between Windows and Android-emulator devices
                if (device == "Android")
                    url = $"http://10.0.2.2/api/";
            }
            else
            {
                url = $"http://{EntryBridgeText}/api/";   
            }

            // Once chosen a way to connect, set the base address
            var usernameFromLink = await client.Link(url, username, device);

            // If there is an error, or something else went wrong: Prevent logging in
            if (usernameFromLink == "error")
            {
                this.DisplayToastMessage("An error has occured! Perhaps the username was empty?", ToastDuration.Short, 14);
                return;
            } 
            else if (usernameFromLink == "http request error" || usernameFromLink == "bridge request error" || usernameFromLink == "")
            {
                this.DisplayToastMessage("The request went wrong! Please try again. Was the link button pressed?", ToastDuration.Short, 14);
                return;
            }

            // Create username in securestorage
            await secureStorage.SetAsync("authorisedUrl", url+usernameFromLink);
            
            // Navigate to LightPage
            await Shell.Current.GoToAsync("//LightPage");
        }

        /// <summary>
        /// Method for displaying Toast messages
        /// </summary>
        /// <param name="message"></param>
        private async void DisplayToastMessage(string message, ToastDuration duration, int textSize)
        {
            CancellationTokenSource cancellationTokenSource = new();
            var toastError = Toast.Make(message, duration, textSize);
            await toastError.Show(cancellationTokenSource.Token);
        }

    }
}
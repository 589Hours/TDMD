using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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
            if (CheckedValue == false)
            {
                string url = $"http://localhost/api/";
                client.SetBaseUrl(url);
            }
            else
            {
                string url = $"http://{EntryBridgeText}/api/";
                client.SetBaseUrl(url);
            }
            var usernameFromLink = await client.Link(username, DeviceInfo.Platform.ToString());

            // If there is an error, or something else went wrong: Prevent logging in
            if (usernameFromLink == "")
            {
                this.DisplayToastMessage("Username cannot be empty!", ToastDuration.Short, 14);
                return;
            }
            // Create username in securestorage
            await secureStorage.SetAsync("username", usernameFromLink);

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
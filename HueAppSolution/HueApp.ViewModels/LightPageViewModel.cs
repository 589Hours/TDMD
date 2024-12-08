using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HueApp.Domain.Clients;
using HueApp.Domain.Models.PhilipsLight;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HueApp.ViewModels
{
    public partial class LightPageViewModel : ObservableObject
    {
        private IPhilipsHueApiClient client;
        private ISecureStorage secureStorage;
        private Dictionary<string, Light> lights = new();

        public LightPageViewModel(IPhilipsHueApiClient client, ISecureStorage secureStorage)
        {
            this.client = client;
            this.secureStorage = secureStorage;
        }

        [ObservableProperty]
        private ObservableCollection<string> lampIds = new();

        [RelayCommand]
        public async Task IsItemSelected(string selectedItem)
        {
            // Get the selected light
            if (lights.TryGetValue(selectedItem, out Light light))
            {
                // Add selected light as parameter to the navigation
                var navigationParameters = new Dictionary<string, object>()
                {
                    {"Key", selectedItem},
                    {"Light", light}
                };

                // Navigate to LightDetailPage
                await Shell.Current.GoToAsync("//LightDetailPage", navigationParameters);
            }
        }

        /// <summary>
        /// Retrieves all of the lights and adds them to a dictionary
        /// </summary>
        public async Task FetchLights()
        {
            // Retrieve lights
            var authorisedUrl = await secureStorage.GetAsync("authorisedUrl");
            lights = await client.GetLightsAsync(authorisedUrl);
            if (lights == null || lights.Keys.Count == 0)
            {
                return;
            }

            // Add lights to dictionary
            foreach (var key in lights.Keys)
            {
                LampIds.Add(key);
            }
        }
    }
}
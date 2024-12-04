using CommunityToolkit.Mvvm.ComponentModel;
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

        [ObservableProperty]
        private ObservableCollection<string> lampIds = new();

        public LightPageViewModel(IPhilipsHueApiClient client, ISecureStorage secureStorage)
        {
            this.client = client;
            this.secureStorage = secureStorage;
        }

        public async Task FetchLights()
        {
            var authorisedUrl = await secureStorage.GetAsync("authorisedUrl");
            lights = await client.GetLightsAsync(authorisedUrl);
            if (lights == null || lights.Keys.Count == 0)
            {
                return;
            }
            Debug.WriteLine("Fetched lights and got results!!");

            foreach (var key in lights.Keys)
            {
                LampIds.Add(key);
            }
        }
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HueApp.Domain.Clients;
using HueApp.Domain.Models.PhilipsLight;

namespace HueApp.ViewModels
{
    public partial class LightDetailPageViewModel : ObservableObject, IQueryAttributable
    {
        private IPhilipsHueApiClient client;
        private ISecureStorage secureStorage;
        private string lightKey;
        private bool isLightOn;

        public LightDetailPageViewModel(ISecureStorage secureStorage, IPhilipsHueApiClient client)
        {
            this.secureStorage = secureStorage;
            this.client = client;
        }

        [ObservableProperty]
        private string lightSwitchText;

        [ObservableProperty]
        private Color circleColor;

        [ObservableProperty]
        private Color lightSwitchButtonColor;

        [ObservableProperty]
        private double hue;

        [ObservableProperty]
        private double brightness;

        [ObservableProperty]
        private double saturation;

        [RelayCommand]
        public async void Back()
        {
            // Navigate back to LightPage
            await Shell.Current.GoToAsync("//LightPage");
        }

        /// <summary>
        /// Retrieves the navigation parameters
        /// </summary>
        /// <param name="query"></param>
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            this.lightKey = query["Key"] as string;
            Light? receivedLight = query["Light"] as Light;
            if (receivedLight == null)
            {
                // Navigate back to LightPage
                await Shell.Current.GoToAsync("//LightPage");
                return;
            }
            var lightState = receivedLight.state;
            isLightOn = lightState.on;
            Hue = lightState.hue;
            Brightness = lightState.bri;
            Saturation = lightState.sat;
            CircleColor = Color.FromHsla(Hue / 65535, Saturation / 255, (Brightness / 255) / 2);
            SwitchButtonText();
        }

        /// <summary>
        /// Returns the URL of a specific light
        /// </summary>
        private async Task<string> GetLightUrl()
        {
            var authorisedUrl = await secureStorage.GetAsync("authorisedUrl");
            var finalUrl = $"{authorisedUrl}/lights/{lightKey}/state";
            if (string.IsNullOrEmpty(authorisedUrl))
                return "";
            return finalUrl;
        }

        [RelayCommand]
        public async Task LightSwitch()
        {
            isLightOn = !isLightOn;

            // Send command to turn light off/on
            var url = await GetLightUrl();
            if (string.IsNullOrEmpty(url)) return;
            await client.SendPutCommandAsync(url, new {
                on = isLightOn
            });
            SwitchButtonText();
        }

        [RelayCommand]
        public async void HueSliderChanged()
        {
            var url = await GetLightUrl();
            if (string.IsNullOrEmpty(url)) return;
            CircleColor = Color.FromHsla(Hue / 65535, Saturation / 255, (Brightness / 255) / 2);
            await client.SendPutCommandAsync(url, new
            {
                hue = (int) Hue
            });
        }

        [RelayCommand]
        public async void BrightnessSliderChanged()
        {
            var url = await GetLightUrl();
            if (string.IsNullOrEmpty(url)) return;
            CircleColor = Color.FromHsla(Hue / 65535, Saturation / 255, (Brightness / 255) / 2);
            await client.SendPutCommandAsync(url, new
            {
                bri = (int) Brightness
            });
        }

        [RelayCommand]
        public async void SaturationSliderChanged()
        {
            var url = await GetLightUrl();
            if (string.IsNullOrEmpty(url)) return;
            CircleColor = Color.FromHsla(Hue / 65535, Saturation / 255, (Brightness / 255) / 2);
            await client.SendPutCommandAsync(url, new
            {
                sat = (int) Saturation
            });
        }

        /// <summary>
        /// Helper method to change text displayed on a button
        /// </summary>
        private void SwitchButtonText()
        {
            if (isLightOn == true)
            {
                LightSwitchText = "Turn Light Off";
                LightSwitchButtonColor = Color.FromRgb(0, 255, 0);
            }
            else
            {
                LightSwitchText = "Turn Light On";
                LightSwitchButtonColor = Color.FromRgb(255, 0, 0);
            }
                
        }
    }
}

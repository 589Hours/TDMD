using System.Diagnostics;
using HueApp.Domain.Clients;
using HueApp.Domain.Models.PhilipsLight;
using HueApp.Infrastructure.HueApi;

namespace HueApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            //todo will be changed when MVVM gets implemented.
            PhilipsHueApiClient client = new PhilipsHueApiClient(new HttpClient());

            Dictionary<string, Light> lights = await client.GetLightsAsync();
            //todo sla de lijst met lampen ergens op.

            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";
        }
    }

}

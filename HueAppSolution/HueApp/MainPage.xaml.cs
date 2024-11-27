using System.Diagnostics;
using HueApp.Domain.Clients;
using HueApp.Domain.Models.PhilipsLight;
using HueApp.Infrastructure.HueApi;

namespace HueApp
{
    public partial class MainPage : ContentPage
    {
        public bool CheckedState = true;

        public MainPage()
        {
            InitializeComponent();
        }

        private void MainPageLoaded(object sender, EventArgs e)
        {

        }

        private void CheckBoxIsLocalHostChanged(object sender, CheckedChangedEventArgs e)
        {
            if (CheckBoxIsLocalhost.IsChecked)
            {
                CheckedState = !CheckedState;
                EntryBridge.IsEnabled = CheckedState;
                LabelBridge.IsEnabled = CheckedState;
                EntryBridge.Text = "";
                EntryPort.IsEnabled = !CheckedState;
                LabelPort.IsEnabled = !CheckedState;
            } else
            {
                CheckedState = !CheckedState;
                EntryBridge.IsEnabled = CheckedState;
                LabelBridge.IsEnabled = CheckedState;
                EntryPort.IsEnabled = !CheckedState;
                LabelPort.IsEnabled = !CheckedState;
                EntryPort.Text = "";
            }
        }

        private async void ButtonSubmitClicked(object sender, EventArgs e)
        {
            //todo will be changed when MVVM gets implemented.
            PhilipsHueApiClient client = new PhilipsHueApiClient(new HttpClient());

            Dictionary<string, Light> lights = await client.GetLightsAsync();
            //todo sla de lijst met lampen ergens op.
        }
    }
}

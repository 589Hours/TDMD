using HueApp.Domain.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApp.ViewModels
{
    public partial class LightPageViewModel
    {
        private IPhilipsHueApiClient client;
        private IPreferences preferences;
        public LightPageViewModel(IPreferences preferences, IPhilipsHueApiClient client)
        {
            this.preferences = preferences;
            this.client = client;
        }

    }
}
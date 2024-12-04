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
        public LightPageViewModel(IPhilipsHueApiClient client)
        {
            this.client = client;
        }

    }
}
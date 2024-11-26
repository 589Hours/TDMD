using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HueApp.Domain.Models.PhilipsLight;

namespace HueApp.Domain.Clients
{
    public interface IPhilipsHueApiClient
    {
        Task<Dictionary<string, Light>> GetLightsAsync();
        Task SendPutCommandAsync();
    }
}

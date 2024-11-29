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
        void SetBaseUrl(string url);
        Task<string> Link(string username, string device);
        Task<Dictionary<string, Light>> GetLightsAsync();
        Task<string> SendPutCommandAsync(string requestUrlPart, string body);
    }
}

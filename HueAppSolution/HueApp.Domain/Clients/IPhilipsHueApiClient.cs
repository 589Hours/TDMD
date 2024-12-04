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
        Task<string> Link(string apiUrl,string username, string device);
        Task<Dictionary<string, Light>> GetLightsAsync(string username);
        Task<string> SendPutCommandAsync(string requestUrlPart, string body);
    }
}

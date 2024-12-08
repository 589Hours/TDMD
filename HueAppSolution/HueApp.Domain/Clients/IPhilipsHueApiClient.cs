using HueApp.Domain.Models.PhilipsLight;

namespace HueApp.Domain.Clients
{
    public interface IPhilipsHueApiClient
    {
        Task<string> Link(string apiUrl,string username, string device);
        Task<Dictionary<string, Light>> GetLightsAsync(string authorizedUrl);
        Task<string> SendPutCommandAsync(string putUrl, object body);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routey.Domain.Clients
{
    public interface IWeatherApiClient
    {
        Task<string> GetRealtimeWeatherAsync();

        // Helper methods:
        Task<string> GetTemperatureAsync(string weatherData);
        Task<string> GetConditionIconAsync(string weatherData);
    }
}

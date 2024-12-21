using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Routey.Domain.Clients;

namespace Routey.Infrastructure.WeatherApiClient
{
    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient httpClient;
        public WeatherApiClient(HttpClient client) 
        { 
            this.httpClient = client;
        }

        public Task<string> GetRealtimeWeatherAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetTemperatureAsync(string weatherData)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetConditionIconAsync(string weatherData)
        {
            throw new NotImplementedException();
        }
    }
}

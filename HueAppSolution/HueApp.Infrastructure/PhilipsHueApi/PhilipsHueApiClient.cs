using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using HueApp.Domain.Clients;
using HueApp.Domain.Models.PhilipsLight;

namespace HueApp.Infrastructure.HueApi
{
    public class PhilipsHueApiClient : IPhilipsHueApiClient

    {
        private HttpClient httpClient;
        public PhilipsHueApiClient (HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Dictionary<string, Light>> GetLightsAsync()
        {
            //TODO recieve URL from user OR add base url AND edit url to how its better working
            try
            {
                var response = await httpClient.GetAsync("http://localhost:80/api/newdeveloper");
                response.EnsureSuccessStatusCode();

                var responseModel = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(responseModel);
                JsonElement root = doc.RootElement;

                JsonElement lightsElement = root.GetProperty("lights");

                var lightDictionary = lightsElement.Deserialize<Dictionary<string, Light>>();

                foreach (var light in lightDictionary)
                {
                    string key = light.Key;
                    Light lightData = light.Value;

                    Debug.WriteLine($"Key: {key} with data: \n{lightData}");
                }
                
                return lightDictionary;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task SendPutCommandAsync() 
        {
            throw new NotImplementedException();
        }
    }
}

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

        public void SetBaseUrl(string url)
        {
            httpClient.BaseAddress = new Uri(url);
        }

        public async Task<string> SendPutCommandAsync(string requestUrlPart, string body)
        {
            var putCommand = httpClient.PutAsJsonAsync<string>(requestUrlPart, body);
            var result = putCommand.Result;

            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
        }

        public JsonElement GetJsonRootElement(string response)
        {
            using JsonDocument doc = JsonDocument.Parse(response);
            JsonElement root = doc.RootElement;
            return root;
        }

        public async Task<Dictionary<string, Light>> GetLightsAsync()
        {
            //TODO recieve URL from user OR add base url AND edit url to how its better working
            try
            {
                var response = await httpClient.GetAsync("newdeveloper");
                response.EnsureSuccessStatusCode();

                var responseModel = await response.Content.ReadAsStringAsync();

                var root = GetJsonRootElement(responseModel);
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

        public async Task<string> Login(string username)
        {
            var response = await httpClient.GetAsync(username);
            response.EnsureSuccessStatusCode();
            var json = GetJsonRootElement(await response.Content.ReadAsStringAsync());
            //TODO read response from login request

            return "";
        }

        public Task<string> SignUp(string username)
        {
            throw new NotImplementedException();
        }
    }
}

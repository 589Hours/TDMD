using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using HueApp.Domain.Clients;
using HueApp.Domain.Models.PhilipsLight;

namespace HueApp.Infrastructure.PhilipsHueApi
{
    public class PhilipsHueApiClient : IPhilipsHueApiClient
    {
        private HttpClient httpClient;

        public PhilipsHueApiClient (HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> SendPutCommandAsync(string putUrl, object body)
        {
            var putCommand = httpClient.PutAsJsonAsync(putUrl, body);
            var result = putCommand.Result;

            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsStringAsync();
        }

        public JsonElement GetJsonRootElement(string response)
        {
            JsonDocument doc = JsonDocument.Parse(response);
            JsonElement root = doc.RootElement;
            return root;
        }

        public async Task<Dictionary<string, Light>> GetLightsAsync(string authorisedUrl)
        {
            try
            {
                var fullUrl = $"{authorisedUrl}/lights";
                Debug.WriteLine(fullUrl);
                var response = await httpClient.GetAsync(fullUrl);
                response.EnsureSuccessStatusCode();

                var responseModel = await response.Content.ReadAsStringAsync();
                var root = GetJsonRootElement(responseModel);
                var lightDictionary = root.Deserialize<Dictionary<string, Light>>();

                foreach (var light in lightDictionary)
                {
                    string key = light.Key;
                    Light lightData = light.Value;
                }
                return lightDictionary;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<string> Link(string apiUrl, string username, string device)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(apiUrl, new
                {
                    devicetype = $"HueApp#{device} {username}"
                });
                response.EnsureSuccessStatusCode();
                var json = GetJsonRootElement(await response.Content.ReadAsStringAsync());

                var responsebody = json[0];
                if (responsebody.TryGetProperty("success", out JsonElement succesElement))
                {
                    if (succesElement.TryGetProperty("username", out JsonElement usernameProperty))
                    {
                        var usernameFromLink = usernameProperty.GetString();
                        return usernameFromLink;
                    }
                    return "";
                }
                return "";
            } 
            catch (UriFormatException ex)
            {
                Debug.Write(ex);
                return "bridge request error";
            } 
            catch (HttpRequestException exc)
            {
                Debug.Write(exc);
                return "http request error";
            }
            catch (Exception e)
            {
                Debug.Write(e);
                return "error";
            }
        }
    }
}

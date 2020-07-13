using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Onfido.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Onfido.Api.Services
{
    public class HttpService
    {
        public readonly OnfidoSettings onfidoSettings;
        public HttpService(IOptions<OnfidoSettings> optionOnfidoSettings)
        {
            this.onfidoSettings = optionOnfidoSettings.Value;
        }

        public async Task<(bool Status, string Message, T Data)> PostAsync<T>(string url, object content)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", $"token={onfidoSettings.AuthToken}");

                    var serialized = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await client.PostAsync(url, serialized))
                    {
                        response.EnsureSuccessStatusCode();
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<T>(responseBody);

                        return (true, "Success", data);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return (false, ex.Message, default(T));
            }
        }

        public async Task<(bool Status, string Message, T Data)> GetAsync<T>(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", $"token={onfidoSettings.AuthToken}");

                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        response.EnsureSuccessStatusCode();
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<T>(responseBody);

                        return (true, "Success", data);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return (false, ex.Message, default(T));
            }
        }
    }
}

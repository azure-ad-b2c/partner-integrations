using Jumio.Api.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Jumio.Api.Services
{
    public class HttpService
    {
        public readonly AppSettings appSettings;

        public readonly JumioSettings jumioSettings;
        public HttpService(IOptions<AppSettings> optionAppSettings, IOptions<JumioSettings> optionjumioSettings)
        {
            this.appSettings = optionAppSettings.Value;
            this.jumioSettings = optionjumioSettings.Value;
        }

        public async Task<(bool Status, string Message, T Data)> PostAsync<T>(string url, object content)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                       new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("User-Agent", jumioSettings.UserAgent);

                    var credentials = $"{jumioSettings.AuthUsername}:{jumioSettings.AuthPassword}";

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

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

                    client.DefaultRequestHeaders.Add("User-Agent", jumioSettings.UserAgent);

                    var credentials = $"{jumioSettings.AuthUsername}:{jumioSettings.AuthPassword}";

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

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

namespace Api.Services
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;
    using Models.Configuration;

    public class IdologyService : IIdologyService
    {
        private readonly IOptions<IdologyConfig> _config;
        private readonly ILogger<IdologyService> _logger;

        public IdologyService(ILogger<IdologyService> logger, IOptions<IdologyConfig> config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<IExpectIdOutput> ExpectIdCall(IExpectIdInput expectIdInput)
        {
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("username", _config.Value.ApiUsername));
            parameters.Add(new KeyValuePair<string, string>("password", _config.Value.ApiPassword));
            parameters.Add(new KeyValuePair<string, string>("firstName", expectIdInput.FirstName));
            parameters.Add(new KeyValuePair<string, string>("lastName", expectIdInput.LastName));
            parameters.Add(new KeyValuePair<string, string>("address", expectIdInput.StreetAddress));
            parameters.Add(new KeyValuePair<string, string>("zip", expectIdInput.Zip));

            string responseData;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, _config.Value.ApiEndpoint))
            {
                request.Content = new FormUrlEncodedContent(parameters);

                var response = await client.SendAsync(request);
                responseData = await response.Content.ReadAsStringAsync();

                _logger.LogDebug($"Result: {responseData}");
            }

            var root = XElement.Parse(responseData);
            var errorElement = root.XPathSelectElement("/error");

            if (errorElement != null)
            {
                return new ExpectIdOutput {Success = false, Error = errorElement.Value};
            }

            var summaryKey = root.XPathSelectElement("/summary-result/key");
            if (summaryKey?.Value == "id.failure")
            {
                var message = root.XPathSelectElement("/results/message");

                return new ExpectIdOutput {Success = false, Error = message.Value, Output = responseData };
            }

            return new ExpectIdOutput { Success = true, Output = responseData };
        }
    }
}
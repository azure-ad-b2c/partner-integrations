namespace Api.Services
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;
    using Models.Configuration;
    using Newtonsoft.Json;

    public class IntegrationService : IIntegrationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IntegrationService> _logger;
        private readonly ThreatMetrixConfig _threatMetrixConfig;

        public IntegrationService(HttpClient httpClient,
            IOptions<ThreatMetrixConfig> threatMetrixConfigOptions,
            ILogger<IntegrationService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentException(nameof(HttpClient));
            _threatMetrixConfig = threatMetrixConfigOptions.Value;
            _logger = logger;
        }

        public async Task<SessionDataOutput> GetSessionData(SessionQueryServiceInput sessionQueryServiceInput)
        {
            var data = new
            {
                Org_Id = _threatMetrixConfig.OrgId,
                Api_Key = _threatMetrixConfig.ApiKey,
                Output_Format = "Json",
                Session_Id = sessionQueryServiceInput.SessionId,
                Service_Type = _threatMetrixConfig.ServiceType,
                Event_Type = sessionQueryServiceInput.EventType,
                Account_Email = sessionQueryServiceInput.Email,
                Account_Telephone = sessionQueryServiceInput.PhoneNumber,
                _threatMetrixConfig.Policy
            };

            var json = JsonConvert.SerializeObject(data);

            _httpClient.BaseAddress = new Uri(_threatMetrixConfig.Url);
            var response = await _httpClient.PostAsync(_threatMetrixConfig.SessionQueryEndPoint,
                new StringContent(json, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();

            var sessionResponse = JsonConvert.DeserializeObject<SessionResponse>(responseData);

            var output = new SessionDataOutput
            {
                ReviewStatus = sessionResponse?.GetReviewStatus(),
                FullOutput = responseData,
                PolicyScore = "", //sessionResponse.policy_details_api.policy_detail_api[0].customer.review_status,
                ReasonCode = sessionResponse?.GetReasonCode(),
                RiskRating = sessionResponse?.GetRiskRating()
            };

            return output;
        }
    }
}
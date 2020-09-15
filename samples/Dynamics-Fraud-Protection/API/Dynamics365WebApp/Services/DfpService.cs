using Dynamics365WebApp.Models;
using Dynamics365WebApp.Models.ApiModels;
using Dynamics365WebApp.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics365WebApp.Services
{
    public class DfpService
    {
        private readonly FraudProtectionSettings fraudProtectionSettings;
        private readonly TokenProviderServiceSettings tokenProviderServiceSettings;

        public DfpService(IOptions<FraudProtectionSettings> fraudProtectionSettings, IOptions<TokenProviderServiceSettings> tokenProviderServiceSettings)
        {
            this.fraudProtectionSettings = fraudProtectionSettings.Value;
            this.tokenProviderServiceSettings = tokenProviderServiceSettings.Value;
        }

        public string NewCorrelationId => Guid.NewGuid().ToString();

        public async Task<(bool Status, string Message, DfpAccountActionResponse Data)> CreateAccount(DfpCreateAccountInputClaims input, string correlationId, string signUpId)
        {
            var endpoint = $"/v1.0/action/account/create/{signUpId}";

            var createAccountInput = new
            {
                Metadata = new
                {
                    TrackingId = Guid.NewGuid().ToString(),
                    SignUpId = signUpId,
                    CustomerLocalDate = DateTime.Now,
                    MerchantTimeStamp = DateTime.Now,
                    AssessmentType = "Protect"
                },
                User = new
                {
                    Username = input.Email,
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    Language = input.Language,
                    UserType = "Consumer",
                },
                SsoAuthenticationProvider = new
                {
                    DisplayName = input.DisplayName,
                    AuthenticationProvider = input.DisplayName
                },
                Email = new[] {
                    new
                    {
                        EmailValue= input.Email,
                        IsEmailValidated= input.IsEmailValidated,
                        IsEmailUsername=input.IsEmailUsername,
                        EmailType= "Primary"
                    }
                },
                Device = new
                {
                    IpAddress = input.IpAddress,
                    DeviceContextId = input.DeviceContextId,
                    Provider = "DFPFingerprinting"
                },

                Name = "AP.AccountCreation",
                Version = "0.5"
            };

            return await PostAsync<DfpAccountActionResponse>(endpoint, createAccountInput, correlationId);
        }

        public async Task<(bool Status, string Message, DfpAccountStatusResponse Data)> CreateAccountStatus(DfpCreateAccountStatusInputClaims input, string correlationId)
        {
            var endpoint = $"/v1.0/observe/account/create/status/{input.SignUpId}";

            var createAccountInput = new
            {
                Metadata = new
                {
                    TrackingId = Guid.NewGuid().ToString(),
                    SignUpId = input.SignUpId,
                    MerchantTimeStamp = DateTime.Now,
                    UserId = input.UserId ?? "UnKnown"
                },
                StatusDetails = new
                {
                    StatusType = input.StatusType,
                    ReasonType = input.ReasonType,
                    ChallengeType = input.ChallengeType,
                    StatusDate = DateTime.Now
                },
                Name = "AP.AccountCreation.Status",
                Version = "0.5"
            };

            return await PostAsync<DfpAccountStatusResponse>(endpoint, createAccountInput, correlationId);
        }

        public async Task<(bool Status, string Message, DfpAccountActionResponse Data)> LoginAccount(DfpLoginAccountInputClaims input, string correlationId, string loginId)
        {
            var endpoint = $"/v1.0/action/account/login/{input.UserId}";

            var createAccountInput = new
            {
                Metadata = new
                {
                    TrackingId = Guid.NewGuid().ToString(),
                    LoginId = loginId,
                    CustomerLocalDate = DateTime.Now,
                    MerchantTimeStamp = DateTime.Now,
                    AssessmentType = "Protect"
                },
                User = new
                {
                    Username = input.Email,
                    UserId = input.UserId,
                    UserType = "Consumer",
                },
                Device = new
                {
                    IpAddress = input.IpAddress,
                    DeviceContextId = input.DeviceContextId,
                    Provider = "DFPFingerprinting"
                },

                Name = "AP.AccountLogin",
                Version = "0.5"
            };

            return await PostAsync<DfpAccountActionResponse>(endpoint, createAccountInput, correlationId);
        }

        public async Task<(bool Status, string Message, DfpAccountStatusResponse Data)> LoginAccountStatus(DfpLoginAccountStatusInputClaims input, string correlationId)
        {
            var endpoint = $"/v1.0/observe/account/login/status/{input.UserId}";

            var createAccountInput = new
            {
                Metadata = new
                {
                    TrackingId = Guid.NewGuid().ToString(),
                    LoginId = input.LoginId,
                    MerchantTimeStamp = DateTime.Now,
                    UserId = input.UserId
                },
                StatusDetails = new
                {
                    StatusType = input.StatusType,
                    ReasonType = input.ReasonType,
                    ChallengeType = input.ChallengeType,
                    StatusDate = DateTime.Now
                },
                Name = "AP.AccountLogin.Status",
                Version = "0.5"
            };

            return await PostAsync<DfpAccountStatusResponse>(endpoint, createAccountInput, correlationId);
        }

        #region PRIVATE METHODS
        private async Task<(bool Status, string Message, T Data)> PostAsync<T>(string endpoint, object content, string correlationId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var authToken = await AcquireTokenAsync();

                    var url = $"{fraudProtectionSettings.ApiBaseUrl}{endpoint}";


                    client.DefaultRequestHeaders.Accept.Add(
                             new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("x-ms-correlation-id", correlationId);

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authToken);

                    var serializationSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };

                    var json = JsonConvert.SerializeObject(content, serializationSettings);
                    var serialized = new StringContent(JsonConvert.SerializeObject(content, serializationSettings), Encoding.UTF8, "application/json");

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

        private async Task<string> AcquireTokenAsync()
        {
            if (string.IsNullOrEmpty(tokenProviderServiceSettings.CertificateThumbprint) && string.IsNullOrEmpty(tokenProviderServiceSettings.ClientSecret))
                throw new InvalidOperationException("Configure the token provider settings in the appsettings.json file.");

            if (tokenProviderServiceSettings.CertificateThumbprint != "" && tokenProviderServiceSettings.ClientSecret != "")
                throw new InvalidOperationException("Only configure certificate or secret authenticate, not both, in the appsettings file.");

            return tokenProviderServiceSettings.CertificateThumbprint != "" ?
                await AcquireTokenWithCertificateAsync() :
                await AcquireTokenWithSecretAsync();
        }

        private async Task<string> AcquireTokenWithCertificateAsync()
        {
            var x509Cert = CertificateUtility.GetByThumbprint(tokenProviderServiceSettings.CertificateThumbprint);
            var clientAssertion = new ClientAssertionCertificate(tokenProviderServiceSettings.ClientId, x509Cert);
            var context = new AuthenticationContext(tokenProviderServiceSettings.Authority);
            var authenticationResult = await context.AcquireTokenAsync(tokenProviderServiceSettings.Resource, clientAssertion);

            return authenticationResult.AccessToken;
        }

        private async Task<string> AcquireTokenWithSecretAsync()
        {
            var clientAssertion = new ClientCredential(tokenProviderServiceSettings.ClientId, tokenProviderServiceSettings.ClientSecret);
            var context = new AuthenticationContext(tokenProviderServiceSettings.Authority);
            var authenticationResult = await context.AcquireTokenAsync(tokenProviderServiceSettings.Resource, clientAssertion);

            return authenticationResult.AccessToken;
        }

        #endregion
    }
}

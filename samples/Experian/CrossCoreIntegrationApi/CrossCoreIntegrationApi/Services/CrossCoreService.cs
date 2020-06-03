namespace CrossCoreIntegrationApi.Services
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;
    using Models.Configuration;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class CrossCoreService : IService
    {
        private readonly IOptions<CrossCoreConfig> _config;
        private readonly ILogger<CrossCoreService> _logger;

        public CrossCoreService(ILogger<CrossCoreService> logger, IOptions<CrossCoreConfig> config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<ICrossCoreOutput> ServiceCall(ICrossCoreInput input)
        {
            var req = SetupRequest(input);
            var contractResolver = new DefaultContractResolver {NamingStrategy = new CamelCaseNamingStrategy {OverrideSpecifiedNames = false}};
            var settings = new JsonSerializerSettings {ContractResolver = contractResolver, NullValueHandling = NullValueHandling.Ignore};

            var json = JsonConvert.SerializeObject(req, settings);

            var clientHandler = new HttpClientHandler();

            clientHandler.ClientCertificates.Add(GetCertificate());
            clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;

            string responseData;

            using (var client = new HttpClient(clientHandler))
            using (var request = new HttpRequestMessage(HttpMethod.Post, _config.Value.ApiEndpoint))
            {
                var signature = GetSignature(json);
                request.Headers.Add("hmac-signature", signature);

                _logger.LogDebug($"Content: {json}");

                request.Content = new StringContent(json, Encoding.Default, "application/json");


                var headers = request.Headers.ToList();

                var response = await client.SendAsync(request);
                responseData = await response.Content.ReadAsStringAsync();

                _logger.LogDebug($"Result: {responseData}");
            }

            var ccResponse = JsonConvert.DeserializeObject<CCResponse>(responseData);

            var output = new CrossCoreOutput()
            {
                Decision = ccResponse.ResponseHeader.OverallResponse.Decision, 
                FullOutput = responseData, 
                Score = ccResponse.ClientResponsePayload.DecisionElements.FirstOrDefault()?.Scores.FirstOrDefault()?.Score.ToString()
            };

            return output;
        }

        private X509Certificate2 GetCertificate()
        {
            using (var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                certStore.Open(OpenFlags.ReadOnly);

                var certCollection = certStore.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    // Replace below with your certificate's thumbprint
                    _config.Value.CertificateThumbprint,
                    false);

                // Get the first cert with the thumbprint
                var cert = certCollection.OfType<X509Certificate2>().FirstOrDefault();

                if (cert == null)
                {
                    throw new Exception("Certificate could not be found");
                }

                return cert;
            }
        }

        private string GetSignature(string message)
        {
            var key = _config.Value.SignatureKey;

            using (var hmac = new HMACSHA256(Encoding.Default.GetBytes(key)))
            {
                var messageBytes = Encoding.UTF8.GetBytes(message);

                var hashValue = hmac.ComputeHash(messageBytes);

                return Convert.ToBase64String(hashValue);
            }
        }

        private CrossCoreRequest SetupRequest(ICrossCoreInput input)
        {
            var config = _config.Value;
            var crossCoreDefaults = config.Defaults;

            var request = new CrossCoreRequest();
            request.Header.TenantId = config.TenantId;
            request.Header.RequestType = crossCoreDefaults.HdrRequestType;
            request.Header.ClientReferenceId = input.CorrelationId;
            //request.Header.ExpRequestId = string.Empty;
            request.Header.MessageTime = DateTime.UtcNow.ToString("s") + "Z";

            request.Header.Options.Workflow = crossCoreDefaults.HdrOptionsWorkFlow;

            request.Payload.Control.Add(new CCControl {Option = "ORG_CODE", Value = config.OrgCode});
            request.Payload.Control.Add(new CCControl {Option = "MODEL_CODE", Value = config.ModelCode});
            request.Payload.Control.Add(new CCControl {Option = "EVENT_ID", Value = input.CorrelationId});

            request.Payload.Application.Type = crossCoreDefaults.PlApplicationType;
            request.Payload.Application.ApplicantionId = crossCoreDefaults.PlApplicationId;
            request.Payload.Application.OriginalRequestTime = DateTime.UtcNow.ToString("s") + "Z";
            request.Payload.Application.Applicants.Add(new CCApplicant
            {
                Type = crossCoreDefaults.PlApplicantsType,
                Id = crossCoreDefaults.PlApplicantsApplicantId,
                ApplicantType = crossCoreDefaults.PlApplicantsApplicantType,
                ContactId = crossCoreDefaults.PlApplicantsContactId
            });

            if (input.DeviceIpAddress != null)
            {
                request.Payload.Device.Id = crossCoreDefaults.PlDeviceId;
                request.Payload.Device.IpAddress = input.DeviceIpAddress;
                request.Payload.Device.Jsc = "true";
                request.Payload.Device.hdim.Payload = "true";
            }
            else
            {
                request.Payload.Device = null;
            }

            var contact = new CCContact {Id = crossCoreDefaults.PlContactId};

            if (input.Email != null)
            {
                contact.Emails.Add(new CCContactEmail
                {
                    Id = crossCoreDefaults.PlContactEmailId, Type = crossCoreDefaults.PlContactEmailType, Email = input.Email
                });
            }
            else
            {
                contact.Emails = null;
            }

            contact.Person.Names.Add(new CCContactPersonName
            {
                Id = crossCoreDefaults.PlContactPersonId,
                Type = crossCoreDefaults.PlContactPersonType,
                FirstName = input.GivenName,
                MiddleNames = input.MiddleName,
                SurName = input.SurName
            });

            contact.Addresses.Add(new CCContactAddress
            {
                Id = crossCoreDefaults.PlContactAddressId,
                AddressType = crossCoreDefaults.PlContactAddressType,
                CountryCode = input.CountryCode,
                Postal = input.Postal,
                PostTown = input.PostTown,
                StateProvinceCode = input.StateProvinceCode,
                Street = input.Street
            });

            if (!string.IsNullOrEmpty(input.PhoneNumber))
            {
                contact.Telephones.Add(new CCContactTelephone
                {
                    Id = crossCoreDefaults.PlContactTelephoneId,
                    PhoneIdentifier = crossCoreDefaults.PlContactTelephoneIdentifier,
                    Number = input.PhoneNumber
                });
            }
            else
            {
                contact.Telephones = null;
            }

            request.Payload.Contacts.Add(contact);

            return request;
        }
    }
}
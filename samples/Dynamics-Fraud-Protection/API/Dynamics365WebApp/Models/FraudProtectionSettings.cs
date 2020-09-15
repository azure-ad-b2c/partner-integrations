using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynamics365WebApp.Models
{
    public class FraudProtectionSettings
    {
        public string InstanceId { get; set; }
        public string ApiBaseUrl { get; set; }

        public string DeviceFingerprintingDomain { get; set; }
    }

    public class TokenProviderServiceSettings
    {
        public string Resource { get; set; }
        public string ClientId { get; set; }
        public string Authority { get; set; }
        public string CertificateThumbprint { get; set; }
        public string ClientSecret { get; set; }
    }
}

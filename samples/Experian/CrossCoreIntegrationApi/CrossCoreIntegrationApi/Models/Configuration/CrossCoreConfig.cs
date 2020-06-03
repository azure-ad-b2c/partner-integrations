namespace CrossCoreIntegrationApi.Models.Configuration
{
    public class CrossCoreConfig
    {
        public string ApiEndpoint { get; set; }

        public string CertificateThumbprint { get; set; }

        public string SignatureKey { get; set; }

        public string TenantId { get; set; }

        public string OrgCode { get; set; }

        public string ModelCode { get; set; }

        public CrossCoreDefaults Defaults { get; set; }
    }
}
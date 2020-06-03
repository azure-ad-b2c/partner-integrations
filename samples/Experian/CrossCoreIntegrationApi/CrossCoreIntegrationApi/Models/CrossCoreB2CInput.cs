namespace CrossCoreIntegrationApi.Models
{
    public class CrossCoreB2CInput : ICrossCoreInput
    {
        public string CorrelationId { get; set; }

        public string DeviceIpAddress { get; set; }

        public string Email { get; set; }

        public string GivenName { get; set; }

        public string MiddleName { get; set; }

        public string SurName { get; set; }

        public string Street { get; set; }

        public string PostTown { get; set; }

        public string Postal { get; set; }

        public string StateProvinceCode { get; set; }

        public string CountryCode { get; set; }

        public string PhoneNumber { get; set; }
    }
}
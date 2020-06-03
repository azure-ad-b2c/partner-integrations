namespace CrossCoreIntegrationApi.Models
{
    using System.Collections.Generic;

    public class CrossCoreRequest
    {
        public CCHeader Header { get; set; } = new CCHeader();

        public CCPayload Payload { get; set; } = new CCPayload();
    }

    public class CCHeader
    {
        public string TenantId { get; set; }

        public string RequestType { get; set; }

        public string ClientReferenceId { get; set; }

        //public string ExpRequestId { get; set; }

        public string MessageTime { get; set; }

        public CCHeaderOptions Options { get; set; } = new CCHeaderOptions();

        public CCResponseInfo OverallResponse { get; set; }
    }

    public class CCHeaderOptions
    {
        public string Workflow { get; set; }
    }

    public class CCPayload
    {
        public string Source => "WEB";

        public List<CCControl> Control { get; } = new List<CCControl>();

        public CCApplication Application { get; set; } = new CCApplication();

        public CCDevice Device { get; set; } = new CCDevice();

        public List<CCContact> Contacts { get; } = new List<CCContact>();
    }

    public class CCControl
    {
        public string Option { get; set; }
        public string Value { get; set; }
    }

    public class CCApplication
    {
        public string Type { get; set; }

        public string ApplicantionId { get; set; }

        public string OriginalRequestTime { get; set; }

        public List<CCApplicant> Applicants { get; } = new List<CCApplicant>();
    }

    public class CCApplicant
    {
        public string Id { get; set; }

        public string Type { get; set; }

        public string ApplicantType { get; set; }

        public string ContactId { get; set; }
    }

    public class CCDevice
    {
        public string Id { get; set; }

        public string IpAddress { get; set; }
        public string Jsc { get; set; }

        public CCDeviceHdim hdim { get; set; } = new CCDeviceHdim();
    }

    public class CCDeviceHdim
    {
        public string Payload { get; set; }
    }

    public class CCContact
    {
        public string Id { get; set; }
        public List<CCContactEmail> Emails { get; set; } = new List<CCContactEmail>();

        public CCContactPerson Person { get; set; } = new CCContactPerson();

        public List<CCContactAddress> Addresses { get; } = new List<CCContactAddress>();
        public List<CCContactTelephone> Telephones { get; set; } = new List<CCContactTelephone>();
    }

    public class CCContactEmail
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
    }

    public class CCContactPerson
    {
        public List<CCContactPersonName> Names { get; } = new List<CCContactPersonName>();
    }

    public class CCContactPersonName
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string SurName { get; set; }
    }

    public class CCContactAddress
    {
        public string Id { get; set; }
        public string AddressType { get; set; }
        public string Street { get; set; }
        public string PostTown { get; set; }
        public string Postal { get; set; }
        public string StateProvinceCode { get; set; }
        public string CountryCode { get; set; }
    }

    public class CCContactTelephone
    {
        public string Id { get; set; }
        public string PhoneIdentifier { get; set; }
        public string Number { get; set; }
    }
}
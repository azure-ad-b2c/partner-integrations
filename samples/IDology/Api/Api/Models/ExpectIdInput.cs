namespace Api.Models
{
    public class ExpectIdInput : IExpectIdInput
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }
    }

    public class ExpectIdExtIdInput : IExpectIdInput
    {
        public string GivenName { get; set; }

        public string PostalCode { get; set; }

        public string Surname { get; set; }

        public string FirstName => GivenName;

        public string LastName => Surname;

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip => PostalCode;
    }
}
namespace Api.Models
{
    public interface IExpectIdInput
    {
        string City { get; }
        string FirstName { get; }
        string LastName { get; }
        string State { get; }
        string StreetAddress { get; }
        string Zip { get; }
    }
}
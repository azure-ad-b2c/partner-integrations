namespace Api.Models
{
    public interface IServiceInput
    {
        string Email { get; set; }

        string PhoneNumber { get; set; }

        string SessionId { get; set; }

        string EventType { get; set; }

        string ServiceType { get; set; }

        string Policy { get; set; }
    }
}
namespace Api.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SessionQueryServiceInput : IServiceInput
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string SessionId { get; set; }

        [Required]
        public string EventType { get; set; }

        [Required]
        public string ServiceType { get; set; }

        [Required]
        public string Policy { get; set; }
    }
}
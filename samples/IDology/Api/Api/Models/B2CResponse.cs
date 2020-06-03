namespace Api.Models
{
    using System.Net;

    public class B2CResponse
    {
        public string Version { get; set; } = "0.0.1";

        public string Code { get; set; } = "Error";

        public string UserMessage { get; set; }

        public string DeveloperMessage { get; set; }

        public int Status { get; set; } = (int) HttpStatusCode.Conflict;
    }
}
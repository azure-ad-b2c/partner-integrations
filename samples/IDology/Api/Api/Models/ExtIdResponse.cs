namespace Api.Models
{
    using System.Net;
    using System.Reflection;

    public class ExtIdResponse
    {
        public ExtIdResponse(string code, string message, HttpStatusCode status, string action)
        {
            Code = code;
            UserMessage = message;
            Status = (int) status;
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Action = action;
        }

        public string Version { get; set; }

        public int Status { get; set; }

        public string UserMessage { get; set; }

        public string Action { get; set; }

        public string Code { get; set; }
    }
}
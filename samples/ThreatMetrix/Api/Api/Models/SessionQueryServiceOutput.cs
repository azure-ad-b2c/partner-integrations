namespace Api.Models
{
    using Newtonsoft.Json.Linq;

    public class SessionQueryServiceOutput
    {
        public string Result { get; set; }
        public JObject Data { get; set; }
    }
}
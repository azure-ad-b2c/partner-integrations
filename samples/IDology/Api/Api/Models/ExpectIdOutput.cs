namespace Api.Models
{
    public class ExpectIdOutput : IExpectIdOutput
    {
        public string Error { get; set; }

        public bool Success { get; set; }

        public string Output { get; set; }
    }
}
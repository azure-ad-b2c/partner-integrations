namespace Api.Models
{
    public class SessionDataOutput
    {
        public string FullOutput { get; set; }

        public string ReviewStatus { get; set; }
        public string PolicyScore { get; set; }
        public string RiskRating { get; set; }
        public string ReasonCode { get; set; }
    }
}
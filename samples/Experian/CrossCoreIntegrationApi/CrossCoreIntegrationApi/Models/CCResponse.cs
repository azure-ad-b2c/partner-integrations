namespace CrossCoreIntegrationApi.Models
{
    using System.Collections.Generic;

    public class CCResponse
    {
        public CCHeader ResponseHeader { get; set; }

        public CCClientResponsePayload ClientResponsePayload { get; set; }

    }

    public class CCClientResponsePayload
    {
        public List<CCDecisionElements> DecisionElements { get; set; }
    }

    public class CCDecisionElements
    {
        public string ServiceName { get; set; }

        public List<CCScoresElement> Scores { get; set; }
    }

    public class CCScoresElement
    {
        public int Score { get; set; }
    }

    public class CCResponseInfo
    {
        public string Decision { get; set; }
    }
}
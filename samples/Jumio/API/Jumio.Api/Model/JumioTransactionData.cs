namespace Jumio.Api.Model
{
    public class JumioTransactionData
    {
        public string TimeStamp { get; set; }
        public string ScanReference { get; set; }

        public JumioDocument Document { get; set; }

        public JumioVerification Verification { get; set; }
    }

    public class JumioDocument
    {
        public string Type { get; set; }
        public string Status { get; set; }
    }

    public class JumioVerification
    {
        public string MrzCheck { get; set; }
        public JumioRejectReason RejectReason { get; set; }
    }

    public class JumioRejectReason
    {
        public string RejectReasonCode { get; set; }

        public string RejectReasonDescription { get; set; }
    }
}

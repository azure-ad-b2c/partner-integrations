namespace Jumio.Api
{
    public class Constants
    {
        public static class IdTokenClaims
        {
            public const string ObjectId = "ObjectId";
        }

        public static class Claims
        {
            public const string ObjectId = "ObjectId";
            public const string TransactionReference = "TransactionReference";
            public const string IsVerified = "IsVerified";
            public const string Message = "Message";
        }

        public static class JumioTransactionStatus
        {
            public const string Pending = "PENDING";
            public const string Done = "DONE";
            public const string Failed = "FAILED";
        }

        public static class JumioDocumentStatus
        {
            public const string ApprovedVerified = "APPROVED_VERIFIED";
        }

        public static class JumioVerificationStatus
        {
            public const string OK = "OK";
        }
    }
}

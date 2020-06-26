namespace Jumio.Api.Model
{
    public class ValidateVerificationTokenInput
    {
        public string ObjectId { get; set; }
        public string TransactionReference { get; set; }
        public string VerificationToken { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(ObjectId)
                && !string.IsNullOrEmpty(TransactionReference)
                && !string.IsNullOrEmpty(VerificationToken);
        }
    }
}

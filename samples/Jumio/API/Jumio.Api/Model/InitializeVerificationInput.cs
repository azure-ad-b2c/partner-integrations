namespace Jumio.Api.Model
{
    public class InitializeVerificationInput
    {
        public string ObjectId { get; set; }
        public string TransactionReference { get; set; }
        public bool Validate()
        {
            return !string.IsNullOrEmpty(ObjectId)
                && !string.IsNullOrEmpty(TransactionReference);
        }
    }
}

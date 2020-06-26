namespace Jumio.Api.Model
{
    public class VerifyTransactionStatusInput
    {
        public string VerificationToken { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(VerificationToken);
        }
    }
}

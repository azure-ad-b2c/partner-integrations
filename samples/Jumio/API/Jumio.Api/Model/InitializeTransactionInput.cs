namespace Jumio.Api.Model
{
    public class InitializeTransactionInput
    {
        public string ObjectId { get; set; }

        public string CorrelationId { get; set; }

        public string ClientId { get; set; }

        public string RedirectUri { get; set; }

        public string Scope { get; set; }

        public string Policy { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(ObjectId)
                && !string.IsNullOrEmpty(CorrelationId)
                && !string.IsNullOrEmpty(ClientId)
                && !string.IsNullOrEmpty(RedirectUri)
                && !string.IsNullOrEmpty(Scope)
                && !string.IsNullOrEmpty(Policy);
        }
    }
}

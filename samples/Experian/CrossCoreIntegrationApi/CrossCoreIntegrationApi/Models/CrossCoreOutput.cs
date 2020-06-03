namespace CrossCoreIntegrationApi.Models
{
    public class CrossCoreOutput : ICrossCoreOutput
    {
        public string FullOutput { get; set; }

        public string Decision { get; set; }

        public string Score { get; set; }
    }
}
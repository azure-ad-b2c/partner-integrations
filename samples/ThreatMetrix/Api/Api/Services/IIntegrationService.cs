namespace Api.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface IIntegrationService
    {
        Task<SessionDataOutput> GetSessionData(SessionQueryServiceInput sessionQueryServiceInput);
    }
}
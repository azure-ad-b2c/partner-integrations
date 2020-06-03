namespace CrossCoreIntegrationApi.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface IService
    {
        Task<ICrossCoreOutput> ServiceCall(ICrossCoreInput input);
    }
}
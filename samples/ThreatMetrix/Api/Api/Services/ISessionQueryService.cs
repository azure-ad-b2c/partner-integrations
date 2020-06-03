namespace Api.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface ISessionQueryService
    {
        Task<string> GetSessionData(IServiceInput sessionQueryServiceInput);
    }
}
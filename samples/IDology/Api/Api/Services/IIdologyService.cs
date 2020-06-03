namespace Api.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface IIdologyService
    {
        Task<IExpectIdOutput> ExpectIdCall(IExpectIdInput expectIdInput);
    }
}
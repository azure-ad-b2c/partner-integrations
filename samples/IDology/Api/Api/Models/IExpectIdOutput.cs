namespace Api.Models
{
    public interface IExpectIdOutput
    {
        string Error { get; }

        bool Success { get; }
    }
}
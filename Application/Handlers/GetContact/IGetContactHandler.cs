namespace Application.Handlers.GetContact;

public interface IGetContactHandler
{
    Task<List<GetContactResult>> ExecuteAsync(string name, CancellationToken ct = default);
}
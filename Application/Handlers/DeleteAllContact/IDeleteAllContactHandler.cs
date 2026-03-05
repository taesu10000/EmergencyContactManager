using Application.Handlers.CreateContact;

namespace Application.Handlers.DeleteAllContact
{
    public interface IDeleteAllContactHandler
    {
        Task<DeleteContactResult> ExecuteAsync(CancellationToken ct = default);
    }
}
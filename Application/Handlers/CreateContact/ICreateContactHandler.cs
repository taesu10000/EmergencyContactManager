namespace Application.Handlers.CreateContact
{
    public interface ICreateContactHandler
    {
        Task<CreateContactResult> ExecuteAsync(CreateContactCommand contact, CancellationToken ct = default);
    }
}
using Application.Handlers.DeleteAllContact;
using Application.Interfaces.Repositories;
using Infrastructure.Services;

namespace Application.Handlers.CreateContact
{
    public class DeleteAllContactHandler : IDeleteAllContactHandler
    {
        private readonly IContactRepository contactRepository;
        public DeleteAllContactHandler(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }
        public async Task<DeleteContactResult> ExecuteAsync(CancellationToken ct = default)
        {
            var affectedCount = await contactRepository.DeleteAllAsync(ct);
            return new DeleteContactResult(affectedCount);
        }
    }
}

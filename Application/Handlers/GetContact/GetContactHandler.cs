using Application.Interfaces.Repositories;

namespace Application.Handlers.GetContact
{
    public class GetContactHandler : IGetContactHandler
    {
        private readonly IContactRepository contactRepository;
        public GetContactHandler(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }
        public async Task<List<GetContactResult>> ExecuteAsync(string name, CancellationToken ct = default)
        {
            var result = await contactRepository.GetAsnyc(name, ct);

            return result.Select(q => new GetContactResult(q.name,
                                                           q.email,
                                                           q.tel)).ToList();
        }
    }
}

using Application.Exceptions;
using Application.Handlers.SearchContact;
using Application.Interfaces.Repositories;
using Application.Resolver;
using Infrastructure.Services;

namespace Application.Handlers.GetContact
{
    public class GetContactHandler : IGetContactHandler
    {
        private readonly IContactParserResolver contactParserResolver;
        private readonly IContactRepository contactRepository;
        private readonly IApplicationTransaction applicationTransaction;
        public GetContactHandler(IContactParserResolver contactParserResolver,
                                    IContactRepository contactRepository,
                                    IApplicationTransaction applicationTransaction)
        {
            this.contactParserResolver = contactParserResolver;
            this.contactRepository = contactRepository;
            this.applicationTransaction = applicationTransaction;
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

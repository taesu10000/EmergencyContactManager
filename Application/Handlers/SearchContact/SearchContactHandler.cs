using Application.Interfaces.Repositories;
using Application.Resolver;
using Infrastructure.Services;

namespace Application.Handlers.SearchContact
{
    public class SearchContactHandler : ISearchContactHandler
    {
        private readonly IContactParserResolver contactParserResolver;
        private readonly IContactRepository contactRepository;
        private readonly IApplicationTransaction applicationTransaction;
        public SearchContactHandler(IContactParserResolver contactParserResolver,
                                    IContactRepository contactRepository,
                                    IApplicationTransaction applicationTransaction)
        {
            this.contactParserResolver = contactParserResolver;
            this.contactRepository = contactRepository;
            this.applicationTransaction = applicationTransaction;
        }
        public async Task<List<SearchContactResult>> ExecuteAsync(SearchContractQuery query, CancellationToken ct = default)
        {
            var list = await contactRepository.GetAsnyc(query.Page, query.PageSize, ct);
            return list.Select(q => new SearchContactResult(q.name,
                                                            q.email,
                                                            q.tel)).ToList();
        }
    }
}

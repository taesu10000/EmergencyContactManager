using Application.Interfaces.Repositories;
using Application.Resolver;
using Infrastructure.Services;

namespace Application.Handlers.SearchContact
{
    public class SearchContactHandler : ISearchContactHandler
    {
        private readonly IContactRepository contactRepository;
        public SearchContactHandler(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }
        public async Task<List<SearchContactResult>> ExecuteAsync(SearchContractQuery query, CancellationToken ct = default)
        {
            var list = await contactRepository.GetAsnyc(query.Page, query.PageSize, ct);
            return list.Select(q => new SearchContactResult(q.name,
                                                            q.email,
                                                            q.tel)).ToList();
        }
        public async Task<List<SearchContactResult>> ExecuteAsync(SearchContactDetailQuery query, CancellationToken ct = default)
        {
            var list = await contactRepository.GetAsnyc(query.q, query.Name, query.Email, query.Tel, query.Joined, query.Page, query.PageSize, ct);
            return list.Select(q => new SearchContactResult(q.name,
                                                            q.email,
                                                            q.tel)).ToList();
        }
    }
}

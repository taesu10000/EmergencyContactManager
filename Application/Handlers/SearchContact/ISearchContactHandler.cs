namespace Application.Handlers.SearchContact
{
    public interface ISearchContactHandler
    {
        Task<List<SearchContactResult>> ExecuteAsync(SearchContractQuery query, CancellationToken ct = default);
        Task<List<SearchContactResult>> ExecuteAsync(SearchContactDetailQuery query, CancellationToken ct = default);
    }
}
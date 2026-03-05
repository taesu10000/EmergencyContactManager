using Domain;

namespace Application.Interfaces.Repositories
{
    public interface IContactRepository
    {
        Task<Contact> CreateAsync(Contact contact, CancellationToken ct = default);
        Task CreateAsync(List<Contact> contacts, CancellationToken ct = default);
        Task<int> DeleteAllAsync(CancellationToken ct = default);
        Task<List<Contact>> GetAsnyc(int page, int pageSize, CancellationToken ct = default);
        Task<List<Contact>> GetAsnyc(string name, CancellationToken ct = default);
    }
}

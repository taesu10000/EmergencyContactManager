using Application.Interfaces.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Infrastructure.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly DBContext dBContext;
        public ContactRepository(DBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Contact> CreateAsync(Contact contact, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            await dBContext.Contacts.AddAsync(contact);
            return contact;
        }

        public async Task CreateAsync(List<Contact> contacts, CancellationToken ct = default)
        {
            await dBContext.Contacts.AddRangeAsync(contacts, ct);
        }

        public async Task<int> DeleteAllAsync(CancellationToken ct = default)
        {
            return await dBContext.Contacts.ExecuteDeleteAsync(ct);
        }

        public async Task<List<Contact>> GetAsnyc(string name, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await dBContext.Contacts.Where(q => q.name == name).ToListAsync(ct);
        }
        public async Task<List<Contact>> GetAsnyc(int page, int pageSize, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var query = dBContext.Contacts.AsQueryable();
            query = query.OrderBy(q => q.name).ThenBy(q => q.email).ThenBy(q => q.tel);
            query = query.Skip((page -1) * pageSize).Take(pageSize);
            return await query.ToListAsync(ct);
        }
    }
}

using Application.Interfaces.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

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
        public async Task<List<Contact>> GetAsnyc(int? page, int? pageSize, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var query = dBContext.Contacts.OrderBy(q => q.name)
                                          .ThenBy(q => q.email)
                                          .ThenBy(q => q.tel)
                                          .AsQueryable();

            if (page.HasValue && pageSize.HasValue)
            {
                page = page == 0 ? 1 : page;
                pageSize = pageSize == 0 ? 10 : pageSize;
                var skip = (page.Value - 1) * pageSize.Value;

                query = query
                    .Skip(skip)
                    .Take(pageSize.Value);
            }

            return await query.ToListAsync(ct);
        }

        public async Task<List<Contact>> GetAsnyc(string q, string? name, string? email, string? tel, DateTimeOffset? joined, int? page, int? pageSize, CancellationToken ct = default)
        {
            var query = dBContext.Contacts.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var tokens = q.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var token in tokens)
                {
                    var pattern = $"%{token}%";

                    query = query.Where(x =>
                        (x.name != null && EF.Functions.ILike(x.name, pattern))
                        || (x.email != null && EF.Functions.ILike(x.email, pattern))
                        || (x.tel != null && EF.Functions.ILike(x.tel, pattern)));
                }
            }
            query = query.OrderBy(x => x.name).ThenBy(x => x.email).ThenBy(x => x.tel);
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.name == name);
            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(x => x.email.Contains(email));
            if (!string.IsNullOrWhiteSpace(tel))
                query = query.Where(x => x.tel.Contains(tel));
            if (joined is not null)
                query = query.Where(x => x.joined == joined);


            if (page.HasValue && pageSize.HasValue)
            {
                page = page == 0 ? 1 : page;
                pageSize = pageSize == 0 ? 10 : pageSize;
                var skip = (page.Value - 1) * pageSize.Value;

                query = query
                    .Skip(skip)
                    .Take(pageSize.Value);
            }

            return await query.ToListAsync(ct);

        }
    }
}

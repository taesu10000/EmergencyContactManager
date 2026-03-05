using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Resolver;
using Domain;
using Infrastructure.Services;

namespace Application.Handlers.CreateContact
{
    public class CreateContactHandler : ICreateContactHandler
    {
        private readonly IContactParserResolver contactParserResolver;
        private readonly IContactRepository contactRepository;
        private readonly IApplicationTransaction applicationTransaction;
        public CreateContactHandler(IContactParserResolver contactParserResolver, IContactRepository contactRepository, IApplicationTransaction applicationTransaction)
        {
            this.contactParserResolver = contactParserResolver;
            this.contactRepository = contactRepository;
            this.applicationTransaction = applicationTransaction;
        }
        public async Task<CreateContactResult> ExecuteAsync(CreateContactCommand cmd, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(cmd.FileRaws))
                throw new CreateContactException();

            var parser = contactParserResolver.Resolve(cmd.FileRaws);
            var objs = parser.Deserialize<Contact>(cmd.FileRaws);

            await contactRepository.CreateAsync(objs, ct);
            var affected = await applicationTransaction.SaveChangesAsync();
            return new CreateContactResult(affectedCount: affected );
        }
    }
}

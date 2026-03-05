using Application.Exceptions;
using Application.Interfaces.Services;
using Infrastructure.Parsers;

namespace Application.Resolver
{
    public sealed class ContactParserResolver : IContactParserResolver
    {
        private readonly IEnumerable<IParsingService> parsers;
        public ContactParserResolver(IEnumerable<IParsingService> parsers) => this.parsers = parsers;

        public IParsingService Resolve(string content)
            => parsers.FirstOrDefault(p => p.CanParse(content))
               ?? throw new UnsupportedFormatException();
    }
}

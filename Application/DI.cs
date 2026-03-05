using Application.Handlers.CreateContact;
using Application.Handlers.DeleteAllContact;
using Application.Handlers.GetContact;
using Application.Handlers.SearchContact;
using Application.Resolver;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public class DI
{
    public static void AddDI(IServiceCollection services)
    {
        services.AddScoped<IContactParserResolver, ContactParserResolver>();

        services.AddScoped<IGetContactHandler, GetContactHandler>();
        services.AddScoped<ICreateContactHandler, CreateContactHandler>();
        services.AddScoped<ISearchContactHandler, SearchContactHandler>();
        services.AddScoped<IDeleteAllContactHandler, DeleteAllContactHandler>();
    }
}

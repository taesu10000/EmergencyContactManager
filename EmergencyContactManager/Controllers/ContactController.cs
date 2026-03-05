using Application.Handlers.CreateContact;
using Application.Handlers.DeleteAllContact;
using Application.Handlers.GetContact;
using Application.Handlers.SearchContact;
using EmergencyContactManager.Factories;
using EmergencyContactManager.Models.Request;
using EmergencyContactManager.Models.Response;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/employee")]
public class ContactController : ControllerBase
{
    private readonly ICreateContactHandler createContactHandler;
    private readonly IGetContactHandler getContactHandler;
    private readonly ISearchContactHandler searchContactHandler;
    private readonly IDeleteAllContactHandler deleteAllContactHandler;
    private readonly ICreateContactCommandFactory createContactCommandFactory;

    public ContactController(ICreateContactHandler createContactHandler,
                             IGetContactHandler getContactHandler,
                             ISearchContactHandler searchContactHandler,
                             IDeleteAllContactHandler deleteAllContactHandler,
                             ICreateContactCommandFactory createContactCommandFactory)
    {
        this.createContactHandler = createContactHandler;
        this.getContactHandler = getContactHandler;
        this.searchContactHandler = searchContactHandler;
        this.deleteAllContactHandler = deleteAllContactHandler;
        this.createContactCommandFactory = createContactCommandFactory;
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] ContactCreateRequest contactCreateRequest, CancellationToken ct)
    {
        var cmd = await createContactCommandFactory.ReadContentAsync(contactCreateRequest, ct);
        var result = await createContactHandler.ExecuteAsync(cmd, ct);

        return Ok(new CreateContactResponse(result.affectedCount));
    }
    [HttpDelete("all")]
    public async Task<IActionResult> DeleteAsync(CancellationToken ct)
    {
        var result = await deleteAllContactHandler.ExecuteAsync(ct);
        return Ok(new DeleteAllResponse(result.affectedCount));
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetAsync(string name, CancellationToken ct)
    {
        var result = await getContactHandler.ExecuteAsync(name);

        return Ok(result.Select(r => new GetContactResponse(Name: r.Name, Email: r.Email, Tel: r.Tel)).ToList());
    }
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] int page, int pageSize, CancellationToken ct)
    {
        var query = new SearchContractQuery(page, pageSize);
        var result = await searchContactHandler.ExecuteAsync(query);

        return Ok(result.Select(r => new GetContactResponse(Name: r.Name, Email: r.Email, Tel: r.Tel)).ToList());
    }
}

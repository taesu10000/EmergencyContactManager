using Application.Handlers.CreateContact;
using EmergencyContactManager.Models.Request;

namespace EmergencyContactManager.Factories
{
    public sealed class CreateContactCommandFactory : ICreateContactCommandFactory
    {
        public async Task<CreateContactCommand> ReadContentAsync(ContactCreateRequest req, CancellationToken ct)
        {
            var content = string.Empty;
            if (req.File is not null && req.File.Length > 0)
            {
                // HTTP lifetime 문제 피하려면 복사해서 넘기는 걸 권장
                using var reader = new StreamReader(req.File.OpenReadStream(), leaveOpen: false);
                content = await reader.ReadToEndAsync(ct);
            }
            else if (!string.IsNullOrEmpty(req.Raw))
            {
                content = req.Raw;
            }
            return new CreateContactCommand(content);
        }
    }

}
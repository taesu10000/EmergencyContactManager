using Domain;

namespace Application.Interfaces.Services
{
    public interface IParsingService
    {
        FileType Format { get; }
        bool CanParse(string content);
        List<T> Deserialize<T>(string content) where T : class;
    }
}
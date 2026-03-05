using Application.Interfaces.Services;
using Domain;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Parsers;

public class JasonParser : IParsingService
{
    public FileType Format { get; } = FileType.JSON;
    public bool CanParse(string content)
    {
		try
		{
            JsonDocument.Parse(content);
            return true;
        }
		catch (Exception)
		{
            return false;
		}
    }

    public List<T> Deserialize<T>(string content) where T : class
    {
        var document = JsonDocument.Parse(content);
        var result = document.Deserialize<List<T>>() ?? new List<T>();
        return result;
    }
}

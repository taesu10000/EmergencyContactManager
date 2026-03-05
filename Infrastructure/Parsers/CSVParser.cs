using Application.Exceptions;
using Application.Interfaces.Services;
using CsvHelper;
using CsvHelper.Configuration;
using Domain;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;


namespace Infrastructure.Parsers;

public class CSVParser : IParsingService
{
    public FileType Format { get; } = FileType.CSV;
    public bool CanParse(string content)
    {
		try
		{
            using var reader = new StringReader(content);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            while (csv.Read())
            {
                var fieldCount = csv.Parser.Count; 
                var record = csv.Parser.Record;
            }
            return true;
        }
		catch (Exception)
		{
            return false;
		}
    }

    public List<T> Deserialize<T>(string content) where T : class
    {
        var hasHeader = HasHeader<T>(content);

        if (!DeserializationValidation<T>(content))
            throw new DeserializationException();

        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = hasHeader,
            TrimOptions = TrimOptions.Trim,
        };
        using var reader = new StringReader(content);
        using var csv = new CsvReader(reader, cfg);
        var records = csv.GetRecords<T>();
        return records.ToList();
    }
    public bool DeserializationValidation<T>(string content) where T : class
    {
        using var reader = new StringReader(content);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Read();
        csv.ReadHeader();
        var type = typeof(T);
        var properties = type.GetProperties();

        if (csv.HeaderRecord?.Count() != properties.Length)
            return false;

        return true;
    }
    public bool HasHeader<T>(string content) where T : class
    {
        using var reader = new StringReader(content);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Read();
        csv.ReadHeader();

        if (csv.HeaderRecord is null || csv.HeaderRecord.Length == 0)
            return false;

        var type = typeof(T);
        var properties = type.GetProperties();

        if (csv.HeaderRecord?.Count() != properties.Length)
            return false;

        var headerSet = csv.HeaderRecord
            .Select(q => q.ToLower())
            .Where(s => s.Length > 0)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var props = typeof(T)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanWrite)
            .Select(p => p.Name.ToLower())
            .Where(x => x.Length > 0)
            .ToArray();

        return  props.All(p => headerSet.Contains(p));
    }
}

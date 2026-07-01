using System.Text.Json;
using FileReaderLibrary.Abstractions;

namespace FileReaderLibrary.Readers;

public sealed class JsonFileReader : IFileReader
{
    public string Read(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var content = File.ReadAllText(path);
        using var _ = JsonDocument.Parse(content);

        return content;
    }
}

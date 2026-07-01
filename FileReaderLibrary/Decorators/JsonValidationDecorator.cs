using System.Text.Json;
using FileReaderLibrary.Abstractions;

namespace FileReaderLibrary.Decorators;

public sealed class JsonValidationDecorator(IFileReader innerReader) : IFileReader
{
    private readonly IFileReader _innerReader = innerReader
        ?? throw new ArgumentNullException(nameof(innerReader));

    public string Read(string path)
    {
        var content = _innerReader.Read(path);
        using var _ = JsonDocument.Parse(content);

        return content;
    }
}

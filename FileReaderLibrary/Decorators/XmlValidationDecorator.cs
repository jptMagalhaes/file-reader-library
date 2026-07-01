using System.Xml.Linq;
using FileReaderLibrary.Abstractions;

namespace FileReaderLibrary.Decorators;

public sealed class XmlValidationDecorator(IFileReader innerReader) : IFileReader
{
    private readonly IFileReader _innerReader = innerReader
        ?? throw new ArgumentNullException(nameof(innerReader));

    public string Read(string path)
    {
        var content = _innerReader.Read(path);
        _ = XDocument.Parse(content);

        return content;
    }
}

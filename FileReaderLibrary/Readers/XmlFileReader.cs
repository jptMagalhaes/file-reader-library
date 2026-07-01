using System.Xml.Linq;
using FileReaderLibrary.Abstractions;

namespace FileReaderLibrary.Readers;

public sealed class XmlFileReader : IFileReader
{
    public string Read(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var content = File.ReadAllText(path);
        _ = XDocument.Parse(content);

        return content;
    }
}

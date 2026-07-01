using FileReaderLibrary.Abstractions;

namespace FileReaderLibrary.Readers;

public sealed class TextFileReader : IFileReader
{
    public string Read(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        return File.ReadAllText(path);
    }
}

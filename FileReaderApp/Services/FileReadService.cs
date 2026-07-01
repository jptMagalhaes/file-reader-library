using FileReaderLibrary.Abstractions;

namespace FileReaderApp.Services;

public sealed class FileReadService(IFileReader reader)
{
    private readonly IFileReader _reader = reader;

    public string Read(string path) => _reader.Read(path);
}

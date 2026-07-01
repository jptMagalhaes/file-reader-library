using FileReaderLibrary.Abstractions;
using FileReaderLibrary.Models;

namespace FileReaderLibrary.Decorators;

public sealed class RoleBasedFileReaderDecorator(
    IFileReader innerReader,
    IFileAccessPolicy accessPolicy,
    UserRole role) : IFileReader
{
    private readonly IFileReader _innerReader = innerReader
        ?? throw new ArgumentNullException(nameof(innerReader));
    private readonly IFileAccessPolicy _accessPolicy = accessPolicy
        ?? throw new ArgumentNullException(nameof(accessPolicy));

    public string Read(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        if (!_accessPolicy.CanRead(path, role))
        {
            throw new UnauthorizedAccessException(
                $"Role '{role}' is not allowed to read '{path}'.");
        }

        return _innerReader.Read(path);
    }
}

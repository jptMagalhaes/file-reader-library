using FileReaderLibrary.Abstractions;
using FileReaderLibrary.Models;

namespace FileReaderLibrary.Infrastructure;

public sealed class ConfigurableFileAccessPolicy(IEnumerable<string>? viewerAllowedFileNames = null)
    : IFileAccessPolicy
{
    private readonly HashSet<string> _viewerAllowedFileNames = new(
        viewerAllowedFileNames ?? [],
        StringComparer.OrdinalIgnoreCase);

    public bool CanRead(string path, UserRole role)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        return role switch
        {
            UserRole.Admin => true,
            UserRole.Viewer => _viewerAllowedFileNames.Contains(Path.GetFileName(path)),
            _ => false
        };
    }
}

using FileReaderLibrary.Models;

namespace FileReaderLibrary.Abstractions;

public interface IFileAccessPolicy
{
    bool CanRead(string path, UserRole role);
}

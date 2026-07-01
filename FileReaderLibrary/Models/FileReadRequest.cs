namespace FileReaderLibrary.Models;

public sealed class FileReadRequest
{
    public required string Path { get; init; }

    public required FileFormat Format { get; init; }

    public bool UseEncryption { get; init; }

    public bool UseRoleSecurity { get; init; }

    public UserRole Role { get; init; } = UserRole.Admin;
}

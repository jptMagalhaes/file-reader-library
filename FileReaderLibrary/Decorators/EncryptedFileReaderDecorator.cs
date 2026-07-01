using FileReaderLibrary.Abstractions;

namespace FileReaderLibrary.Decorators;

public sealed class EncryptedFileReaderDecorator(
    IFileReader innerReader,
    IEncryptionAlgorithm encryptionAlgorithm) : IFileReader
{
    private readonly IFileReader _innerReader = innerReader 
        ?? throw new ArgumentNullException(nameof(innerReader));
    private readonly IEncryptionAlgorithm _encryptionAlgorithm = encryptionAlgorithm
            ?? throw new ArgumentNullException(nameof(encryptionAlgorithm));

    public string Read(string path)
    {
        var encryptedContent = _innerReader.Read(path);
        return _encryptionAlgorithm.Decrypt(encryptedContent);
    }
}

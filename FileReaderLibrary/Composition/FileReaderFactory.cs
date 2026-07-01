using FileReaderLibrary.Abstractions;
using FileReaderLibrary.Decorators;
using FileReaderLibrary.Models;
using FileReaderLibrary.Readers;

namespace FileReaderLibrary.Composition;

public sealed class FileReaderFactory(
    IEncryptionAlgorithm encryptionAlgorithm,
    IFileAccessPolicy accessPolicy)
{
    private readonly IEncryptionAlgorithm _encryptionAlgorithm = encryptionAlgorithm
        ?? throw new ArgumentNullException(nameof(encryptionAlgorithm));
    private readonly IFileAccessPolicy _accessPolicy = accessPolicy
        ?? throw new ArgumentNullException(nameof(accessPolicy));

    public IFileReader Create(FileReadRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        IFileReader reader = request.Format switch
        {
            FileFormat.Text => new TextFileReader(),
            FileFormat.Xml when request.UseEncryption => new TextFileReader(),
            FileFormat.Xml => new XmlFileReader(),
            FileFormat.Json when request.UseEncryption => new TextFileReader(),
            FileFormat.Json => new JsonFileReader(),
            _ => throw new ArgumentOutOfRangeException(nameof(request), request.Format, "Unsupported file format.")
        };

        if (request.UseEncryption)
        {
            reader = new EncryptedFileReaderDecorator(reader, _encryptionAlgorithm);

            if (request.Format == FileFormat.Xml)
            {
                reader = new XmlValidationDecorator(reader);
            }

            if (request.Format == FileFormat.Json)
            {
                reader = new JsonValidationDecorator(reader);
            }
        }

        if (request.UseRoleSecurity)
        {
            reader = new RoleBasedFileReaderDecorator(reader, _accessPolicy, request.Role);
        }

        return reader;
    }
}

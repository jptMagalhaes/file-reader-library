using FileReaderLibrary.Abstractions;

namespace FileReaderLibrary.Infrastructure;

public sealed class ReverseEncryptionAlgorithm : IEncryptionAlgorithm
{
    public string Encrypt(string plainText)
    {
        ArgumentNullException.ThrowIfNull(plainText);
        return new string([.. plainText.Reverse()]);
    }

    public string Decrypt(string cipherText) => Encrypt(cipherText);
}

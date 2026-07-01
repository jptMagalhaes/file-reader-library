namespace FileReaderLibrary.Abstractions;

public interface IEncryptionAlgorithm
{
    string Encrypt(string plainText);

    string Decrypt(string cipherText);
}

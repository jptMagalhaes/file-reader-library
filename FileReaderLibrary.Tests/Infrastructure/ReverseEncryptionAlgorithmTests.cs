using FileReaderLibrary.Infrastructure;

namespace FileReaderLibrary.Tests.Infrastructure;

public class ReverseEncryptionAlgorithmTests
{
    private readonly ReverseEncryptionAlgorithm _algorithm = new();

    [Fact]
    public void Encrypt_ReversesPlainText()
    {
        const string plainText = "hello";

        var encrypted = _algorithm.Encrypt(plainText);

        Assert.Equal("olleh", encrypted);
    }

    [Fact]
    public void Decrypt_ReversesCipherTextBackToPlainText()
    {
        const string plainText = "Hello from FileReaderLibrary v3.";

        var encrypted = _algorithm.Encrypt(plainText);
        var decrypted = _algorithm.Decrypt(encrypted);

        Assert.Equal(plainText, decrypted);
    }

    [Fact]
    public void Encrypt_WhenPlainTextIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _algorithm.Encrypt(null!));
    }

    [Fact]
    public void Decrypt_WhenCipherTextIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _algorithm.Decrypt(null!));
    }

    [Fact]
    public void Encrypt_WhenPlainTextIsEmpty_ReturnsEmptyString()
    {
        var encrypted = _algorithm.Encrypt(string.Empty);

        Assert.Equal(string.Empty, encrypted);
    }
}

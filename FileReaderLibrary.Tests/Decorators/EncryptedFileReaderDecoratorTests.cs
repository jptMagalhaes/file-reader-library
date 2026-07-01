using FileReaderLibrary.Abstractions;
using FileReaderLibrary.Decorators;
using FileReaderLibrary.Infrastructure;
using FileReaderLibrary.Readers;

namespace FileReaderLibrary.Tests.Decorators;

public class EncryptedFileReaderDecoratorTests
{
    [Fact]
    public void Read_WhenFileContainsEncryptedText_ReturnsDecryptedContent()
    {
        var path = Path.GetTempFileName();
        var algorithm = new ReverseEncryptionAlgorithm();
        var reader = new EncryptedFileReaderDecorator(new TextFileReader(), algorithm);

        try
        {
            const string plainText = "secret message";
            File.WriteAllText(path, algorithm.Encrypt(plainText));

            var result = reader.Read(path);

            Assert.Equal(plainText, result);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Read_SampleEncryptedHelloFile_ReturnsDecryptedContent()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "samples", "hello.encrypted.txt");
        var reader = new EncryptedFileReaderDecorator(
            new TextFileReader(),
            new ReverseEncryptionAlgorithm());

        var result = reader.Read(path);

        Assert.Contains("Hello from FileReaderLibrary v3.", result);
    }

    [Fact]
    public void Read_WhenInnerReaderFails_PropagatesException()
    {
        var reader = new EncryptedFileReaderDecorator(
            new TextFileReader(),
            new ReverseEncryptionAlgorithm());
        var missingPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        Assert.Throws<FileNotFoundException>(() => reader.Read(missingPath));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Read_WhenPathIsInvalid_PropagatesArgumentExceptionFromInnerReader(string? path)
    {
        var reader = new EncryptedFileReaderDecorator(
            new TextFileReader(),
            new ReverseEncryptionAlgorithm());

        Assert.ThrowsAny<ArgumentException>(() => reader.Read(path!));
    }

    [Fact]
    public void Constructor_WhenInnerReaderIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new EncryptedFileReaderDecorator(null!, new ReverseEncryptionAlgorithm()));
    }

    [Fact]
    public void Constructor_WhenEncryptionAlgorithmIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new EncryptedFileReaderDecorator(new TextFileReader(), null!));
    }
}

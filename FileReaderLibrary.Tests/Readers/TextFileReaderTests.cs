using FileReaderLibrary.Readers;

namespace FileReaderLibrary.Tests.Readers;

public class TextFileReaderTests
{
    private readonly TextFileReader _reader = new();

    [Fact]
    public void Read_WhenFileExists_ReturnsFullContent()
    {
        var path = Path.GetTempFileName();
        try
        {
            const string expected = "line one\nline two";
            File.WriteAllText(path, expected);

            var result = _reader.Read(path);

            Assert.Equal(expected, result);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Read_SampleHelloFile_ReturnsContent()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "samples", "hello.txt");

        var result = _reader.Read(path);

        Assert.Contains("Hello from FileReaderLibrary v1", result);
    }

    [Fact]
    public void Read_WhenFileMissing_ThrowsFileNotFoundException()
    {
        var missingPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        Assert.Throws<FileNotFoundException>(() => _reader.Read(missingPath));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Read_WhenPathIsInvalid_ThrowsArgumentException(string? path)
    {
        Assert.ThrowsAny<ArgumentException>(() => _reader.Read(path!));
    }
}

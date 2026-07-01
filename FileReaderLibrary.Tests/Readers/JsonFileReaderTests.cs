using System.Text.Json;
using FileReaderLibrary.Readers;

namespace FileReaderLibrary.Tests.Readers;

public class JsonFileReaderTests
{
    private readonly JsonFileReader _reader = new();

    [Fact]
    public void Read_WhenJsonIsValid_ReturnsFullContent()
    {
        var path = Path.GetTempFileName();
        try
        {
            const string expected = """{"message":"line one"}""";
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
    public void Read_SampleHelloJson_ReturnsContent()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "samples", "hello.json");

        var result = _reader.Read(path);

        Assert.Contains("\"message\": \"Hello from FileReaderLibrary v7.\"", result);
    }

    [Fact]
    public void Read_WhenJsonIsMalformed_ThrowsJsonException()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, """{"message":""");

            Assert.ThrowsAny<JsonException>(() => _reader.Read(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Read_WhenFileIsEmpty_ThrowsJsonException()
    {
        var path = Path.GetTempFileName();
        try
        {
            Assert.ThrowsAny<JsonException>(() => _reader.Read(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Read_WhenFileMissing_ThrowsFileNotFoundException()
    {
        var missingPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.json");

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

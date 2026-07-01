using System.Xml;
using FileReaderLibrary.Readers;

namespace FileReaderLibrary.Tests.Readers;

public class XmlFileReaderTests
{
    private readonly XmlFileReader _reader = new();

    [Fact]
    public void Read_WhenXmlIsWellFormed_ReturnsFullContent()
    {
        var path = Path.GetTempFileName();
        try
        {
            const string expected = "<root><message>line one</message></root>";
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
    public void Read_SampleHelloXml_ReturnsContent()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "samples", "hello.xml");

        var result = _reader.Read(path);

        Assert.Contains("<message>Hello from FileReaderLibrary v2.</message>", result);
    }

    [Fact]
    public void Read_WhenXmlIsMalformed_ThrowsXmlException()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, "<root><unclosed>");

            Assert.Throws<XmlException>(() => _reader.Read(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Read_WhenFileMissing_ThrowsFileNotFoundException()
    {
        var missingPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.xml");

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

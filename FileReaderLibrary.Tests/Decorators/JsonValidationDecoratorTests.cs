using System.Text.Json;
using FileReaderLibrary.Decorators;
using FileReaderLibrary.Readers;

namespace FileReaderLibrary.Tests.Decorators;

public class JsonValidationDecoratorTests
{
    [Fact]
    public void Read_WhenContentIsValidJson_ReturnsContent()
    {
        var path = Path.GetTempFileName();
        var reader = new JsonValidationDecorator(new TextFileReader());

        try
        {
            const string json = """{"message":"well formed"}""";
            File.WriteAllText(path, json);

            var result = reader.Read(path);

            Assert.Equal(json, result);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Read_WhenContentIsMalformedJson_ThrowsJsonException()
    {
        var path = Path.GetTempFileName();
        var reader = new JsonValidationDecorator(new TextFileReader());

        try
        {
            File.WriteAllText(path, """{"message":""");

            Assert.ThrowsAny<JsonException>(() => reader.Read(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Constructor_WhenInnerReaderIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new JsonValidationDecorator(null!));
    }
}

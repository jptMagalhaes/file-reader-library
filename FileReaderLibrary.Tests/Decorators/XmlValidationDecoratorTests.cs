using System.Xml;
using FileReaderLibrary.Decorators;
using FileReaderLibrary.Readers;

namespace FileReaderLibrary.Tests.Decorators;

public class XmlValidationDecoratorTests
{
    [Fact]
    public void Read_WhenContentIsWellFormedXml_ReturnsContent()
    {
        var path = Path.GetTempFileName();
        var reader = new XmlValidationDecorator(new TextFileReader());

        try
        {
            const string xml = "<root><message>well formed</message></root>";
            File.WriteAllText(path, xml);

            var result = reader.Read(path);

            Assert.Equal(xml, result);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Read_WhenContentIsMalformedXml_ThrowsXmlException()
    {
        var path = Path.GetTempFileName();
        var reader = new XmlValidationDecorator(new TextFileReader());

        try
        {
            File.WriteAllText(path, "<root><unclosed>");

            Assert.Throws<XmlException>(() => reader.Read(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Constructor_WhenInnerReaderIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new XmlValidationDecorator(null!));
    }
}

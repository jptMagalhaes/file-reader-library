using FileReaderLibrary.Decorators;
using FileReaderLibrary.Infrastructure;
using FileReaderLibrary.Models;
using FileReaderLibrary.Readers;

namespace FileReaderLibrary.Tests.Decorators;

public class RoleBasedFileReaderDecoratorTests
{
    [Fact]
    public void Read_WhenRoleIsAdmin_ReturnsXmlContent()
    {
        var path = Path.GetTempFileName();
        var reader = new RoleBasedFileReaderDecorator(
            new XmlFileReader(),
            new ConfigurableFileAccessPolicy(),
            UserRole.Admin);

        try
        {
            const string xml = "<root><message>admin access</message></root>";
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
    public void Read_WhenViewerAccessIsDenied_ThrowsUnauthorizedAccessException()
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.xml");
        var reader = new RoleBasedFileReaderDecorator(
            new XmlFileReader(),
            new ConfigurableFileAccessPolicy(["hello.rbac.xml"]),
            UserRole.Viewer);

        Assert.Throws<UnauthorizedAccessException>(() => reader.Read(path));
    }

    [Fact]
    public void Read_SampleRbacXmlAsViewer_ReturnsContent()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "samples", "hello.rbac.xml");
        var reader = new RoleBasedFileReaderDecorator(
            new XmlFileReader(),
            new ConfigurableFileAccessPolicy(["hello.rbac.xml"]),
            UserRole.Viewer);

        var result = reader.Read(path);

        Assert.Contains("<message>Hello from FileReaderLibrary v4.</message>", result);
    }

    [Fact]
    public void Read_WhenInnerReaderFails_PropagatesException()
    {
        var reader = new RoleBasedFileReaderDecorator(
            new XmlFileReader(),
            new ConfigurableFileAccessPolicy(),
            UserRole.Admin);
        var missingPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.xml");

        Assert.Throws<FileNotFoundException>(() => reader.Read(missingPath));
    }

    [Fact]
    public void Constructor_WhenInnerReaderIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new RoleBasedFileReaderDecorator(null!, new ConfigurableFileAccessPolicy(), UserRole.Admin));
    }

    [Fact]
    public void Constructor_WhenAccessPolicyIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new RoleBasedFileReaderDecorator(new XmlFileReader(), null!, UserRole.Admin));
    }
}

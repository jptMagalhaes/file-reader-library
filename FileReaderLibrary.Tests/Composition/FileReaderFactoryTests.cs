using System.Xml;
using FileReaderLibrary.Composition;
using FileReaderLibrary.Infrastructure;
using FileReaderLibrary.Models;

namespace FileReaderLibrary.Tests.Composition;

public class FileReaderFactoryTests
{
    private readonly FileReaderFactory _factory = new(
        new ReverseEncryptionAlgorithm(),
        new ConfigurableFileAccessPolicy(["hello.rbac.xml"]));

    [Fact]
    public void Create_WhenEncryptedXmlRequested_ReadsDecryptedXml()
    {
        var path = Path.GetTempFileName();
        var algorithm = new ReverseEncryptionAlgorithm();
        var reader = _factory.Create(new FileReadRequest
        {
            Path = path,
            Format = FileFormat.Xml,
            UseEncryption = true
        });

        try
        {
            const string xml = "<root><message>encrypted xml</message></root>";
            File.WriteAllText(path, algorithm.Encrypt(xml));

            var result = reader.Read(path);

            Assert.Equal(xml, result);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Create_SampleEncryptedXml_ReturnsDecryptedContent()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "samples", "hello.encrypted.xml");
        var reader = _factory.Create(new FileReadRequest
        {
            Path = path,
            Format = FileFormat.Xml,
            UseEncryption = true
        });

        var result = reader.Read(path);

        Assert.Contains("<message>Hello from FileReaderLibrary v5.</message>", result);
    }

    [Fact]
    public void Create_WhenEncryptionDisabled_ReadsRawXml()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "samples", "hello.xml");
        var reader = _factory.Create(new FileReadRequest
        {
            Path = path,
            Format = FileFormat.Xml,
            UseEncryption = false
        });

        var result = reader.Read(path);

        Assert.Contains("<message>Hello from FileReaderLibrary v2.</message>", result);
    }

    [Fact]
    public void Create_WhenEncryptedXmlIsMalformed_ThrowsXmlException()
    {
        var path = Path.GetTempFileName();
        var algorithm = new ReverseEncryptionAlgorithm();
        var reader = _factory.Create(new FileReadRequest
        {
            Path = path,
            Format = FileFormat.Xml,
            UseEncryption = true
        });

        try
        {
            File.WriteAllText(path, algorithm.Encrypt("<root><unclosed>"));

            Assert.Throws<XmlException>(() => reader.Read(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Create_WhenRoleSecurityEnabled_EnforcesAccessPolicy()
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.xml");
        var reader = _factory.Create(new FileReadRequest
        {
            Path = path,
            Format = FileFormat.Xml,
            UseRoleSecurity = true,
            Role = UserRole.Viewer
        });

        Assert.Throws<UnauthorizedAccessException>(() => reader.Read(path));
    }

    [Fact]
    public void Create_WhenTextRequested_ReadsPlainText()
    {
        var path = Path.GetTempFileName();
        var reader = _factory.Create(new FileReadRequest
        {
            Path = path,
            Format = FileFormat.Text
        });

        try
        {
            const string expected = "plain text";
            File.WriteAllText(path, expected);

            var result = reader.Read(path);

            Assert.Equal(expected, result);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Create_WhenEncryptedTextRequested_ReadsDecryptedText()
    {
        var path = Path.GetTempFileName();
        var algorithm = new ReverseEncryptionAlgorithm();
        var reader = _factory.Create(new FileReadRequest
        {
            Path = path,
            Format = FileFormat.Text,
            UseEncryption = true
        });

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
    public void Create_WhenXmlEncryptedAndRoleSecurityAllEnabled_ComposesAllDecoratorsAndSucceeds()
    {
        var path = Path.GetTempFileName();
        var algorithm = new ReverseEncryptionAlgorithm();
        var reader = _factory.Create(new FileReadRequest
        {
            Path = path,
            Format = FileFormat.Xml,
            UseEncryption = true,
            UseRoleSecurity = true,
            Role = UserRole.Admin
        });

        try
        {
            const string xml = "<root><message>full stack</message></root>";
            File.WriteAllText(path, algorithm.Encrypt(xml));

            var result = reader.Read(path);

            Assert.Equal(xml, result);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Create_WhenXmlEncryptedAndRoleSecurityAllEnabled_DeniesBeforeReadingWhenUnauthorized()
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.xml");
        var reader = _factory.Create(new FileReadRequest
        {
            Path = path,
            Format = FileFormat.Xml,
            UseEncryption = true,
            UseRoleSecurity = true,
            Role = UserRole.Viewer
        });

        Assert.Throws<UnauthorizedAccessException>(() => reader.Read(path));
    }

    [Fact]
    public void Create_WhenFormatIsUnsupported_ThrowsArgumentOutOfRangeException()
    {
        var reader = () => _factory.Create(new FileReadRequest
        {
            Path = "irrelevant.dat",
            Format = (FileFormat)99
        });

        Assert.Throws<ArgumentOutOfRangeException>(reader);
    }

    [Fact]
    public void Create_WhenRequestIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _factory.Create(null!));
    }

    [Fact]
    public void Constructor_WhenEncryptionAlgorithmIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new FileReaderFactory(null!, new ConfigurableFileAccessPolicy()));
    }

    [Fact]
    public void Constructor_WhenAccessPolicyIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new FileReaderFactory(new ReverseEncryptionAlgorithm(), null!));
    }
}

using FileReaderLibrary.Infrastructure;
using FileReaderLibrary.Models;

namespace FileReaderLibrary.Tests.Infrastructure;

public class ConfigurableFileAccessPolicyTests
{
    [Fact]
    public void CanRead_WhenRoleIsAdmin_ReturnsTrueForAnyPath()
    {
        var policy = new ConfigurableFileAccessPolicy();

        var canRead = policy.CanRead(@"C:\secrets\payroll.xml", UserRole.Admin);

        Assert.True(canRead);
    }

    [Fact]
    public void CanRead_WhenRoleIsViewerAndFileIsAllowed_ReturnsTrue()
    {
        var policy = new ConfigurableFileAccessPolicy(["hello.rbac.xml"]);

        var canRead = policy.CanRead(@"C:\samples\hello.rbac.xml", UserRole.Viewer);

        Assert.True(canRead);
    }

    [Fact]
    public void CanRead_WhenRoleIsViewerAndFileIsNotAllowed_ReturnsFalse()
    {
        var policy = new ConfigurableFileAccessPolicy(["hello.rbac.xml"]);

        var canRead = policy.CanRead(@"C:\samples\secret.xml", UserRole.Viewer);

        Assert.False(canRead);
    }

    [Fact]
    public void CanRead_WhenRoleIsViewerAndAllowListIsEmpty_ReturnsFalse()
    {
        var policy = new ConfigurableFileAccessPolicy();

        var canRead = policy.CanRead(@"C:\samples\hello.rbac.xml", UserRole.Viewer);

        Assert.False(canRead);
    }

    [Fact]
    public void CanRead_WhenRoleIsAnonymous_ReturnsFalse()
    {
        var policy = new ConfigurableFileAccessPolicy(["hello.rbac.xml"]);

        var canRead = policy.CanRead(@"C:\samples\hello.rbac.xml", UserRole.Anonymous);

        Assert.False(canRead);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CanRead_WhenPathIsInvalid_ThrowsArgumentException(string? path)
    {
        var policy = new ConfigurableFileAccessPolicy();

        Assert.ThrowsAny<ArgumentException>(() => policy.CanRead(path!, UserRole.Admin));
    }
}

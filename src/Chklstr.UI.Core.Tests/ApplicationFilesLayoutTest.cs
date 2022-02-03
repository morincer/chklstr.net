using System.IO;
using Chklstr.UI.Core.Infra;
using NUnit.Framework;

namespace Chklstr.Core.Tests;

public class ApplicationFilesLayoutTest
{
    private ApplicationFilesLayout layout;

    [SetUp]
    public void SetUp()
    {
        this.layout = new ApplicationFilesLayout(".test-data");
    }

    [Test]
    public void ShouldCreateDirectoriesIfMissing()
    {
        var configPath = this.layout.ConfigFolder;
        if (Directory.Exists(configPath))
        {
            Directory.Delete(configPath, true);
        }
        
        Assert.False(Directory.Exists(configPath));

        ApplicationFilesLayout.EnsureDirectoryExists(configPath);
        
        Assert.True(Directory.Exists(configPath));
    }
}
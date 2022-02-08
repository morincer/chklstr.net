using System.IO;
using Chklstr.Core.Utils;
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

        FileUtils.EnsureDirectoryExists(configPath);
        
        Assert.True(Directory.Exists(configPath));
    }
}
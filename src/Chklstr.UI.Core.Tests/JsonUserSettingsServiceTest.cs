using System.IO;
using Chklstr.Core.Tests;
using Chklstr.UI.Core.Infra;
using Chklstr.UI.Core.Services;
using NUnit.Framework;
using Serilog;

namespace Chklstr.UI.Core.Tests;

public class JsonUserSettingsServiceTest
{
    private JsonUserSettingsService service;

    [SetUp]
    public void SetUp()
    {
        var layout = new ApplicationFilesLayout(".test-data");
        this.service = new JsonUserSettingsService(Path.Combine(layout.ConfigFolder, "user.workspace.json"),
            TestData.Logger<JsonUserSettingsService>());
    }
    
    [Test]
    public void ShouldLoadAndWriteConfiguration()
    {
        if (File.Exists(service.ConfigFile))
        {
            File.Delete(service.ConfigFile);
        }
        
        Assert.False(File.Exists(service.ConfigFile));

        var config = service.Load();
        
        Assert.NotNull(config);
        Assert.NotNull(config.RecentCrafts);
        Assert.That(config.RecentCrafts, Is.Empty);
        
        config.RecentCrafts.Add(new RecentCraftRecord ("Test", "C:/test/test.chklst"));
        
        service.Save(config);
        
        Assert.True(File.Exists(service.ConfigFile));

        var anotherConfig = service.Load();
        
        Assert.That(anotherConfig, Is.Not.EqualTo(config));
        Assert.That(anotherConfig.RecentCrafts, Is.EquivalentTo(config.RecentCrafts));
    }
}
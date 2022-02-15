using System;
using System.IO;
using System.Threading.Tasks;
using Chklstr.Core.Tests;
using Chklstr.UI.Core.Infra;
using DocumentFormat.OpenXml.Drawing;
using NUnit.Framework;
using Path = System.IO.Path;

namespace Chklstr.UI.Core.Tests;

public class FileWatchTest
{
    private FileWatchService _service;

    [SetUp]
    public void Setup()
    {
        this._service = new FileWatchService(TestData.Logger<FileWatchService>());
    }

    [Test]
    public async Task ShouldReportFileChange()
    {
        var path = Path.GetFullPath(".test-data/watch-this.txt");
        await File.WriteAllTextAsync(path, "Test");
        
        _service.Register(path);

        bool changed = false;

        _service.FileChanged += args =>
        {
            if (args.FullPath.Equals(path))
            {
                changed = true;
            }
        };

        await File.WriteAllTextAsync(path, "Test2");

        await Task.Delay(TimeSpan.FromMilliseconds(200));
        Assert.True(changed);
    }
}
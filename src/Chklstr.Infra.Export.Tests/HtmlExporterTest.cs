using System.IO;
using System.Threading.Tasks;
using Chklstr.Core.Model;
using Chklstr.Core.Tests;
using Chklstr.Core.Utils;
using Chklstr.Infra.Export.HTML;
using Chklstr.Infra.Parser;
using NUnit.Framework;

namespace Chklstr.Infra.Export.Tests;

public class HtmlExporterTest
{
    private HtmlExporter _exporter = null!;
    private HTML.Layout _layout = null!;
    private QuickReferenceHandbook _book = null!;

    [SetUp]
    public void Setup()
    {
        this._exporter = new HtmlExporter();
        this._layout = new Layout();

        var service = new QRHParserService(TestData.Logger<QRHParserService>());
        this._book = service.parse(TestData.readFA50Sample()).Result!;

        FileUtils.EnsureDirectoryExists(".test-data");
    }

    [Test]
    public async Task ShouldGenerateFile()
    {
        var path = Path.GetFullPath(".test-data/output.html");
        if (File.Exists(path)) File.Delete(path);
        
        Assert.False(File.Exists(path));
        
        await this._exporter.Export(this._book, path, this._layout, "APU Start");
        
        Assert.True(File.Exists(path));
        Assert.That(new FileInfo(path).Length, Is.GreaterThan(0));
    }
}
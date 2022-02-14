using System.IO;
using Chklstr.Core.Model;
using Chklstr.Core.Tests;
using Chklstr.Core.Utils;
using NUnit.Framework;
using Chklstr.Infra.Parser;

namespace Chklstr.Infra.Export.Tests;

public class DocxExporterTest
{
    private DocxExporter _exporter = null!;
    private Layout _layout = null!;
    private QuickReferenceHandbook _book = null!;

    [SetUp]
    public void Setup()
    {
        this._exporter = new DocxExporter(TestData.LoggerFactory);
        this._layout = new Layout();

        var service = new QRHParserService(TestData.Logger<QRHParserService>());
        this._book = service.parse(TestData.readFA50Sample()).Result!;

        FileUtils.EnsureDirectoryExists(".test-data");
    }

    [Test]
    public void ShouldGenerateFile()
    {
        var path = Path.GetFullPath(".test-data/output.docx");
        if (File.Exists(path)) File.Delete(path);
        
        Assert.False(File.Exists(path));
        
        this._exporter.Export(this._book, path, this._layout, "APU Start");
        
        Assert.True(File.Exists(path));
        Assert.That(new FileInfo(path).Length, Is.GreaterThan(0));
    }
}
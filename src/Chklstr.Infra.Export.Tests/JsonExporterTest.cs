using System.IO;
using Chklstr.Core.Model;
using Chklstr.Core.Tests;
using Chklstr.Infra.Export.Json;
using Chklstr.Infra.Parser;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Chklstr.Infra.Export.Tests;

public class JsonExporterTest
{
    private JsonExporter _exporter;
    private QuickReferenceHandbook _book;

    [SetUp]
    public void SetUp() {
        this._exporter = new JsonExporter();

        var service = new QRHParserService(TestData.Logger<QRHParserService>());
        this._book = service.parse(TestData.readFA50Sample()).Result!;
    }

    [Test]
    public void ShouldGenerateNonEmptyJson() {
        var result = this._exporter.ExportToString(_book, "APU Start");
    Assert.NotNull(result);
    Assert.IsTrue(result.Contains("checklist"));

    var path = Path.GetFullPath(".test-data/output.json");
    File.WriteAllText(path, result);
}
}
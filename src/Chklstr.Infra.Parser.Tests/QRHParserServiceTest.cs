using Chklstr.Core.Tests;
using NUnit.Framework;

namespace Chklstr.Infra.Parser.Tests;

public class QRHParserServiceTest
{
    private QRHParserService _service;

    [SetUp]
    public void SetUp()
    {
        _service = new(TestData.Logger<QRHParserService>());
    }

    [Test]
    public void ShouldParseFa50Sample()
    {
        var result = _service.parse(TestData.readFA50Sample());
        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Result);
        Assert.True(result.Result!.Checklists.Count > 1);
    }

    [Test]
    public void ShouldParseB60TSample()
    {
        var result = _service.parse(TestData.readB60TSample());
        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Result);
        Assert.True(result.Result!.Checklists.Count > 1);
    }

    [Test]
    public void ShouldParseC172Sample()
    {
        var result = _service.parse(TestData.readC172Sample());
        Assert.True(result.IsSuccess());
        Assert.NotNull(result.Result);
        Assert.True(result.Result!.Checklists.Count > 1);
    }
}
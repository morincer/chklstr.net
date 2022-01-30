using Antlr4.Runtime;
using Chklstr.Infra.Parser.Antlr;
using Chklstr.Infra.Parser.Antlr.Gen;
using NUnit.Framework;

namespace Chklstr.Infra.Parser.Tests;

public class ChecklistParserTest
{
    private string? warning;
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ShouldParseSimpleString()
    {
        var sample = "> Header \n=== Checklist";
        var lexer = new ChecklistLexer(CharStreams.fromString(sample));
        var tokens = new CommonTokenStream(lexer);
        var parser = new ChecklistParser(tokens);
        parser.AddErrorListener(new ThrowingErrorListener());
        parser.doc();
    }
}
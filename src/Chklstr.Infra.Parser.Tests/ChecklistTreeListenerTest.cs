using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Chklstr.Core.Model;
using Chklstr.Core.Tests;
using Chklstr.Core.Utils;
using Chklstr.Infra.Parser.Antlr;
using Chklstr.Infra.Parser.Antlr.Gen;
using NUnit.Framework;

namespace Chklstr.Infra.Parser.Tests;

public class ChecklistTreeListenerTest
{
    private ChecklistTreeListener listener;
    private ChecklistParser parser;

    [SetUp]
    public void setUp()
    {
        listener = new ChecklistTreeListener();
    }

    [Test]
    public void shouldGenerateBook()
    {
        var inputStr = "> Header\n=== Checklist 1";
        var book = parse(inputStr);

        Assert.That(book.Checklists.Count, Is.EqualTo(1));
        var cl = book.GetChecklistByName("Checklist 1");
        Assert.NotNull(cl);
    }

    [Test]
    public void shouldAddDefaultContexts()
    {
        var inputStr = "> Header @<ctx>\n=== Checklist 1";
        var book = parse(inputStr);
        Assert.True(book.DefaultContexts.Contains("ctx"));
    }

    [Test]
    public void shouldGenerateChecklistItem()
    {
        var inputStr = "> Header\n=== Checklist 1\n[ ] Item 1 ... CHECK @<somectx> @<another ctx>";
        var book = parse(inputStr);

        Assert.That(book.Checklists.Count, Is.EqualTo(1));
        var cl = book.GetChecklistByName("Checklist 1");
        Assert.NotNull(cl);

        Assert.That(cl.Items.Count, Is.EqualTo(1));
        var item = (SingleCheckItem) cl.Items[0];
        Assert.That(item.CheckName, Is.EqualTo("Item 1"));
        Assert.That(item.Value, Is.EqualTo("CHECK"));
        Assert.True(item.Contexts.Contains("somectx"));
        Assert.True(item.Contexts.Contains("another ctx"));
    }

    [Test]
    public void shouldParseSubList()
    {
        var inputStr = "> H\n=== C1\n--- S2 @<ctx>";
        var book = parse(inputStr);
        Assert.That(book.Checklists.Count, Is.EqualTo(1));
        var cl = book.GetChecklistByName("C1");
        Assert.NotNull(cl);

        var sublist = (Checklist) cl.Items[0];
        Assert.That(sublist.Name, Is.EqualTo("S2"));
        Assert.True(sublist.Contexts.Contains("ctx"));
    }

    [Test]
    public void shouldParseSeriesOfSubLists()
    {
        var inputStr = "> H\n=== C1\n--- S2\n[ ] Item ... CHECK\n--- S3\n[ ] Item 2 ... CHECK";
        var book = parse(inputStr);

        Assert.That(book.GetChecklistByName("C1").Items.Count, Is.EqualTo(2));
    }

    [Test]
    public void shouldParseContexts()
    {
        var inputStr =
            "> H\n=== C1\n[ ] Item ... CHECK @<real> @<not real>\n[ ] Item2 ... CHECK @<real>\n[ ] Item3 ... CHECK";
        var book = parse(inputStr);
        var cl = book.GetChecklistByName("C1");
        Assert.NotNull(cl);
        Assert.That(cl.Items.Count, Is.EqualTo(3));
        Assert.True(cl.Items[0].Contexts.Contains("real"));
        Assert.True(cl.Items[0].Contexts.Contains("not real"));
        Assert.True(cl.Items[1].Contexts.Contains("real"));
        Assert.True(cl.Items[2].Contexts.IsEmpty());
    }

    [Test]
    public void shouldParseDocstring()
    {
        var inputStr = "> H\n=== C1\n```Docstring```";
        var book = parse(inputStr);
        var cl = book.GetChecklistByName("C1");
        Assert.That(cl.Description, Is.EqualTo("Docstring"));
    }

    [Test]
    public void shouldAddSeparator()
    {
        var inputStr = "> H\n=== C1\n-------";
        var book = parse(inputStr);
        var cl = book.GetChecklistByName("C1");
        var item = cl.Items[0];
        Assert.True(item is Separator);
    }

    private QuickReferenceHandbook parse(string inputStr)
    {
        var parser = createParser(inputStr);
        var tree = parser.doc();
        var walker = new ParseTreeWalker();
        walker.Walk(listener, tree);

        Assert.True(listener.SyntaxErrors.IsEmpty(), string.Join(", ", listener.SyntaxErrors));

        var book = listener.Handbook;
        Assert.NotNull(book);

        return book;
    }

    [Test]
    public void shouldParseRealChecklistWithoutErrors()
    {
        var inputStr = TestData.readFA50Sample();
        var book = parse(inputStr);
        Assert.That(book.AircraftName, Is.EqualTo("Flysimware Falcon 50"));
        var startCl = book.GetChecklistByName("Start");
        Assert.NotNull(startCl);
        Assert.That(startCl.Items.Count, Is.EqualTo(40));
        var lastItem = (SingleCheckItem) startCl.Items[3];
        Assert.That(lastItem.CheckName, Is.EqualTo("Exit Lights"));
        Assert.That(lastItem.Value, Is.EqualTo("ARMED"));
    }

    [Test]
    public void shouldParseSublist()
    {
        var inputstring = ">T\r\n=== CL\r\n" +
                          "--- Fuel pumps test\r\n" +
                          "\t[ ] R Pump switch  ......  Pump 1\n" +
                          "---//---\n" +
                          "\t\n" +
                          "\t[ ] Fuel selectors  ......  Crossfeed (For 10-15 seconds)";

        var book = parse(inputstring);
        var cl = book.GetChecklistByName("CL");
        Assert.NotNull(cl);

        Assert.True(cl.Items[0] is Checklist);
    }


    [Test]
    public void shouldIgnorePrependingSymbols()
    {
        var inputStr = "> Test\r\n===CL\r\n     [ ] Item1 ....... CHECK\r\n\t\t\t[ ] Item2 ...... Check";
        var book = parse(inputStr);
        var cl = book.GetChecklistByName("CL");
        Assert.NotNull(cl);
        Assert.That(cl.Items.Count, Is.EqualTo(2));
        Assert.That(((SingleCheckItem) cl.Items[0]).CheckName, Is.EqualTo("Item1"));
        Assert.That(((SingleCheckItem) cl.Items[1]).CheckName, Is.EqualTo("Item2"));
    }

    private ChecklistParser createParser(string str)
    {
        var lexer = new ChecklistLexer(CharStreams.fromString(str));
        var tokens = new CommonTokenStream(lexer);
        parser = new ChecklistParser(tokens);
        parser.AddErrorListener(new ThrowingErrorListener());

        return parser;
    }
}
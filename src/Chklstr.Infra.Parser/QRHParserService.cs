using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Chklstr.Core.Model;
using Chklstr.Core.Services;
using Chklstr.Core.Utils;
using Chklstr.Infra.Parser.Antlr;
using Chklstr.Infra.Parser.Antlr.Gen;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using ILogger = Serilog.ILogger;

namespace Chklstr.Infra.Parser;

public class QRHParserService : IQRHParserService
{
    private ILogger<QRHParserService> _log;

    public QRHParserService(ILogger<QRHParserService> logger)
    {
        _log = logger;
    }

    public ParseResult<QuickReferenceHandbook> parse(string inputString)
    {
        var lexer = new ChecklistLexer(CharStreams.fromString(inputString));
        lexer.RemoveErrorListeners();
        lexer.AddErrorListener(new ThrowingErrorListener());

        var tokens = new CommonTokenStream(lexer);
        var parser = new ChecklistParser(tokens);
        parser.RemoveErrorListeners();
        parser.AddErrorListener(new ThrowingErrorListener());

        try {
            var doc = parser.doc();
            var listener = new ChecklistTreeListener();
            var walker = new ParseTreeWalker();
            walker.Walk(listener, doc);

            if (!listener.SyntaxErrors.IsEmpty()) {
                return ParseResult<QuickReferenceHandbook>.Failed(listener.SyntaxErrors);
            }

            var result = listener.Handbook;
            if (result == null)
            {
                throw new Exception("Empty book returned");
            }
            return ParseResult<QuickReferenceHandbook>.Success(result);
        }
        catch (Exception e) {
            _log.LogError(e, e.Message);
            return ParseResult<QuickReferenceHandbook>.Failed(new List<string> { e.Message });
        }
    }
}
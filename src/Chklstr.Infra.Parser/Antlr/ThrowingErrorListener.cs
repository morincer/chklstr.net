using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace Chklstr.Infra.Parser.Antlr;

public class ThrowingErrorListener : BaseErrorListener, IAntlrErrorListener<int>
{
    public static readonly ThrowingErrorListener INSTANCE = new ThrowingErrorListener();

    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line,
        int charPositionInLine,
        string msg, RecognitionException e)
    {
        throwException(line, charPositionInLine, msg, e);
    }

    private static void throwException(int line, int charPositionInLine, string msg, RecognitionException e)
    {
        throw new ParseCanceledException(e.Context.GetText().Trim() + " (line " + line + ":" +
                                         charPositionInLine +
                                         "): " + msg);
    }

    public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line,
        int charPositionInLine,
        string msg, RecognitionException e)
    {
        throwException(line, charPositionInLine, msg, e);
    }
}
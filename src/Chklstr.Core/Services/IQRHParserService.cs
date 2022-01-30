using Chklstr.Core.Model;

namespace Chklstr.Core.Services;

public interface IQRHParserService
{
    ParseResult<QuickReferenceHandbook> parse(string inputString);
}
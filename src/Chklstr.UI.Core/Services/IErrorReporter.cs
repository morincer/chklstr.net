namespace Chklstr.UI.Core.Services;

public interface IErrorReporter
{
    void ReportError(Type? senderType, Exception e);
}
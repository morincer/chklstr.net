namespace Chklstr.UI.Core.Services;

public interface IErrorReporter
{
    void ReportSuccess(string message);
    void ReportError(Type? senderType, Exception e);
}
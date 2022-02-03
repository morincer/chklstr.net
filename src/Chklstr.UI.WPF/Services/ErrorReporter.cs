using System;
using System.Windows;
using Chklstr.UI.Core.Services;
using Microsoft.Extensions.Logging;
using MvvmCross;

namespace Chklstr.UI.WPF.Services;

public class ErrorReporter : IErrorReporter
{
    public void ReportError(Type? senderType, Exception e)
    {
        var factory = Mvx.IoCProvider.Resolve<ILoggerFactory>();
        var logger = factory.CreateLogger(senderType ?? GetType());
        
        logger.LogError(e, e.Message);

        MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
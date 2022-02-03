using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Chklstr.Core.Services.Voice;
using Chklstr.Infra.Voice;
using Chklstr.UI.Core.Infra;
using Chklstr.UI.Core.Services;
using Chklstr.UI.WPF.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using MvvmCross;
using MvvmCross.Core;
using MvvmCross.IoC;
using MvvmCross.Platforms.Wpf.Core;
using Serilog;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Chklstr.UI.WPF;

public class Setup : MvxWpfSetup<Core.App>
{
    public Setup()
    {
        AttachToParentConsole();
        Application.Current.DispatcherUnhandledException += (sender, args) =>
        {
            OnFatalException(args.Exception);
            args.Handled = true;
        };
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            OnFatalException(args.ExceptionObject);
        };
        TaskScheduler.UnobservedTaskException += (sender, args) =>
        {
            OnFatalException(args.Exception);
            args.SetObserved();
        };
    }
    
    

    public void OnFatalException(object e)
    {
        Log.Logger.Fatal(e.ToString());
        
        if (e is Exception exception)
        {
            Mvx.IoCProvider.Resolve<IErrorReporter>().ReportError(null, exception);
        }
        else
        {
            Mvx.IoCProvider.Resolve<IErrorReporter>().ReportError(null, new Exception(e.ToString()));
        }
    }

    protected override IMvxIoCProvider InitializeIoC()
    {
        var ioc = base.InitializeIoC();
        
        ioc.LazyConstructAndRegisterSingleton<IErrorReporter, ErrorReporter>();
        ioc.LazyConstructAndRegisterSingleton<ITextToSpeechService, TextToSpeechService>();
        ioc.LazyConstructAndRegisterSingleton<IVoiceCommandDetectionService, VoiceCommandDetectionService>();
        ioc.RegisterSingleton(ApplicationFilesLayout.Default);
        
        var pathToConfig = Path.Combine(ioc.Resolve<ApplicationFilesLayout>().ConfigFolder, "user.workspace.json");
        
        ioc.LazyConstructAndRegisterSingleton<IUserSettingsService>(
            () => new JsonUserSettingsService(pathToConfig, Mvx.IoCProvider.Resolve<ILogger<JsonUserSettingsService>>()));
        
        return ioc;
    }

    protected override ILoggerProvider? CreateLogProvider()
    {
        return new SerilogLoggerProvider();
    }

    protected override ILoggerFactory? CreateLogFactory()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] ({SourceContext}.{Method}) {Message}{NewLine}{Exception}")
            .CreateLogger();

        return new SerilogLoggerFactory();
    }

    private const int ATTACH_PARENT_PROCESS = -1;

    [DllImport("kernel32.dll")]
    private static extern bool AttachConsole(int dwProcessId);

    /// <summary>
    ///     Redirects the console output of the current process to the parent process.
    /// </summary>
    /// <remarks>
    ///     Must be called before calls to <see cref="Console.WriteLine()" />.
    /// </remarks>
    public static void AttachToParentConsole()
    {
        AttachConsole(ATTACH_PARENT_PROCESS);
    }
}
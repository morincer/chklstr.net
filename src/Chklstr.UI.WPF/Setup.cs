using System;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;
using Serilog;
using Serilog.Extensions.Logging;

namespace Chklstr.UI.WPF;

public class Setup : MvxWpfSetup<Core.App>
{
    public Setup()
    {
        AttachToParentConsole();
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            if (args.ExceptionObject is Exception exception)
            {
                Log.Logger.Fatal(exception, exception.Message);
            }
            else
            {
                Log.Logger.Fatal(args.ExceptionObject.ToString());
            }
        };
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
            .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] ({SourceContext}.{Method}) {Message}{NewLine}{Exception}")
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
using System.IO;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace Chklstr.Core.Tests;

public static class TestData
{
    public static ILoggerFactory LoggerFactory { get; }
    
    static TestData()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();

        LoggerFactory = new SerilogLoggerFactory();
    }
    
    public static ILogger<T> Logger<T>()
    {
        return LoggerFactory.CreateLogger<T>();
    }
    
    public static string readFA50Sample()
    {
        return ReadSample("FA50");
    }

    private static string ReadSample(string name)
    {
        var fullPath = Path.GetFullPath($"../../../../../samples/{name}.chklst");
        return File.ReadAllText(fullPath);
    }

    public static string readB60TSample()
    {
        return ReadSample("B60T");
    }

    public static string readC172Sample()
    {
        return ReadSample("C172");
    }
}
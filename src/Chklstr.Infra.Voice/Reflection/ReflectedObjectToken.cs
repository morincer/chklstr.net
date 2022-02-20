using System.Speech.Synthesis;

namespace Chklstr.Infra.Voice.Reflection;
public class ReflectedObjectToken
{
    public static readonly Type SourceType = typeof(SpeechSynthesizer).Assembly.GetType("System.Speech.Internal.ObjectTokens.ObjectToken")!;
    public object Source { get; }

    public object? Attributes => Source.GetPropertyValue<object>("Attributes");
    public string? Description => Source.GetPropertyValue<string?>("Description");

    public ReflectedObjectToken(object source)
    {
        if (source.GetType() != SourceType)
        {
            throw new ArgumentException($"Wrong type: {source.GetType()} found, {SourceType} expected");
        }

        Source = source;
    }
}
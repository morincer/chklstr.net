using System.Collections;
using System.Reflection;
using System.Speech.Synthesis;
using Chklstr.Core.Utils;

namespace Chklstr.Infra.Voice.Reflection;
public class ReflectedObjectTokenCategory : IDisposable
{
    public static readonly Type SourceType = typeof(SpeechSynthesizer).Assembly.GetType("System.Speech.Internal.ObjectTokens.ObjectTokenCategory")!;
    public object Source { get; }

    public ReflectedObjectTokenCategory(object source)
    {
        if (source.GetType() != SourceType)
        {
            throw new ArgumentException($"Wrong type: {source.GetType()} found, {SourceType} expected");
        }
        
        Source = source;
    }

    public void Dispose()
    {
        ((IDisposable) Source).Dispose();
    }

    public static ReflectedObjectTokenCategory Create(string sCategoryId)
    {
        var obj = SourceType.CallStatic("Create", new object?[] {sCategoryId})! 
               ?? throw new InvalidOperationException();

        return new ReflectedObjectTokenCategory(obj);
    }

    public IList<ReflectedObjectToken>? FindMatchingTokens(string? requiredAttributes, string? optionalAttributes)
    {
        var tokens = (IList?) Source.CallNonPublic("FindMatchingTokens", new object?[] {null, null});
        if (tokens == null) return null;

        var result = new List<ReflectedObjectToken>();
        
        foreach (var item in tokens)
        {
            result.Add(new ReflectedObjectToken(item));
        }

        return result;
    }

    
}
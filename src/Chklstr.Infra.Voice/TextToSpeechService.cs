using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.Synthesis;
using Chklstr.Core.Services.Voice;
using Microsoft.Extensions.Logging;

namespace Chklstr.Infra.Voice;

public class TextToSpeechService : ITextToSpeechService
{
    private readonly ILogger<TextToSpeechService> _log;
    private SpeechSynthesizer SpeechSynthesizer { get; init; } = new();

    public TextToSpeechService(ILogger<TextToSpeechService> log)
    {
        _log = log;
        Rate = 1;
        _log.LogDebug($"Initialized tts service. Available voices are: {string.Join(", ", Voices)}");
    }

    public string VoiceName
    {
        get => SpeechSynthesizer.Voice.Name;
        set => SpeechSynthesizer.SelectVoice(value);
    }

    public int Rate
    {
        get => SpeechSynthesizer.Rate;
        set => SpeechSynthesizer.Rate = value;
    }

    public ReadOnlyCollection<string> Voices => new(SpeechSynthesizer.GetInstalledVoices(new CultureInfo("en-US"))
        .Select(c => c.VoiceInfo.Name).ToList());

    public void SayAsync(string text, bool priority = false)
    {
        _log.LogDebug($"Playing {text} with voice {SpeechSynthesizer.Voice.Name}, rate {SpeechSynthesizer.Rate}");
        if (priority)
        {
            SpeechSynthesizer.SpeakAsyncCancelAll();
        }
        
        SpeechSynthesizer.SpeakAsync(text);
    }

    public void Stop()
    {
        SpeechSynthesizer.SpeakAsyncCancelAll();
    }
}
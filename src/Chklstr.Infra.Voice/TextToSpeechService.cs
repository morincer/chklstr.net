using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Speech.Synthesis;
using Chklstr.Core.Services.Voice;
using Microsoft.Extensions.Logging;
using Chklstr.Core.Utils;
using Chklstr.Infra.Voice.Reflection;

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
        _log.LogDebug($"Volume: {SpeechSynthesizer.Volume}, Rate: {SpeechSynthesizer.Rate}");

        InjectOneCoreVoices();
    }

    private const string ONE_CORE_VOICES_REGISTRY = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech_OneCore\Voices";

    private void InjectOneCoreVoices()
    {
        try
        {
            var voiceSynthesizer = SpeechSynthesizer.GetPropertyValue<object>("VoiceSynthesizer");

            var installedVoices = voiceSynthesizer?.GetFieldValue<IList>("_installedVoices");
            if (installedVoices == null) return;

            var otc = ReflectedObjectTokenCategory.Create(ONE_CORE_VOICES_REGISTRY);
            using (otc)
            {
                var tokens = otc.FindMatchingTokens(null, null);
                if (tokens == null) return;

                foreach (var objectToken in tokens)
                {
                    if (objectToken.Attributes == null) continue;

                    var voiceInfo =
                        typeof(SpeechSynthesizer).Assembly
                            .CreateInstance("System.Speech.Synthesis.VoiceInfo", true,
                                BindingFlags.Instance | BindingFlags.NonPublic, null,
                                new[] {objectToken.Source}, null, null);

                    if (voiceInfo == null) continue;

                    var installedVoice =
                        typeof(SpeechSynthesizer).Assembly
                            .CreateInstance("System.Speech.Synthesis.InstalledVoice", true,
                                BindingFlags.Instance | BindingFlags.NonPublic, null,
                                new object[] {voiceSynthesizer, voiceInfo}, null, null);

                    if (installedVoice == null) continue;

                    installedVoices.Add(installedVoice);
                }
            }
        }
        catch (Exception e)
        {
            _log.LogWarning($"OneCore voices injection failed: {e.Message}", e);
        }
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

    public Task SayAsync(string text, bool priority = false)
    {
        _log.LogDebug($"Playing {text} with voice {SpeechSynthesizer.Voice.Name}, rate {SpeechSynthesizer.Rate}");
        if (priority)
        {
            SpeechSynthesizer.SpeakAsyncCancelAll();
        }


        var prompt = SpeechSynthesizer.SpeakAsync(text);
        return Task.Run(() =>
        {
            while (!prompt.IsCompleted)
            {
                Thread.Sleep(200);
            }
        });
    }

    public void Stop()
    {
        _log.LogDebug("Stopping tts service");
        SpeechSynthesizer.SpeakAsyncCancelAll();
    }
}
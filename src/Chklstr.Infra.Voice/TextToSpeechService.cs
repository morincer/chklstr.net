﻿using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.Synthesis;
using Chklstr.Core.Services.Voice;
using Microsoft.Extensions.Logging;

namespace Chklstr.Infra.Voice;

public class TextToSpeechService : ITextToSpeechService
{
    private readonly ILogger<TextToSpeechService> _log;
    private SpeechSynthesizer SpeechSynthesizer { get; } = new();

    public TextToSpeechService(ILogger<TextToSpeechService> log)
    {
        _log = log;
        Rate = 1;
        _log.LogDebug($"Initialized tts service. Available voices are: {string.Join(", ", Voices)}");
        _log.LogDebug($"Volume: {SpeechSynthesizer.Volume}, Rate: {SpeechSynthesizer.Rate}");

        InjectOneCoreVoices();
    }

    private void InjectOneCoreVoices()
    {
        try
        {
            SpeechSynthesizer.InjectOneCoreVoices();
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
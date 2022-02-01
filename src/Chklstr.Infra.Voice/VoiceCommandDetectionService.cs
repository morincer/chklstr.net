using System.Collections.ObjectModel;
using System.Globalization;
using System.Speech.Recognition;
using Chklstr.Core;
using Chklstr.Core.Services.Voice;
using Microsoft.Extensions.Logging;

namespace Chklstr.Infra.Voice;

public class VoiceCommandDetectionService : IDisposable, IVoiceCommandDetectionService
{
    public Dictionary<VoiceCommand, HashSet<string>> WordsDictionary { get; }

    public event VoiceCommandDelegate? VoiceCommandDetected;

    private readonly ILogger<VoiceCommandDetectionService> _log;

    public void Dispose()
    {
        _recognizer.Dispose();
    }

    private readonly SpeechRecognitionEngine _recognizer;
    public bool IsRunning { get; private set; }

    public float ConfidenceThreshold { get; set; } = 0.8f;


    public VoiceCommandDetectionService(ILogger<VoiceCommandDetectionService> log,
        CultureInfo cultureInfo,
        Dictionary<VoiceCommand, HashSet<string>> wordsDictionary)
    {
        WordsDictionary = wordsDictionary;
        _log = log;

        var allPhrases = wordsDictionary.Values.SelectMany(c => c).ToHashSet().ToArray();

        var gb = new GrammarBuilder();

        _log.LogDebug($"Initialize recognizer to match: {string.Join(", ", allPhrases)}");

        var choices = new Choices(allPhrases);

        gb.Append(choices);
        gb.Culture = cultureInfo;

        // Create and load a dictation grammar.  
        _recognizer = new SpeechRecognitionEngine(cultureInfo);
        _recognizer.LoadGrammar(new Grammar(gb));

        // Add a handler for the speech recognized event.  
        _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

        // Configure input to the speech recognizer.  
        _recognizer.SetInputToDefaultAudioDevice();
    }

    public VoiceCommandDetectionService(ILogger<VoiceCommandDetectionService> log)
        : this(log, new CultureInfo("en-US"), CommandChoices.Default)
    {
    }

    public void Start()
    {
        IsRunning = true;
        _log.LogInformation("Starting voice recognition service");
        _recognizer.RecognizeAsync(RecognizeMode.Multiple);
    }

    public void Stop()
    {
        IsRunning = false;
        _recognizer.RecognizeAsyncCancel();
    }

    private void Recognizer_SpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
    {
        _log.LogDebug($"Detected word {e.Result.Text}, confidence {e.Result.Confidence}");

        if (!IsRunning) return;

        if (e.Result.Confidence < ConfidenceThreshold) return;

        var command = WordsDictionary
            .Where(kvp => kvp.Value.Contains(e.Result.Text))
            .Select(kvp => kvp.Key).FirstOrDefault();

        _log.LogDebug($"Word {e.Result.Text} mapped to {command}");

        OnVoiceCommandDetected(command);
    }

    private void OnVoiceCommandDetected(VoiceCommand command)
    {
        VoiceCommandDetected?.Invoke(this, command);
    }
}
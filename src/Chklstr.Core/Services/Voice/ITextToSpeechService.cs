using System.Collections.ObjectModel;

namespace Chklstr.Core.Services.Voice;

public interface ITextToSpeechService
{
    string VoiceName { get; set; }
    int Rate { get; set; }
    ReadOnlyCollection<string> Voices { get; }
    Task SayAsync(string text, bool priority = false);
    void Stop();
}
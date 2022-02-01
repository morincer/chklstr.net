namespace Chklstr.Core.Services.Voice;

public static class CommandChoices
{
    public static readonly Dictionary<VoiceCommand, HashSet<string>> Default = new()
    {
        {
            VoiceCommand.Check, new HashSet<string> {"check", "checked", "yes", "yeah", "yeap"}
        },
        {
            VoiceCommand.Skip, new HashSet<string> {"skip", "skipped", "next"}
        }
    };
}

public delegate void VoiceCommandDelegate(object sender, VoiceCommand args);

public enum VoiceCommand
{
    Check,
    Skip
}

public interface IVoiceCommandDetectionService
{
    Dictionary<VoiceCommand, HashSet<string>> WordsDictionary { get; }
    float ConfidenceThreshold { get; set; }
    event VoiceCommandDelegate? VoiceCommandDetected;
    void Dispose();
    void Start();
    void Stop();
}
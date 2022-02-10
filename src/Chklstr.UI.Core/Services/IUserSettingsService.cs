namespace Chklstr.UI.Core.Services;

public class RecentCraftRecord
{
    public string AircraftName { get; set; }
    public string Path { get; set; }
    public long Timestamp { get; set; }

    public RecentCraftRecord() : this("", "", 0)
    {
        
    }

    public RecentCraftRecord(string aircraftName, string path) : this(aircraftName, path,
        DateTimeOffset.Now.ToUnixTimeMilliseconds())
    {
        
    }
    
    public RecentCraftRecord(string aircraftName, string path, long timestamp)
    {
        AircraftName = aircraftName;
        Path = path;
        Timestamp = timestamp;
    }

    protected bool Equals(RecentCraftRecord other)
    {
        return AircraftName == other.AircraftName && Path == other.Path && Timestamp == other.Timestamp;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((RecentCraftRecord) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(AircraftName, Path, Timestamp);
    }
}

public class Config
{
    public string? ExternalEditorPath { get; set; } = "notepad.exe";
    public string? SelectedVoice { get; set; }
    public bool VoiceControlEnabled { get; set; } = true;
    public List<RecentCraftRecord> RecentCrafts { get; set; } = new();
}

public interface IUserSettingsService
{
    public Config Load();
    public void Save(Config config);
}
namespace Chklstr.UI.Core.Services;

public class RecentCraftRecord
{
    public string AircraftName { get;}
    public string Path { get; }
    public int Timestamp { get;}

    public RecentCraftRecord(string aircraftName, string path, int timestamp)
    {
        AircraftName = aircraftName;
        Path = path;
        Timestamp = timestamp;
    }
}

public class Config
{
    public List<RecentCraftRecord> RecentCrafts { get; } = new();
}

public interface IUserSettingsService
{
    public Config Load();
    public void Save(Config config);
}
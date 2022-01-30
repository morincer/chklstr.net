#nullable enable
namespace Chklstr.Core.Model;

public class QuickReferenceHandbook
{
    public string AircraftName { get; }
    public List<Checklist> Checklists { get; } = new() ;

    public HashSet<string> DefaultContexts { get; init; } = new();


    public QuickReferenceHandbook(string aircraftName)
    {
        AircraftName = aircraftName;
    }

    public Checklist Add(string checklistName)
    {
        var cl = new Checklist(checklistName, null);
        Checklists.Add(cl);
        return cl;
    }

    public Checklist? GetChecklistByName(string checklistName)
    {
        return Checklists.Find(cl => checklistName.Equals(cl.Name));
    }
    
    public HashSet<String> GetAllAvailableContexts()
    {
        var result = Checklists
            .SelectMany(c => c.GetAllAvailableContexts())
            .ToHashSet();


        return result;
    }
}
using Chklstr.Core.Utils;

namespace Chklstr.Core.Model;

public class Checklist : ChecklistItem
{
    public string Name { get; set; }

    public List<ChecklistItem> Items { get; init; } = new();

    public Checklist(string name, Checklist? parent) : base(parent, true)
    {
        Name = name;
    }
    
    public SingleCheckItem AddSingleItem(string title, string check) {
        var item = new SingleCheckItem(this)
        {
            CheckName = title,
            Value = check
        };
        
        Items.Add(item);
        
        return item;
    }
    
    public bool IsComplete(params string[] contexts) {
        if (!IsAvailableInContext(contexts)) return false;

        return GetCountCheckable(contexts) == GetCountChecked(contexts);
    }
    
    public int GetCountCheckable(params string[] contexts) {
        if (!IsAvailableInContext(contexts)) return 0;

        return Items
            .Where(i => i.IsAvailableInContext(contexts))
            .Where(i => i.IsCheckable || i is Checklist)
            .Select(i => i is Checklist checklist ? checklist.GetCountCheckable(contexts) : 1)
            .Sum();
    }

    public int GetCountChecked(params string[] contexts) {
        if (!IsAvailableInContext(contexts)) return 0;
        
        return Items
            .Where(i => i.IsAvailableInContext(contexts))
            .Where(i => i.Checked || i is Checklist)
            .Select(i => i is Checklist checklist ? checklist.GetCountChecked(contexts) : 1)
            .Sum();
    }

    public Checklist AddSubList(string name)
    {
        var item = new Checklist(name, this);
        Items.Add(item);
        return item;
    }

    public Separator AddSeparator()
    {
        var item = new Separator(this);
        Items.Add(item);
        return item;
    }
    
    public override HashSet<string> GetAllAvailableContexts() {
        var result = new HashSet<string>(Contexts);

        foreach (var item in Items) {
            result.AddAll(item.GetAllAvailableContexts());
        }

        return result;
    }
}
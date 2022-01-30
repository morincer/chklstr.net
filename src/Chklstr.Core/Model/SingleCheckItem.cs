namespace Chklstr.Core.Model;

public class SingleCheckItem : ChecklistItem
{
    public string CheckName { get; set; } = "";
    public string Value { get; set; } = "";

    public SingleCheckItem(Checklist parent) : base(parent, true)
    {
        
    }
}
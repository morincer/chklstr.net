using Markdig;

namespace Chklstr.Core.Model;

public abstract class ChecklistItem
{
    public bool IsEnabled { get; set; } = true;
    public string Description { get; set; } = "";
    public bool IsCheckable { get; init; }
    
    public bool Checked { get; set; }
    public HashSet<string> Contexts { get; } = new();
    public Checklist? Parent { get; init; }

    protected ChecklistItem(Checklist? parent, bool isCheckable)
    {
        IsCheckable = isCheckable;
        Parent = parent;
    }

    public bool IsAvailableInContext(params string[] contexts)
    {
        if (!IsEnabled) return false;
        if (this.Parent != null && !Parent.IsAvailableInContext(contexts)) {
            return false;
        }

        return this.Contexts.Count == 0 || contexts.Any(c => Contexts.Contains(c));
    }
    
    public virtual HashSet<string> GetAllAvailableContexts() {
        return Contexts;
    }

    public string DescriptionAsHtml()
    {
        return Markdown.ToHtml(Description);
    }
}
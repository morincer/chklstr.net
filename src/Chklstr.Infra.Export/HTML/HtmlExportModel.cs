using Chklstr.Core.Model;

namespace Chklstr.Infra.Export.HTML;

public class HtmlExportModel
{
    public QuickReferenceHandbook Book { get; set; }
    public Layout Layout { get; set; }

    public HashSet<string> SelectedContexts { get; } = new();
    public Dictionary<string, string> ContextClasses { get; } = new();
    
    public string Filler { get; set; }
}
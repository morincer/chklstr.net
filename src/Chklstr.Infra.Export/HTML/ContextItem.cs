using Chklstr.Core.Model;

namespace Chklstr.Infra.Export.HTML;

public class ContextItem
{
    public HtmlExportModel ExportModel { get; }
    
    public ChecklistItem Item { get; }

    public ContextItem(HtmlExportModel exportModel, ChecklistItem item)
    {
        ExportModel = exportModel;
        Item = item;
    }

    public T As<T>() where T : ChecklistItem
    {
        return Item as T ?? throw new InvalidOperationException();
    }

    public string ContextToClasses()
    {
        return string.Join(" ", Item.Contexts.Select(ExportModel.ContextClasses.GetValueOrDefault).Where(c => c != null));
    }
}
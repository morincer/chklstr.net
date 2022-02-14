using Chklstr.Core.Services;
using Chklstr.Infra.Export.HTML;
using Chklstr.UI.Core.ViewModels;

namespace Chklstr.UI.WPF.Services.Export;

public class HtmlExporterService : IExporterService
{
    public string FormatName => "HTML";
    public string FormatExtension => "html";
    public Task Export(ExportViewModel exportViewModel)
    {
        var exporter = new HtmlExporter();
        var layout = new Layout
        {
            ShowDescription = exportViewModel.ShowDescriptions
        };

        return exporter.Export(exportViewModel.Book, 
            exportViewModel.TargetPath, 
            layout, 
            exportViewModel.Contexts.Where(c => c.Selected).Select(c => c.Name).ToArray());
    }
}
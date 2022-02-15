using Chklstr.Core.Services;
using Chklstr.Infra.Export.Json;
using Chklstr.UI.Core.ViewModels;

namespace Chklstr.UI.Core.Infra.Export;

public class JsonExporterService : IExporterService
{
    public string FormatName => "JSON";
    public string FormatExtension => "json";
    public Task Export(ExportViewModel exportViewModel)
    {
        var exporter = new JsonExporter();
        return Task.Run(() =>
        {
            exporter.Export(exportViewModel.Book, exportViewModel.TargetPath, 
                exportViewModel.Contexts.Where(c => c.Selected).Select(c => c.Name).ToArray());
        });
    }
}
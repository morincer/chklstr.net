using System.Linq;
using System.Threading.Tasks;
using Chklstr.Core.Services;
using Chklstr.Infra.Export;
using Chklstr.UI.Core.ViewModels;
using Microsoft.Extensions.Logging;
using MvvmCross;

namespace Chklstr.UI.WPF.Services.Export;

public class WordExporterService : IExporterService
{
    public string FormatName => "Microsoft Word";
    public string FormatExtension => "docx";
    
    public Task Export(ExportViewModel exportViewModel)
    {
        var loggerFactory = Mvx.IoCProvider.Resolve<ILoggerFactory>();
        var exporter = new DocxExporter(loggerFactory);
        var layout = new Layout
        {
            ShowDescriptions = exportViewModel.ShowDescriptions
        };

        return Task.Run(() =>
        {
            exporter.Export(exportViewModel.Book, exportViewModel.TargetPath, layout,
                exportViewModel.Contexts.Where(c => c.Selected).Select(c => c.Name).ToArray());
        });
    }
}
using Chklstr.UI.Core.ViewModels;

namespace Chklstr.Core.Services;

public interface IExporterService
{
    string FormatName { get; }
    string FormatExtension { get; }
    
    Task Export(ExportViewModel exportViewModel);
}
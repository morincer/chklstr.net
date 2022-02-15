using System.Collections.ObjectModel;
using Chklstr.Core.Model;
using Chklstr.Core.Services;
using Chklstr.Core.Utils;
using Chklstr.UI.Core.Services;
using Chklstr.UI.Core.Utils;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Chklstr.UI.Core.ViewModels;

public class ExportViewModel : MvxViewModel<QRHViewModel>
{
    private readonly IMvxNavigationService _navigationService;
    private readonly IErrorReporter _errorReporter;
    private string _targetPath;

    public string TargetPath
    {
        get => _targetPath;
        set
        {
            SetProperty(ref _targetPath, value);
            RaisePropertyChanged(() => CommandExport);
        }
    }

    public IExporterService ExporterService { get; }

    public ObservableCollection<ContextViewModel> Contexts { get; } = new();

    private bool _showDescriptions = true;

    public bool ShowDescriptions
    {
        get => _showDescriptions;
        set => SetProperty(ref _showDescriptions, value);
    }
    
    public QuickReferenceHandbook Book { get; private set; }

    public MvxAsyncCommand CommandExport => new(Export, () => !string.IsNullOrEmpty(TargetPath));

    private bool _isRunning;

    public bool IsRunning
    {
        get => _isRunning;
        set => SetProperty(ref _isRunning, value);
    }

    public async Task Export()
    {
        IsRunning = true;
        try
        {
            await Task.Run(() => ExporterService.Export(this));
            
            IsRunning = false;
            
            _errorReporter.ReportSuccess($"Exported to {TargetPath}");
            await _navigationService.Close(this);
        }
        catch (Exception e)
        {
            _errorReporter.ReportError(GetType(), e);
        }
        finally
        {
            IsRunning = false;
        }
    }

    public MvxInteraction<SelectFileRequest> SelectFilePathInteraction = new();
    public MvxAsyncCommand CommandClose => new(Close);

    public async Task Close()
    {
        await _navigationService.Close(this);
    }

    public MvxCommand ChooseTargetPathCommand => new(ChooseTarget);

    private void ChooseTarget()
    {
        var request = new SelectFileRequest()
        {
            FileExtension = "*." + ExporterService.FormatExtension,
            BaseFolder = Path.GetDirectoryName(TargetPath),
            FormatName = ExporterService.FormatName,
            FileSelectedCallback = async path =>
            {
                if (path == null) return;
                path = Path.GetFullPath(path);
                TargetPath = path;
            }
        };

        SelectFilePathInteraction.Raise(request);
    }

    public ExportViewModel(IMvxNavigationService navigationService,
        IErrorReporter errorReporter, IExporterService exporterService)
    {
        _navigationService = navigationService;
        _errorReporter = errorReporter;
        ExporterService = exporterService;
    }

    public override void Prepare(QRHViewModel qrhViewModel)
    {
        if (qrhViewModel.LoadedFrom != null)
        {
            TargetPath = Path.Combine(
                Path.GetDirectoryName(qrhViewModel.LoadedFrom) ?? "",
                Path.GetFileNameWithoutExtension(qrhViewModel.LoadedFrom) + "." + ExporterService.FormatExtension);
        }

        Book = qrhViewModel.Item;

        Contexts.AddAll(qrhViewModel.Contexts.Select(c => new ContextViewModel { Name = c.Name, Selected = c.Selected }));
        
    }
}
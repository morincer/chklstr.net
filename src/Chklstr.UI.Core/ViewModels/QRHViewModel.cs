using System.Collections.ObjectModel;
using Chklstr.Core.Model;
using Chklstr.Core.Services;
using Chklstr.UI.Core.Infra;
using Chklstr.UI.Core.Infra.Export;
using Chklstr.UI.Core.Services;
using Chklstr.UI.Core.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Chklstr.UI.Core.ViewModels;

public class QRHViewModelResult
{
    public QuickReferenceHandbook? RedirectTo { get; set; }
}
public class QRHViewModel : MvxViewModel<QuickReferenceHandbook, QRHViewModelResult>
{
    private readonly ILogger<QRHViewModel> _logger;
    private readonly IUserSettingsService _userSettingsService;
    private readonly IMvxNavigationService _navigationService;
    private readonly IErrorReporter _errorReporter;
    public QuickReferenceHandbook Item { get; private set; }
    public string AircraftName { get; set; }
    
    public string? LoadedFrom { get; set; }
    public ObservableCollection<ChecklistViewModel> Checklists { get; init; } = new();

    public ObservableCollection<ContextViewModel> Contexts { get; init; } = new();

    private ChecklistViewModel? _selectedChecklist;

    public ChecklistViewModel? SelectedChecklist
    {
        get => _selectedChecklist;
        set => SetProperty(ref _selectedChecklist, value);
    }

    private bool _isTextToSpeechEnabled;

    public bool IsTextToSpeechEnabled
    {
        get => _isTextToSpeechEnabled;
        set => SetProperty(ref _isTextToSpeechEnabled, value);
    }

    private bool _isVoiceControlEnabled;

    public bool IsVoiceControlEnabled
    {
        get => _isVoiceControlEnabled;
        set => SetProperty(ref _isVoiceControlEnabled, value);
    }

    private bool _isSaying;

    public bool IsSaying
    {
        get => _isSaying;
        set => SetProperty(ref _isSaying, value);
    }

    private bool _isListening;

    public bool IsListening
    {
        get => _isListening;
        set => SetProperty(ref _isListening, value);
    }
    
    public MvxCommand SpeakCommand => new(ToggleSpeak);
    
    public void ToggleSpeak()
    {
        IsTextToSpeechEnabled = !IsTextToSpeechEnabled;
    }

    public MvxCommand EditCommand => new(OpenExternalEditor);

    public void OpenExternalEditor()
    {
        var cmd = this._userSettingsService.Load().ExternalEditorPath;
        if (string.IsNullOrEmpty(cmd) || string.IsNullOrEmpty(Item.Metadata.LoadedFrom)) return;

        var path = Path.GetFullPath(Item.Metadata.LoadedFrom);
        
        if (cmd.Contains("%1"))
        {
            cmd = cmd.Replace("%1", path);
        }
        else
        {
            cmd = $"\"{cmd}\" \"{path}\"";
        }
        
        _logger.LogInformation("Executing command " + cmd);

        System.Diagnostics.Process.Start(cmd);
    }

    public MvxInteraction<SelectFileRequest> SelectFilePathInteraction = new();
    private readonly FileWatchService _fileWatchService;

    public MvxCommand ExportToWordCommand => new(ExportToWord);
    public MvxCommand ExportToHtmlCommand => new(ExportToHtml);
    public MvxCommand ExportToJsonCommand => new(ExportToJson);
    public MvxCommand OpenCommand => new(OpenAnotherFile);

    public MvxCommand ReloadCommand => new(Reload);

    public async void Reload()
    {
        if (LoadedFrom == null) return;
        await RequestQRHOpen(LoadedFrom);
    }

    public MvxCommand SettingsCommand => new(OpenSettings);

    public async void ExportToJson()
    {
        await ExportUsing(new JsonExporterService());
    }
    
    public async void ExportToWord()
    {
        var exporter = new WordExporterService();
        await ExportUsing(exporter);
    }
    
    public async void ExportToHtml()
    {
        var exporter = new HtmlExporterService();
        await ExportUsing(exporter);
    }

    private async Task ExportUsing(IExporterService exporter)
    {
        IsTextToSpeechEnabled = false;
        IsVoiceControlEnabled = false;
        var viewModel = new ExportViewModel(_navigationService, _errorReporter, exporter);
        viewModel.Prepare(this);
        await _navigationService.Navigate(viewModel);
    }

    public async void OpenSettings()
    {
        IsTextToSpeechEnabled = false;
        IsVoiceControlEnabled = false;
        
        await _navigationService.Navigate<SettingsViewModel>();
    }
    public void OpenAnotherFile()
    {
        var config = _userSettingsService.Load();

        var recentCraft = config.RecentCrafts.OrderBy(c => -c.Timestamp).FirstOrDefault();

        var request = new SelectFileRequest()
        {
            BaseFolder = recentCraft != null ? Path.GetDirectoryName(recentCraft.Path) : null,
            FileExtension = "*.chklst",
            FileSelectedCallback = async path =>
            {
                if (path == null) return;

                path = Path.GetFullPath(path);
                
                _logger.LogInformation($"Trying to load {path}");
                await RequestQRHOpen(path);
            }
        };
        
        SelectFilePathInteraction.Raise(request);
    }

    private async Task RequestQRHOpen(string path)
    {
        var result =
            await _navigationService.Navigate<QRHParsingViewModel, string, ParseResult<QuickReferenceHandbook>>(path);
        if (result != null && result.IsSuccess())
        {
            await Close(result.Result);
        }
    }

    public override void ViewAppeared()
    {
        LoadConfig();
        if (LoadedFrom != null)
        {
            _fileWatchService.Register(LoadedFrom);
        }
    }

    public override void ViewDestroy(bool viewFinishing = true)
    {
        if (LoadedFrom != null)
        {
            _fileWatchService.Unregister(LoadedFrom);
        }
        base.ViewDestroy(false);
    }

    private async Task Close(QuickReferenceHandbook? redirectTo)
    {
        _logger.LogInformation($"Closing QRH {AircraftName}");
        IsTextToSpeechEnabled = false;
        await _navigationService.Close(this, new() { RedirectTo = redirectTo });
    }

    private bool _isDirty;

    public bool IsDirty
    {
        get => _isDirty;
        set => SetProperty(ref _isDirty, value);
    }

    public String[] SelectedContexts
    {
        get => Contexts.Where(c => c.Selected).Select(c => c.Name).ToArray();
    }

    public QRHViewModel(IUserSettingsService userSettingsService,
        IMvxNavigationService navigationService,
        IErrorReporter errorReporter,
        ILoggerFactory loggerFactory)
    {
        _navigationService = navigationService;
        _errorReporter = errorReporter;
        _logger = loggerFactory.CreateLogger<QRHViewModel>();
        _userSettingsService = userSettingsService;
        _fileWatchService = new FileWatchService(loggerFactory.CreateLogger<FileWatchService>());
        _fileWatchService.FileChanged += _ =>
        {
            Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(() =>
            {
                IsDirty = true;
            });
        };
    }

    public override void Prepare(QuickReferenceHandbook book)
    {
        Item = book;
    }

    public override async Task Initialize()
    {
        AircraftName = Item.AircraftName;
        LoadedFrom = Item.Metadata.LoadedFrom;

        var level = new HierarchyLevel();

        foreach (var ctx in Item.GetAllAvailableContexts())
        {
            var viewModel = Mvx.IoCProvider.IoCConstruct<ContextViewModel>();
            viewModel.Prepare(ctx);
            await viewModel.Initialize();
            viewModel.Selected = Item.DefaultContexts.Contains(ctx);
            viewModel.PropertyChanged += (sender, args) => UpdateContexts();
            Contexts.Add(viewModel);
        }
        
        foreach (var checklist in Item.Checklists)
        {
            try
            {
                var viewModel = Mvx.IoCProvider.IoCConstruct<ChecklistViewModel>();
                viewModel.QRH = this;
                viewModel.Prepare(checklist);
                await viewModel.Initialize();
                viewModel.ListNumber = level.ToString();
                level = level.Next();
                Checklists.Add(viewModel);
                _logger.LogDebug($"Adding checklist {viewModel.Name}");
            }
            catch (Exception e)
            {
                Mvx.IoCProvider.Resolve<IErrorReporter>().ReportError(typeof(QRHViewModel), e);
            }
        }
        
        UpdateContexts();
        SelectedChecklist = Checklists.FirstOrDefault(cl => cl.IsEnabled);
    }

    private void LoadConfig()
    {
        var config = _userSettingsService.Load();
        IsVoiceControlEnabled = config.VoiceControlEnabled;
    }

    private void UpdateContexts()
    {
        var ctxs = SelectedContexts;
        
        foreach (var checklistViewModel in Checklists)
        {
            checklistViewModel.Contexts = ctxs;
        }
    }
}
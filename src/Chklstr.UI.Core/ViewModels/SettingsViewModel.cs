using System.Collections.ObjectModel;
using Chklstr.Core.Services.Voice;
using Chklstr.Core.Utils;
using Chklstr.UI.Core.Services;
using Chklstr.UI.Core.Utils;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmValidation;

namespace Chklstr.UI.Core.ViewModels;

public class SettingsViewModel : ValidatableMvxViewModel
{
    private readonly IMvxNavigationService _navigationService;
    private readonly IUserSettingsService _userSettingsService;
    private readonly ITextToSpeechService _textToSpeechService;
    private readonly IErrorReporter _errorReporter;
    private string? _externalEditorPath;

    public string? ExternalEditorPath
    {
        get => _externalEditorPath;
        set => SetProperty(ref _externalEditorPath, value);
    }

    private string? _selectedVoice;

    public string? SelectedVoice
    {
        get => _selectedVoice;
        set => SetProperty(ref _selectedVoice, value);
    }

    public ObservableCollection<string> Voices { get; } = new();

    private bool _voiceControlEnabled;

    public bool VoiceControlEnabled
    {
        get => _voiceControlEnabled;
        set => SetProperty(ref _voiceControlEnabled, value);
    }
    
    public MvxInteraction<SelectFileRequest> SelectFilePathInteraction = new();

    public MvxCommand ChooseEditorCommand => new(ChooseEditor);
    
    public void ChooseEditor()
    {
        var request = new SelectFileRequest()
        {
            FileExtension = "*.exe",
            FileSelectedCallback = async path =>
            {
                if (path == null) return;

                path = Path.GetFullPath(path);

                ExternalEditorPath = path;
            }
        };
        
        SelectFilePathInteraction.Raise(request);
    }

    public MvxAsyncCommand CommandClose => new(Close);

    public async Task Close()
    {
        await _navigationService.Close(this);
    }

    public MvxAsyncCommand CommandTestVoice => new(TestVoice);

    public async Task TestVoice()
    {
        if (SelectedVoice == null) return;
        
        var oldVoice = _textToSpeechService.VoiceName;
        _textToSpeechService.VoiceName = SelectedVoice;
        await _textToSpeechService.SayAsync("Prestart checklist started.", true);
        _textToSpeechService.VoiceName = oldVoice;
    }

    public MvxAsyncCommand CommandSaveAndClose => new(SaveAndClose);

    public async Task SaveAndClose()
    {
        try
        {
            var result = await Validator.ValidateAllAsync();
            if (!result.IsValid)
            {
                throw new ValidationException(result.ToString());
            }
            
            var config = _userSettingsService.Load();
            config.SelectedVoice = SelectedVoice;
            config.ExternalEditorPath = ExternalEditorPath;
            config.VoiceControlEnabled = VoiceControlEnabled;
            
            _userSettingsService.Save(config);

            await Close();
        }
        catch (Exception e)
        {
            _errorReporter.ReportError(this.GetType(), e);
        }
    }
    
    public SettingsViewModel(
        IMvxNavigationService navigationService,
        IUserSettingsService userSettingsService,
        ITextToSpeechService textToSpeechService,
        IErrorReporter errorReporter)
    {
        this._navigationService = navigationService;
        _userSettingsService = userSettingsService;
        _textToSpeechService = textToSpeechService;
        _errorReporter = errorReporter;
        
        Validator.AddRequiredRule(() => ExternalEditorPath, "External editor path is required");
        Validator.AddRequiredRule(() => SelectedVoice, "You must select a voice for TTS");
        Validator.AddRule(() => ExternalEditorPath,
            () => RuleResult.Assert(string.IsNullOrEmpty(ExternalEditorPath) || File.Exists(ExternalEditorPath), $"File {ExternalEditorPath} does not exist"));

    }

    public override void ViewAppeared()
    {
        Refresh();
    }

    private void Refresh()
    {
        try
        {
            var config = _userSettingsService.Load();
            
            Voices.Clear();
            Voices.AddAll(_textToSpeechService.Voices);
            
            ExternalEditorPath = config.ExternalEditorPath;
            SelectedVoice = config.SelectedVoice;
            if (SelectedVoice == null && !Voices.IsEmpty())
            {
                SelectedVoice = Voices[0];
            }
            VoiceControlEnabled = config.VoiceControlEnabled;

            Validator.ValidateAll();
        }
        catch (Exception e)
        {
            _errorReporter.ReportError(typeof(SettingsViewModel), e);
        }
    }
}
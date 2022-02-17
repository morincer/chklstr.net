using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Chklstr.Core.Model;
using Chklstr.Core.Services;
using Chklstr.Core.Services.Voice;
using Chklstr.Core.Utils;
using Chklstr.UI.Core.Infra;
using Chklstr.UI.Core.Services;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Chklstr.UI.Core.ViewModels;

public partial class 
    
    ApplicationViewModel : MvxViewModel
{
    private readonly IMvxNavigationService _navigationService;
    private readonly IUserSettingsService _userSettingsService;
    private readonly IVoiceCommandDetectionService _voiceCommandDetectionService;
    private readonly ITextToSpeechService _textToSpeechService;
    private readonly ILogger<ApplicationViewModel> _logger;

    public ObservableCollection<RecentCraftRecord> RecentCrafts { get; } = new();

    public ApplicationViewModel(IMvxNavigationService navigationService,
        IUserSettingsService userSettingsService,
        IVoiceCommandDetectionService voiceCommandDetectionService,
        ITextToSpeechService textToSpeechService,
        ILoggerFactory loggerFactory)
    {
        _navigationService = navigationService;
        _userSettingsService = userSettingsService;
        _voiceCommandDetectionService = voiceCommandDetectionService;
        _textToSpeechService = textToSpeechService;
        _logger = loggerFactory.CreateLogger<ApplicationViewModel>();
    }

    public MvxAsyncCommand<string> LoadQRHCommand => new(LoadQRH);

    public override void Start()
    {
        Trace.WriteLine("test trace");
        _logger.LogInformation("Starting application");

        /*var lastLoaded = _userSettingsService.Load().RecentCrafts
            .OrderBy(c => -c.Timestamp).FirstOrDefault();

        if (lastLoaded != null)
        {
            Task.Run(() => LoadQRH(lastLoaded.Path));
        }*/

    }

    public override void ViewAppeared()
    {
        RecentCrafts.Clear();
        RecentCrafts.AddAll(_userSettingsService.Load().RecentCrafts);
    }

    public async Task LoadQRH(string pathToFile)
    {
        try
        {
            var path = Path.GetFullPath(pathToFile);
            var result = await _navigationService.Navigate<QRHParsingViewModel, string, ParseResult<QuickReferenceHandbook>>(path);
            if (result == null || !result.IsSuccess()) return;

            _logger.LogDebug($"Loading QRH ViewModel for {result.Result?.AircraftName}");
            var quickReferenceHandbook = result.Result!;
            
            await OpenQRH(quickReferenceHandbook);
        }
        catch (Exception e)
        {
            Mvx.IoCProvider.Resolve<IErrorReporter>().ReportError(GetType(), e);
        }
    }

    private async Task OpenQRH(QuickReferenceHandbook quickReferenceHandbook)
    {
        try
        {
            if (quickReferenceHandbook.Metadata.LoadedFrom != null)
            {
                var config = _userSettingsService.Load();
                config.RecentCrafts.Add(new RecentCraftRecord(quickReferenceHandbook.AircraftName,
                    quickReferenceHandbook.Metadata.LoadedFrom));
                _userSettingsService.Save(config);
            }

            _logger.LogDebug($"Creating QRH View model for {quickReferenceHandbook.AircraftName}");
            var redirect =
                await _navigationService.Navigate<QRHViewModel, QuickReferenceHandbook, QRHViewModelResult>(
                    quickReferenceHandbook);

            if (redirect?.RedirectTo == null)
            {
                _logger.LogDebug("QRH closed without any redirection - exiting");
                return;
            }

            await OpenQRH(redirect.RedirectTo);
        }
        catch (Exception e)
        {
            Mvx.IoCProvider.Resolve<IErrorReporter>().ReportError(GetType(), e);
            await _navigationService.Close(this);
        }
    }

    public void Exit()
    {
        _logger.LogInformation("Closing the application");
        _voiceCommandDetectionService.Stop();
        _textToSpeechService.Stop();
    }
}
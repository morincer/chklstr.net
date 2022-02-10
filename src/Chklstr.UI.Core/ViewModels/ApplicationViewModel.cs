using System.ComponentModel;
using System.Diagnostics;
using Chklstr.Core.Model;
using Chklstr.Core.Services;
using Chklstr.UI.Core.Services;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Chklstr.UI.Core.ViewModels;

public partial class ApplicationViewModel : MvxViewModel
{
    private readonly IMvxNavigationService _navigationService;
    private readonly IUserSettingsService _userSettingsService;
    private readonly ILogger<ApplicationViewModel> _logger;

    public ApplicationViewModel(IMvxNavigationService navigationService,
        IUserSettingsService userSettingsService,
        ILogger<ApplicationViewModel> logger)
    {
        _navigationService = navigationService;
        _userSettingsService = userSettingsService;
        _logger = logger;
    }

    public override void Start()
    {
        Trace.WriteLine("test trace");
        _logger.LogInformation("Starting application");

        Task.Run(() => LoadQRH($"../../../../../samples/FA50.chklst"));
        // Task.Run(() =>_navigationService.Navigate<SettingsViewModel>());
    }

    public async Task LoadQRH(string pathToFile)
    {
        try
        {
            var path = Path.GetFullPath(pathToFile);
            var result = await _navigationService.Navigate<QRHParsingViewModel, string, ParseResult<QuickReferenceHandbook>>(path);
            if (result == null || !result.IsSuccess()) return;

            var config = _userSettingsService.Load();
            config.RecentCrafts.Add(new RecentCraftRecord(result.Result!.AircraftName, path));
            _userSettingsService.Save(config);
        
        
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
}
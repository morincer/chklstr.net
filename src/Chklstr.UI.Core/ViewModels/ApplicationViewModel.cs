using System.ComponentModel;
using System.Diagnostics;
using Chklstr.Core.Model;
using Chklstr.Core.Services;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Chklstr.UI.Core.ViewModels;

public partial class ApplicationViewModel : MvxViewModel
{
    private readonly IMvxNavigationService _navigationService;
    private readonly ILogger<ApplicationViewModel> _logger;

    public ApplicationViewModel(IMvxNavigationService navigationService, ILogger<ApplicationViewModel> logger)
    {
        _navigationService = navigationService;
        _logger = logger;
    }

    public override void Start()
    {
        Trace.WriteLine("test trace");
        _logger.LogInformation("Starting application");

        Task.Run(() => LoadQRH($"../../../../../samples/FA50.chklst"));
    }

    public async Task LoadQRH(string pathToFile)
    {
        var path = Path.GetFullPath(pathToFile);
        var result = await _navigationService.Navigate<QRHParsingViewModel, string, ParseResult<QuickReferenceHandbook>>(path);
        if (result == null || !result.IsSuccess()) return;
        
        _logger.LogDebug($"Loading QRH ViewModel for {result.Result?.AircraftName}");
        var quickReferenceHandbook = result.Result!;
        
        await OpenQRH(quickReferenceHandbook);
    }

    private async Task OpenQRH(QuickReferenceHandbook quickReferenceHandbook)
    {
        var redirect =
            await _navigationService.Navigate<QRHViewModel, QuickReferenceHandbook, QRHViewModelResult>(
                quickReferenceHandbook);

        if (redirect?.RedirectTo == null) return;

        await OpenQRH(redirect.RedirectTo);
    }
}
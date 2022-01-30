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

        Task.Run(LoadQRH);
    }

    public async Task LoadQRH()
    {
        var path = Path.GetFullPath($"../../../../../samples/FA50.chklst");
        var result = await _navigationService.Navigate<QRHParsingViewModel, string, ParseResult<QuickReferenceHandbook>>(path);
        if (result == null || !result.IsSuccess()) return;
        
        _logger.LogDebug($"Loading QRH ViewModel for {result.Result?.AircraftName}");
        await _navigationService.Navigate<QRHViewModel, QuickReferenceHandbook>(result.Result!);
    }
}
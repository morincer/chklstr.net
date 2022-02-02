using Chklstr.Core.Model;
using Chklstr.Core.Services;
using Chklstr.UI.Core.ViewModels;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;

namespace Chklstr.UI.Core;

public class GlobalActions
{
    private readonly ILogger<GlobalActions> _log;
    private readonly IMvxNavigationService _navigationService;

    public GlobalActions(ILogger<GlobalActions> log, IMvxNavigationService navigationService)
    {
        _log = log;
        _navigationService = navigationService;
    }
    
    public async Task<ParseResult<QuickReferenceHandbook>?> TryOpenAndParse(String pathToFile)
    {
        _log.LogInformation($"Trying to load {pathToFile}");
        var path = Path.GetFullPath(pathToFile);
        var result = await _navigationService.Navigate<QRHParsingViewModel, string, ParseResult<QuickReferenceHandbook>>(path);
        return result;
    }
}
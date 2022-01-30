using Chklstr.Core.Model;
using Chklstr.Core.Services;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Chklstr.UI.Core.ViewModels;

public class QRHParsingViewModel : MvxViewModel<string, ParseResult<QuickReferenceHandbook>>
{
    private readonly IQRHParserService _parserService;
    private readonly IMvxNavigationService _navigationService;
    private readonly ILogger<QRHParsingViewModel> _logger;
    public string Path { get; private set; }

    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private string _message;

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public ParseResult<QuickReferenceHandbook> ParseResult { get; private set; }

    public QRHParsingViewModel(IQRHParserService parserService,
        IMvxNavigationService _navigationService,
        ILogger<QRHParsingViewModel> logger)
    {
        _parserService = parserService;
        this._navigationService = _navigationService;
        _logger = logger;
    }

    public override void Prepare(string path)
    {
        Path = path;
    }

    public override void ViewAppeared()
    {
        _logger.LogDebug($"Parsing {Path}");
        Task.Run(Parse);
    }

    public async Task Parse()
    {
        try
        {
            IsLoading = true;
            Message = $"Loading QRH from {Path}";
            _logger.LogInformation(Message);

            var input = await File.ReadAllTextAsync(Path);
            ParseResult = _parserService.parse(input);

            if (ParseResult.IsSuccess())
            {
                _logger.LogDebug("QRH Loaded successfully");
                await Close();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            ParseResult = ParseResult<QuickReferenceHandbook>.Failed(e.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task Close()
    {
        await _navigationService.Close(this, ParseResult);
    }
}
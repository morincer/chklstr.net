using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Chklstr.Core.Services.Voice;
using Chklstr.UI.Core.ViewModels;
using Microsoft.Extensions.Logging;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using MvvmCross.WeakSubscription;

namespace Chklstr.UI.WPF.Voice;

public class QRHVoiceView
{
    private readonly ILogger _log;
    private readonly IVoiceCommandDetectionService _voiceCommandDetectionService;
    private readonly ITextToSpeechService _textToSpeechService;
    public QRHViewModel ViewModel { get; }
    
    public QRHVoiceView(QRHViewModel viewModel, 
        ILogger<QRHVoiceView> log,
        IVoiceCommandDetectionService voiceCommandDetectionService,
        ITextToSpeechService textToSpeechService
    )
    {
        this._log = log;
        _voiceCommandDetectionService = voiceCommandDetectionService;
        _textToSpeechService = textToSpeechService;
        ViewModel = viewModel;
    }

    public void Prepare()
    {
        _log.LogDebug($"Binding QRH {ViewModel.AircraftName} to voice controller view");
        
        _voiceCommandDetectionService.Stop();
        _textToSpeechService.Stop();

        ViewModel.IsVoiceEnabled = false;
        ViewModel.WeakSubscribe(() => ViewModel.SelectedChecklist, OnSelectedChecklistChanged);
        ViewModel.WeakSubscribe(() => ViewModel.IsVoiceEnabled, OnVoiceEnabledChanged);
        
        foreach (var cl in ViewModel.Checklists)
        {
            cl.WeakSubscribe(() => cl.SelectedItem, OnSelectedItemChanged);
            cl.WeakSubscribe(() => cl.HasActiveItems, OnHasActiveItemsChanged);
        }
    }

    private void OnHasActiveItemsChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!ViewModel.IsVoiceEnabled || ViewModel.SelectedChecklist == null ||
            ViewModel.SelectedChecklist.HasActiveItems) return;
        
        _textToSpeechService.SayAsync($"{ViewModel.SelectedChecklist.Name} checklist complete");
        ViewModel.IsVoiceEnabled = false;
    }

    private void OnSelectedItemChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!CanVoiceSelection()) return;

        var checklist = ViewModel.SelectedChecklist!;
        var selectedItem = checklist.SelectedItem!;

        var priority = true;
        
        if (checklist.CheckedItemsCount == 0)
        {
            _textToSpeechService.SayAsync($"{checklist.Name} checklist started.");
            priority = false;
        }

        if (selectedItem.IsSelectable)
        {
            _textToSpeechService.SayAsync($"{CleanForVoice(selectedItem.Title)}, {CleanForVoice(selectedItem.Text)}", priority);
        }
    }

    private string CleanForVoice(string src)
    {
        var result = src.Replace("/", ", ").Trim();

        result = Regex.Replace(result, "[\r\n\t]", " ");
        
        return result;
    }

    private bool CanVoiceSelection()
    {
        return ViewModel.IsVoiceEnabled 
               && ViewModel.SelectedChecklist != null 
               && ViewModel.SelectedChecklist.SelectedItem != null;
    }

    private void OnVoiceEnabledChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!ViewModel.IsVoiceEnabled)
        {
            _voiceCommandDetectionService.Stop();
            _textToSpeechService.Stop();
        }
        
        OnSelectedItemChanged(sender, e);
    }

    private void OnSelectedChecklistChanged(object? sender, PropertyChangedEventArgs e)
    {
        _voiceCommandDetectionService.Stop();
        _textToSpeechService.Stop();
        ViewModel.IsVoiceEnabled = false;
    }

    
}


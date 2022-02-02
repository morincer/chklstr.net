using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
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

    private SoundPlayer _checkedSound;
    
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

        InitializeSounds();       
        
        _voiceCommandDetectionService.Stop();
        _textToSpeechService.Stop();

        _voiceCommandDetectionService.VoiceCommandDetected += OnVoiceCommand;

        ViewModel.IsVoiceEnabled = false;
        ViewModel.WeakSubscribe(() => ViewModel.SelectedChecklist, OnSelectedChecklistChanged);
        ViewModel.WeakSubscribe(() => ViewModel.IsVoiceEnabled, OnVoiceEnabledChanged);

        foreach (var cl in ViewModel.Checklists)
        {
            cl.WeakSubscribe(() => cl.SelectedItem, OnSelectedItemChanged);
            cl.WeakSubscribe(() => cl.HasActiveItems, OnHasActiveItemsChanged);
        }
    }

    private void InitializeSounds()
    {
        _checkedSound = new SoundPlayer(GetResourceStreamByPostfix("checked.wav"));
    }
    

    private Stream GetResourceStreamByPostfix(string postfix)
    {
        var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        var resourceName = names.FirstOrDefault(n => n.EndsWith(postfix));

        if (resourceName == null)
        {
            _log.LogError($"Failed to find resource by postfix {postfix}. Available names are {string.Join(", ", names)}");
            throw new Exception("Failed to load resource by postfix " + postfix);
        }

        return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)!;
    }

    private void OnVoiceCommand(object sender, VoiceCommand args)
    {
        if (!IsSelectionVoiceable()) return;
        _voiceCommandDetectionService.Stop();

        switch (args)
        {
            case VoiceCommand.Check:
                if (ViewModel.SelectedChecklist!.CanCheckAndAdvance)
                {
                    _checkedSound.Play();
                    ViewModel.SelectedChecklist!.CheckAndAdvance();
                }

                break;
        }
    }

    private void OnHasActiveItemsChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!ViewModel.IsVoiceEnabled || ViewModel.SelectedChecklist == null ||
            ViewModel.SelectedChecklist.HasActiveItems) return;

        _textToSpeechService.SayAsync($"{ViewModel.SelectedChecklist.Name} checklist complete");
        ViewModel.IsVoiceEnabled = false;
        
        _voiceCommandDetectionService.Stop();
    }

    private void OnSelectedItemChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!IsSelectionVoiceable()) return;

        var checklist = ViewModel.SelectedChecklist!;
        var selectedItem = checklist.SelectedItem!;

        var priority = true;

        if (checklist.CheckedItemsCount == 0)
        {
            _textToSpeechService.SayAsync($"{checklist.Name} checklist started.");
            priority = false;
        }

        _voiceCommandDetectionService.Stop();
        _textToSpeechService.SayAsync($"{CleanForVoice(selectedItem.Title)}, {CleanForVoice(selectedItem.Text)}",
            priority);
        _voiceCommandDetectionService.Start();
    }

    private string CleanForVoice(string src)
    {
        var result = src.Replace("/", ", ").Trim();

        result = Regex.Replace(result, "[\r\n\t]", " ");

        return result;
    }

    private bool IsSelectionVoiceable()
    {
        return ViewModel.IsVoiceEnabled
               && ViewModel.SelectedChecklist != null
               && ViewModel.SelectedChecklist.SelectedItem != null
               && ViewModel.SelectedChecklist.SelectedItem.IsSelectable;
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
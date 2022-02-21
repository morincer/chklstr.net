using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Chklstr.Core.Services.Voice;
using Chklstr.Core.Utils;
using Chklstr.UI.Core.Services;
using Chklstr.UI.Core.ViewModels;
using Microsoft.Extensions.Logging;
using MvvmCross.WeakSubscription;

namespace Chklstr.UI.WPF.Voice;

public class QRHVoiceView
{
    private readonly ILogger _log;
    private readonly IVoiceCommandDetectionService _voiceCommandDetectionService;
    private readonly ITextToSpeechService _textToSpeechService;
    private readonly IUserSettingsService _userSettingsService;
    public QRHViewModel ViewModel { get; }

    private SoundPlayer _checkedSound;

    private SynchronizationContext _context;

    public QRHVoiceView(QRHViewModel viewModel,
        ILogger<QRHVoiceView> log,
        IVoiceCommandDetectionService voiceCommandDetectionService,
        ITextToSpeechService textToSpeechService,
        IUserSettingsService userSettingsService
    )
    {
        _log = log;
        _log.LogDebug("Created new voice view service");
        _voiceCommandDetectionService = voiceCommandDetectionService;
        _textToSpeechService = textToSpeechService;
        _userSettingsService = userSettingsService;
        ViewModel = viewModel;

        _context = SynchronizationContext.Current!;
    }

    private List<MvxNotifyPropertyChangedEventSubscription> _subscriptions = new();

    public void Prepare()
    {
        _log.LogDebug($"Binding QRH {ViewModel.AircraftName} to voice controller view");

        InitializeSounds();

        StopVoiceDetection();
        _textToSpeechService.Stop();

        _voiceCommandDetectionService.VoiceCommandDetected += OnVoiceCommand;

        var config = _userSettingsService.Load();

        _textToSpeechService.VoiceName = config.SelectedVoice ?? _textToSpeechService.Voices.First();

        ViewModel.IsTextToSpeechEnabled = false;
        _subscriptions.Add(ViewModel.WeakSubscribe(() => ViewModel.SelectedChecklist, OnSelectedChecklistChanged));
        _subscriptions.Add(ViewModel.WeakSubscribe(() => ViewModel.IsTextToSpeechEnabled, OnVoiceEnabledChanged));
        _subscriptions.Add(ViewModel.WeakSubscribe(() => ViewModel.IsVoiceControlEnabled, OnVoiceEnabledChanged));

        foreach (var cl in ViewModel.Checklists)
        {
            _subscriptions.Add(cl.WeakSubscribe(() => cl.SelectedItem, OnSelectedItemChanged));
            _subscriptions.Add(cl.WeakSubscribe(() => cl.HasActiveItems, OnHasActiveItemsChanged));
        }
    }

    private void RunOnUI(Action action)
    {
        _context.Post(_ => action.Invoke(), null);
    }

    public void ViewDisappearing()
    {
        _log.LogDebug($"Un-binging {ViewModel.AircraftName} from voice view");
        StopVoiceDetection();
        _textToSpeechService.Stop();
        _voiceCommandDetectionService.VoiceCommandDetected -= OnVoiceCommand;
        foreach (var subscription in _subscriptions)
        {
            subscription.Dispose();
        }

        _subscriptions.Clear();
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
            _log.LogError(
                $"Failed to find resource by postfix {postfix}. Available names are {string.Join(", ", names)}");
            throw new Exception("Failed to load resource by postfix " + postfix);
        }

        return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)!;
    }

    private void OnVoiceCommand(object sender, VoiceCommand args)
    {
        RunOnUI(() =>
        {
            if (!IsSelectionVoiceable()) return;
            StopVoiceDetection();

            if (!ViewModel.IsVoiceControlEnabled) return;

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
        });
    }

    private void OnHasActiveItemsChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!ViewModel.IsTextToSpeechEnabled || ViewModel.SelectedChecklist == null ||
            ViewModel.SelectedChecklist.HasActiveItems) return;

        SayAsync($"{ViewModel.SelectedChecklist.Name} checklist complete")
            .ContinueWith(_ => { ViewModel.IsTextToSpeechEnabled = false; });

        StopVoiceDetection();
    }

    private void OnSelectedItemChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!IsSelectionVoiceable()) return;

        var checklist = ViewModel.SelectedChecklist!;
        var selectedItem = checklist.SelectedItem!;

        var priority = true;

        if (checklist.CheckedItemsCount == 0)
        {
            SayAsync($"{checklist.Name} checklist started.");
            priority = false;
        }

        if (selectedItem.SectionName != null)
        {
            var activeItems = checklist.Children
                .Where(c => c.IsSelectable && c.Item.IsAvailableInContext(checklist.Contexts))
                .ToList();

            var sectionItems = activeItems
                .Where(i => selectedItem.SectionName.Equals(i.SectionName)).ToList();

            if (!sectionItems.Any(i => i.IsChecked))
            {
                SayAsync($"{selectedItem.SectionName} section started");
                priority = false;
            }
        }

        StopVoiceDetection();
        SayAsync($"{CleanForVoice(selectedItem.Title)}, {CleanForVoice(selectedItem.Text)}",
            priority);

        if (ViewModel.IsVoiceControlEnabled)
        {
            StartVoiceDetection();
        }
    }

    private void StopVoiceDetection()
    {
        _voiceCommandDetectionService.Stop();
        ViewModel.IsListening = false;
    }

    private void StartVoiceDetection()
    {
        _voiceCommandDetectionService.Start();
        ViewModel.IsListening = true;
    }

    private HashSet<Task> _sayings = new();

    private Task SayAsync(string text, bool priority = false)
    {
        var task = _textToSpeechService.SayAsync(text, priority);

        _sayings.Add(task);

        UpdateTTSStatus();

        return task.ContinueWith(_ => RunOnUI(UpdateTTSStatus));
    }

    private void UpdateTTSStatus()
    {
        _sayings.RemoveWhere(t => t.IsCompleted);
        ViewModel.IsSaying = !_sayings.IsEmpty();
    }

    private string CleanForVoice(string src)
    {
        var result = src.Replace("/", ", ").Trim();

        result = Regex.Replace(result, "[\r\n\t]", " ");

        return result;
    }

    private bool IsSelectionVoiceable()
    {
        return ViewModel.IsTextToSpeechEnabled
               && ViewModel.SelectedChecklist != null
               && ViewModel.SelectedChecklist.SelectedItem != null
               && ViewModel.SelectedChecklist.SelectedItem.IsSelectable;
    }

    private void OnVoiceEnabledChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!ViewModel.IsTextToSpeechEnabled)
        {
            _textToSpeechService.Stop();
            StopVoiceDetection();
        }

        if (!ViewModel.IsVoiceControlEnabled)
        {
            StopVoiceDetection();
        }

        OnSelectedItemChanged(sender, e);
        UpdateTTSStatus();
    }

    private void OnSelectedChecklistChanged(object? sender, PropertyChangedEventArgs e)
    {
        StopVoiceDetection();
        _textToSpeechService.Stop();
        ViewModel.IsTextToSpeechEnabled = false;
    }
}
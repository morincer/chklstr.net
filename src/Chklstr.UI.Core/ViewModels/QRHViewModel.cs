using System.Collections.ObjectModel;
using Chklstr.Core.Model;
using Chklstr.Core.Utils;
using Chklstr.UI.Core.Utils;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Binding.Extensions;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Chklstr.UI.Core.ViewModels;

public class QRHViewModel : MvxViewModel<QuickReferenceHandbook>
{
    private readonly ILogger<QRHViewModel> _logger;
    public QuickReferenceHandbook Item { get; private set; }
    public string AircraftName { get; set; }
    public ObservableCollection<ChecklistViewModel> Checklists { get; init; } = new();

    public ObservableCollection<ContextViewModel> Contexts { get; init; } = new();

    private ChecklistViewModel? _selectedChecklist;

    public ChecklistViewModel? SelectedChecklist
    {
        get => _selectedChecklist;
        set => SetProperty(ref _selectedChecklist, value);
    }

    private bool _isVoiceEnabled;

    public bool IsVoiceEnabled
    {
        get => _isVoiceEnabled;
        set => SetProperty(ref _isVoiceEnabled, value);
    }
    
    public MvxCommand SpeakCommand => new(ToggleSpeak);
    
    public void ToggleSpeak()
    {
        IsVoiceEnabled = !IsVoiceEnabled;
    }

    public String[] SelectedContexts
    {
        get => Contexts.Where(c => c.Selected).Select(c => c.Name).ToArray();
    }

    public QRHViewModel(ILogger<QRHViewModel> logger)
    {
        _logger = logger;
    }

    public override void Prepare(QuickReferenceHandbook book)
    {
        Item = book;
    }

    public override async Task Initialize()
    {
        AircraftName = Item.AircraftName;

        var level = new HierarchyLevel();

        foreach (var ctx in Item.GetAllAvailableContexts())
        {
            var viewModel = Mvx.IoCProvider.IoCConstruct<ContextViewModel>();
            viewModel.Prepare(ctx);
            await viewModel.Initialize();
            viewModel.Selected = Item.DefaultContexts.Contains(ctx);
            viewModel.PropertyChanged += (sender, args) => UpdateContexts();
            Contexts.Add(viewModel);
        }
        
        foreach (var checklist in Item.Checklists)
        {
            try
            {
                var viewModel = Mvx.IoCProvider.IoCConstruct<ChecklistViewModel>();
                viewModel.QRH = this;
                viewModel.Prepare(checklist);
                await viewModel.Initialize();
                viewModel.ListNumber = level.ToString();
                level = level.Next();
                Checklists.Add(viewModel);
                _logger.LogDebug($"Adding checklist {viewModel.Name}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
        
        UpdateContexts();
        SelectedChecklist = Checklists.FirstOrDefault(cl => cl.IsEnabled);
    }

    private void UpdateContexts()
    {
        var ctxs = SelectedContexts;
        
        foreach (var checklistViewModel in Checklists)
        {
            checklistViewModel.Contexts = ctxs;
        }
    }
}
using System.Collections.ObjectModel;
using Chklstr.Core.Model;
using Chklstr.UI.Core.Utils;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MvvmCross.WeakSubscription;
using Serilog;

namespace Chklstr.UI.Core.ViewModels;

public class ChecklistViewModel : MvxViewModel<Checklist>
{
    public delegate void ChecklistChangedHandler(ChecklistViewModel sender, EventArgs args);
    public Checklist Item { get; private set; }
    
    public string ListNumber { get; set; }
    
    public string Name { get; set; }
    
    private string[] _contexts = Array.Empty<string>();
    public string[] Contexts
    {
        get => _contexts;
        set
        {
            _contexts = value;
            Update();
        }
    }

    public ICollection<string> ApplicableContexts => Item.Contexts;

    private int _checkableItemsCount;
    public int CheckableItemsCount
    {
        get => _checkableItemsCount;
        set
        {
            SetProperty(ref _checkableItemsCount, value);
            RaisePropertyChanged(() => IsComplete);
            RaisePropertyChanged(() => IsEnabled);
        }
    }

    private int _checkedItemsCount;

    public int CheckedItemsCount
    {
        get => _checkedItemsCount;
        set
        {
            SetProperty(ref _checkedItemsCount, value);
            RaisePropertyChanged(() => IsComplete);
            RaisePropertyChanged(() => IsEnabled);
        }
    }

    public ObservableCollection<ChecklistItemViewModel> Children { get; init; } = new();

    private ChecklistItemViewModel? _selectedItem;

    public bool HasActiveItems => Children.Any(c => c.IsSelectable && c.IsEnabled && !c.IsChecked);
    
    public bool CanCheckAndAdvance => !IsComplete && IsEnabled && HasActiveItems;

    public MvxCommand CheckAndAdvanceCommand => new(CheckAndAdvance);

    public MvxInteraction<ChecklistItemViewModel> ScrollIntoViewInteraction { get; init; } = new();
    
    public void CheckAndAdvance()
    {
        if (!CanCheckAndAdvance) return;
        SelectedItem!.IsChecked = true;
        SelectedItem = GetNextActiveItem(Children);

        if (SelectedItem != null)
        {
            ScrollIntoViewInteraction.Raise(SelectedItem);
        }
    }

    public MvxCommand SkipCommand => new(Skip);

    public void Skip()
    {
        if (SelectedItem == null) return;

        var items = Children
            .SkipWhile(c => c != SelectedItem)
            .Skip(1);

        var nextItem = GetNextActiveItem(items);

        SelectedItem = nextItem ?? GetNextActiveItem(Children);

        if (SelectedItem != null)
        {
            ScrollIntoViewInteraction.Raise(SelectedItem);
        }
    }


    public ChecklistItemViewModel? SelectedItem
    {
        get => _selectedItem;
        set
        {
            SetProperty(ref _selectedItem, value);
        }
    }

    public bool IsEnabled => _checkableItemsCount != 0;

    public bool IsComplete => _checkedItemsCount == _checkableItemsCount && _checkableItemsCount != 0;

    private List<MvxNamedNotifyPropertyChangedEventSubscription<bool>> _listeners = new();
    
    public override void Prepare(Checklist checklist)
    {
        Item = checklist;
        Name = Item.Name;
    }

    public override async Task Initialize()
    {
        await InitializeChecklist(Item, new HierarchyLevel());
    }

    public ChecklistItemViewModel? GetNextActiveItem(IEnumerable<ChecklistItemViewModel> items)
    {
        return items
            .FirstOrDefault(c => c.IsSelectable
                                 && !c.IsChecked
                                 && c.Item.IsAvailableInContext(Contexts));
    }

    private async Task InitializeChecklist(Checklist checklist, HierarchyLevel level)
    {
        foreach(var item in checklist.Items)
        {
            var viewModel = Mvx.IoCProvider.IoCConstruct<ChecklistItemViewModel>(new Dictionary<string, object>()
            {
                ["parent"] = this
            });
            viewModel.Prepare(item);
            await viewModel.Initialize();

            viewModel.ListNumber = level.ToString();
            
            Children.Add(viewModel);
            
            if (item is Checklist cl)
            {
                level = level.Indent();
                await InitializeChecklist(cl, level);
                level = level.Unindent();
            } else if (item is SingleCheckItem)
            {
                var listener = viewModel.WeakSubscribe(() => viewModel.IsChecked,
                    (sender, args) => Update());
                _listeners.Add(listener);
            } 

            if (!(item is Separator))
            {
                level = level.Next();
            }
        }
        
        Update();
    }

    public void Update()
    {
        foreach (var item in Children)
        {
            item.Update();
        }
        
        CheckableItemsCount = Item.GetCountCheckable(Contexts);
        CheckedItemsCount = Item.GetCountChecked(Contexts);

        if (IsEnabled && (SelectedItem == null || !SelectedItem.Item.IsAvailableInContext(Contexts)))
        {
            SelectedItem = GetNextActiveItem(Children);
        }

        RaisePropertyChanged(() => CanCheckAndAdvance);
        RaisePropertyChanged(() => CanReset);
    }

    public override void ViewDestroy(bool viewFinishing = true)
    {
        foreach (var listener in _listeners)
        {
            listener.Dispose();
        }
    }

    public bool CanReset => CheckedItemsCount > 0 && IsEnabled;
    public MvxCommand ResetCommand => new(Reset);
    public void Reset()
    {
        foreach (var item in Children)
        {
            item.IsChecked = false;
        }

        SelectedItem = GetNextActiveItem(Children);
        
        if (SelectedItem != null)
        {
            ScrollIntoViewInteraction.Raise(SelectedItem);
        }
    }
}

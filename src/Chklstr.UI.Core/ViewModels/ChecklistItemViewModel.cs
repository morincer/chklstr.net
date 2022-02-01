using Chklstr.Core.Model;
using MvvmCross.ViewModels;

namespace Chklstr.UI.Core.ViewModels;

public class ChecklistItemViewModel : MvxViewModel<ChecklistItem>
{
    public ChecklistItem Item { get; private set; }

    public string ListNumber { get; set; } = "";
    public string Title { get; set; } = "";
    public string Text { get; set; } = "";
    public string? DescriptionMarkdown { get; set; }
    
    public bool IsSeparator { get; set; }
    public bool IsIndent { get; set; }

    public bool IsSelectable { get; set; } = true;

    private bool _isChecked;

    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            Item.Checked = value;
            SetProperty(ref _isChecked, value);
            Update();
        }
    }

    public ICollection<string> ApplicableContexts => Item.Contexts;

    public ChecklistViewModel Parent { get; init; }

    private bool _isEnabled;

    public bool IsEnabled
    {
        get => _isEnabled;
        set => SetProperty(ref _isEnabled, value);
    }

    public ChecklistItemViewModel(ChecklistViewModel parent)
    {
        Parent = parent;
    }

    public override void Prepare(ChecklistItem checklistItem)
    {
        Item = checklistItem;
        DescriptionMarkdown = checklistItem.Description;

        switch (Item)
        {
            case SingleCheckItem singleCheckItem:
                Title = singleCheckItem.CheckName;
                Text = singleCheckItem.Value;
                IsIndent = singleCheckItem.Parent != null;
                break;
            case Checklist checklist:
                Title = checklist.Name;
                IsSelectable = false;
                break;
            case Separator:
                IsSeparator = true;
                break;
            default:
                throw new NotImplementedException($"Unknown item type: {checklistItem.GetType()}");
        }

        Update();
    }

    /*public override async Task Initialize()
    {
        if (!string.IsNullOrEmpty(DescriptionMarkdown))
        {
            await Task.Run(() =>
            {
                
            });
        }
    }*/

    public void Update()
    {
        IsEnabled = Item.IsAvailableInContext(Parent.Contexts);
    }
}
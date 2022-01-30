using Chklstr.Core.Model;
using MvvmCross.ViewModels;

namespace Chklstr.UI.Core.ViewModels;

public class ChecklistItemViewModel : MvxViewModel<ChecklistItem>
{
    public ChecklistItem Item { get; private set; }

    public string ListNumber { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string DescriptionMarkdown { get; set; }

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

    private bool _IsEnabled;

    public bool IsEnabled
    {
        get => _IsEnabled;
        set => SetProperty(ref _IsEnabled, value);
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
    }

    public void Update(params string[] contexts)
    {
        IsEnabled = Item.IsAvailableInContext(contexts);
    }
}
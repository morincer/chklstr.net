using MvvmCross.ViewModels;

namespace Chklstr.UI.Core.ViewModels;

public class ContextViewModel : MvxViewModel<string>
{
    public string Name { get; set; }
    private bool _selected;

    public bool Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public override void Prepare(string name)
    {
        Name = name;
    }
}
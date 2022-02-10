using System.Collections;
using System.ComponentModel;
using MvvmCross.ViewModels;
using MvvmValidation;

namespace Chklstr.UI.Core.ViewModels;

public abstract class ValidatableMvxViewModel : MvxViewModel, INotifyDataErrorInfo
{
    protected ValidationHelper Validator { get; }
    private NotifyDataErrorInfoAdapter NotifyDataErrorInfoAdapter { get; }

    protected ValidatableMvxViewModel()
    {
        Validator = new ValidationHelper();
        NotifyDataErrorInfoAdapter = new NotifyDataErrorInfoAdapter(Validator);
        PropertyChanged += (sender, args) =>
        {
            if (nameof(HasErrors) == args.PropertyName) return;
            
            Validator.Validate(args.PropertyName);
            RaisePropertyChanged(nameof(HasErrors));
        };
    }

    public IEnumerable GetErrors(string? propertyName)
    {
        return NotifyDataErrorInfoAdapter.GetErrors(propertyName);
    }

    public bool HasErrors => NotifyDataErrorInfoAdapter.HasErrors;

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged
    {
        add => NotifyDataErrorInfoAdapter.ErrorsChanged += value;
        remove => NotifyDataErrorInfoAdapter.ErrorsChanged -= value;
    }
}
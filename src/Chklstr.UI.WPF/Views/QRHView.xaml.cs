using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Chklstr.Core.Model;
using Chklstr.UI.Core.ViewModels;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Serilog;

namespace Chklstr.UI.WPF.Views;

public class ChecklistItemTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        var checklistItem = item as ChecklistItemViewModel;
        var element = (FrameworkElement) container;

        var key = $"Template{checklistItem?.Item.GetType().Name}";

        if (element.FindResource(key) is DataTemplate template)
        {
            return template;
        }

        Log.Logger.Warning($"Failed to identify data template for {item.GetType()}");
        return null;
    }
}

[MvxContentPresentation]
[MvxViewFor(typeof(QRHViewModel))]
public partial class QRHView : MvxWpfView<QRHViewModel>, IComponentConnector
{
    public QRHView()
    {
        InitializeComponent();
    }
}
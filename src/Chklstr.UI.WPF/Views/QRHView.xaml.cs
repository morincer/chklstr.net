using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Chklstr.Core.Model;
using Chklstr.UI.Core.ViewModels;
using Chklstr.UI.WPF.Utils;
using MvvmCross.Base;
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
        this.Loaded += OnLoaded;
    }
    
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        foreach (var checklist in ViewModel.Checklists)
        {
            checklist.ScrollIntoViewInteraction.Requested += OnInteractionRequested;
        }

        TabControl_Checklists.SelectionChanged += OnChecklistShown;
    }

    private void OnChecklistShown(object sender, SelectionChangedEventArgs e)
    {
        if (TabControl_Checklists.SelectedItem is not ChecklistViewModel checklistViewModel)
        {
            return;
        }

        if (checklistViewModel.SelectedItem == null) return;
        ScrollIntoItem(checklistViewModel.SelectedItem);
    }


    private void OnInteractionRequested(object? sender, MvxValueEventArgs<ChecklistItemViewModel> e)
    {
        var checklistItem = e.Value;
        if (checklistItem.Parent != ViewModel.SelectedChecklist) return;
        
        ScrollIntoItem(checklistItem);
    }

    private void ScrollIntoItem(ChecklistItemViewModel checklistItem)
    {
        var listBox = Helpers.FindChild<ListBox>(this, lb =>
            lb.DataContext == checklistItem.Parent);

        if (listBox == null)
        {
            Log.Error("Failed to find list box");
            return;
        }

        listBox.ScrollToCenterOfView(checklistItem);
    }
}
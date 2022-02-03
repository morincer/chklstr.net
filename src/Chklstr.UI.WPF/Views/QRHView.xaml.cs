using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;
using Chklstr.UI.Core.Utils;
using Chklstr.UI.Core.ViewModels;
using Chklstr.UI.WPF.Utils;
using Chklstr.UI.WPF.Voice;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Serilog;
using ListBox = System.Windows.Controls.ListBox;

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
    private QRHVoiceView voiceView;
    public QRHView()
    {
        InitializeComponent();
        this.Loaded += OnLoaded;
    }
    
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        foreach (var checklist in ViewModel.Checklists)
        {
            checklist.ScrollIntoViewInteraction.Requested += OnScrollIntoViewInteractionRequested;
        }

        TabControl_Checklists.SelectionChanged += OnChecklistShown;

        ViewModel.SelectFilePathInteraction.Requested += OnSelectedFileInteractionRequested;
        
        InitializeVoiceView();
    }

    private void OnSelectedFileInteractionRequested(object? sender, MvxValueEventArgs<SelectFileRequest> e)
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Multiselect = false;
        openFileDialog.Filter = $"Checklist ({e.Value.FileExtension}|{e.Value.FileExtension}";
        openFileDialog.InitialDirectory = e.Value.BaseFolder;

        var dialogResult = openFileDialog.ShowDialog();
        
        e.Value.FileSelectedCallback(dialogResult == DialogResult.OK ? openFileDialog.FileName : null);
    }

    private void InitializeVoiceView()
    {
        voiceView = Mvx.IoCProvider.IoCConstruct<QRHVoiceView>(new Dictionary<string, object>
        {
            ["viewModel"] = ViewModel
        });

        voiceView.Prepare();
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


    private void OnScrollIntoViewInteractionRequested(object? sender, MvxValueEventArgs<ChecklistItemViewModel> e)
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

        listBox.ScrollToCenterOfView(checklistItem, 500);
    }
}
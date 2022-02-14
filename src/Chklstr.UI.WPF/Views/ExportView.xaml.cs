using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;
using Chklstr.UI.Core.Utils;
using Chklstr.UI.Core.ViewModels;
using MvvmCross.Base;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

namespace Chklstr.UI.WPF.Views;

[MvxContentPresentation]
[MvxViewFor(typeof(ExportViewModel))]
public partial class ExportView : MvxWpfView<ExportViewModel>, IComponentConnector
{
    public ExportView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ViewModel.SelectFilePathInteraction.Requested += OnSelectedFileInteractionRequested;
    }


    private void OnSelectedFileInteractionRequested(object? sender, MvxValueEventArgs<SelectFileRequest> e)
    {
        var openFileDialog = new OpenFileDialog();
        openFileDialog.Multiselect = false;
        openFileDialog.Filter = $"{e.Value.FormatName} ({e.Value.FileExtension}|{e.Value.FileExtension}";
        openFileDialog.InitialDirectory = e.Value.BaseFolder;

        var dialogResult = openFileDialog.ShowDialog();
        
        e.Value.FileSelectedCallback(dialogResult == DialogResult.OK ? openFileDialog.FileName : null);
    }
}
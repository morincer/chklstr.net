using System.Windows;
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
[MvxViewFor(typeof(ApplicationViewModel))]
public partial class HomeView : MvxWpfView<ApplicationViewModel>, IComponentConnector
{
    public HomeView()
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
        openFileDialog.Filter = $"Checklist ({e.Value.FileExtension}|{e.Value.FileExtension}";
        openFileDialog.InitialDirectory = e.Value.BaseFolder;

        var dialogResult = openFileDialog.ShowDialog();
        
        e.Value.FileSelectedCallback(dialogResult == DialogResult.OK ? openFileDialog.FileName : null);
    }
}
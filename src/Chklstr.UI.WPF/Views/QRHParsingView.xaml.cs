using System.Windows;
using System.Windows.Controls;
using Chklstr.UI.Core.ViewModels;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

namespace Chklstr.UI.WPF.Views;

[MvxContentPresentation]
[MvxViewFor(typeof(QRHParsingViewModel))]
public partial class QRHParsingView : MvxWpfView {
    public QRHParsingView()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        ((QRHParsingViewModel) ViewModel).Close();
    }
}
using System.Windows;
using System.Windows.Markup;
using Chklstr.UI.Core.ViewModels;
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
    }
}
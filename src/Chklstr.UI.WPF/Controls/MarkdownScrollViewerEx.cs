using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Chklstr.UI.WPF.Utils;
using MdXaml;
using Serilog;

namespace Chklstr.UI.WPF.Controls;

public class MarkdownScrollViewerEx : MarkdownScrollViewer
{
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        e.Handled = false;
    }

    protected override void OnGotFocus(RoutedEventArgs e)
    {
        var item = (FrameworkElement) this;
        while (true)
        {
            if (item.Parent == null)
            {
                if (item.TemplatedParent == null)
                {
                    break;
                }

                item = (FrameworkElement) item.TemplatedParent;
            }
            else
            {
                item = (FrameworkElement) item.Parent;
            }

            if (item is ListBoxItem)
            {
                break;
            }
        }

        if (item is not ListBoxItem listBoxItem) return;

        if (listBoxItem.Focusable)
        {
            listBoxItem.IsSelected = true;
        }
    }
}
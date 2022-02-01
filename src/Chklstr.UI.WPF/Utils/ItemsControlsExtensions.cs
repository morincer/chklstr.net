using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Chklstr.UI.WPF.Utils;

public static class ItemsControlExtensions
{
    public static DependencyProperty VerticalOffsetAnimatedProperty = DependencyProperty.RegisterAttached(
        "VerticalOffsetAnimated", typeof(double), typeof(ScrollViewer), 
        new UIPropertyMetadata((o, args) =>
        {
            if (o is not ScrollViewer scrollViewer)
            {
                return;
            }
            
            scrollViewer.ScrollToVerticalOffset((double) args.NewValue);
        }));
    
    public static void ScrollToCenterOfView(this ItemsControl itemsControl, object item, int animate = 0)
    {
        // Scroll immediately if possible
        if (!itemsControl.TryScrollToCenterOfView(item, animate))
        {
            // Otherwise wait until everything is loaded, then scroll
            if (itemsControl is ListBox) ((ListBox) itemsControl).ScrollIntoView(item);
            itemsControl.Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                new Action(() => { itemsControl.TryScrollToCenterOfView(item, animate); }));
        }
    }

    private static bool TryScrollToCenterOfView(this ItemsControl itemsControl, object item, int animate)
    {
        // Find the container
        var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as UIElement;
        if (container == null) return false;

        // Find the ScrollContentPresenter
        ScrollContentPresenter? presenter = null;
        for (Visual? vis = container;
             vis != null && vis != itemsControl;
             vis = VisualTreeHelper.GetParent(vis) as Visual)
            if ((presenter = vis as ScrollContentPresenter) != null)
                break;
        if (presenter == null) return false;

        // Find the IScrollInfo
        var scrollInfo =
            !presenter.CanContentScroll
                ? presenter
                : presenter.Content as IScrollInfo ??
                  FirstVisualChild(presenter.Content as ItemsPresenter) as IScrollInfo ??
                  presenter;

        // Compute the center point of the container relative to the scrollInfo
        Size size = container.RenderSize;
        Point center = container.TransformToAncestor((Visual) scrollInfo)
            .Transform(new Point(size.Width / 2, size.Height / 2));
        center.Y += scrollInfo.VerticalOffset;
        center.X += scrollInfo.HorizontalOffset;

        // Adjust for logical scrolling
        if (scrollInfo is StackPanel || scrollInfo is VirtualizingStackPanel)
        {
            double logicalCenter = itemsControl.ItemContainerGenerator.IndexFromContainer(container) + 0.5;
            Orientation orientation = scrollInfo is StackPanel
                ? ((StackPanel) scrollInfo).Orientation
                : ((VirtualizingStackPanel) scrollInfo).Orientation;
            if (orientation == Orientation.Horizontal)
                center.X = logicalCenter;
            else
                center.Y = logicalCenter;
        }
        
        // Scroll the center of the container to the center of the viewport
        if (scrollInfo.CanVerticallyScroll)
        {
            var targetVerticalOffset = CenteringOffset(center.Y, scrollInfo.ViewportHeight, scrollInfo.ExtentHeight);

            if (animate == 0)
            {
                scrollInfo.SetVerticalOffset(targetVerticalOffset);
            }
            else
            {
                var verticalAnimation = new DoubleAnimation();
                verticalAnimation.From = scrollInfo.VerticalOffset;
                verticalAnimation.To = targetVerticalOffset;
                var easingFunction = new QuadraticEase();
                easingFunction.EasingMode = EasingMode.EaseOut;
                verticalAnimation.EasingFunction = easingFunction;
                verticalAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(animate));

                var storyboard = new Storyboard();
                storyboard.Children.Add(verticalAnimation);
                Storyboard.SetTarget(verticalAnimation, scrollInfo.ScrollOwner);
                Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath(VerticalOffsetAnimatedProperty));
                storyboard.Begin();
            }
        }

        if (scrollInfo.CanHorizontallyScroll)
            scrollInfo.SetHorizontalOffset(CenteringOffset(center.X, scrollInfo.ViewportWidth, scrollInfo.ExtentWidth));
        return true;
    }

    private static double CenteringOffset(double center, double viewport, double extent)
    {
        return Math.Min(extent - viewport, Math.Max(0, center - viewport / 2));
    }

    private static DependencyObject FirstVisualChild(Visual visual)
    {
        if (visual == null) return null;
        if (VisualTreeHelper.GetChildrenCount(visual) == 0) return null;
        return VisualTreeHelper.GetChild(visual, 0);
    }
}
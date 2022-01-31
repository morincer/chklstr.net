using System.Windows;
using System.Windows.Media;

namespace Chklstr.UI.WPF.Utils;

public class Helpers
{
    public static T? FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        //get parent item
        DependencyObject? parentObject = VisualTreeHelper.GetParent(child);

        //we've reached the end of the tree
        if (parentObject == null) return null;

        //check if the parent matches the type we're looking for
        if (parentObject is T parent)
            return parent;
        
        return FindParent<T>(parentObject);
    }
    
    public static T? GetAncestorOfType<T>(FrameworkElement child) where T : FrameworkElement
    {
        var parent = VisualTreeHelper.GetParent(child);
        
        if (parent != null && parent is not T) 
            return GetAncestorOfType<T>((FrameworkElement)parent);
        
        return (T?) parent;
    }
}
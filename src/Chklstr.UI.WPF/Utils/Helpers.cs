using System;
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
    
    // <summary>
    /// Finds a Child of a given item in the visual tree. 
    /// </summary>
    /// <param name="parent">A direct parent of the queried item.</param>
    /// <typeparam name="T">The type of the queried item.</typeparam>
    /// <param name="childName">x:Name or Name of child. </param>
    /// <returns>The first parent item that matches the submitted type parameter. 
    /// If not matching item can be found, 
    /// a null parent is being returned.</returns>
    public static T? FindChild<T>(DependencyObject? parent, Func<T, bool>? selector)
        where T : DependencyObject
    {    
        // Confirm parent and childName are valid. 
        if (parent == null) return null;

        T? foundChild = null;

        int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            // If the child is not of the request child type child
            if (child is not T childType)
            {
                // recursively drill down the tree
                foundChild = FindChild(child, selector);

                // If the child is found, break so we do not overwrite the found child. 
                if (foundChild != null) break;
            }
            else if (selector != null)
            {
                if (selector(childType))
                {
                    foundChild = childType;
                    break;
                }
                /*var frameworkElement = child as FrameworkElement;
                // If the child's name is set for search
                if (frameworkElement != null && frameworkElement.Name == childName)
                {
                    // if the child's name is of the request name
                    foundChild = (T)child;
                    break;
                }*/
            }
            else
            {
                // child element found.
                foundChild = childType;
                break;
            }
        }

        return foundChild;
    }
}
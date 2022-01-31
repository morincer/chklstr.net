using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Chklstr.UI.WPF.Utils;

[ValueConversion(typeof(ICollection<string>), typeof(Visibility))]
public class HasItemsToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not ICollection<string> collection)
        {
            throw new InvalidOperationException("The value must be a collection of strings");
        }

        return collection.Any() ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
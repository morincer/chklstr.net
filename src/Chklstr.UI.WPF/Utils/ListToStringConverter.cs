using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace Chklstr.UI.WPF.Utils;

[ValueConversion(typeof(ICollection<string>), typeof(string))]
public class ListToStringConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is not ICollection<string> collection)
        {
            throw new InvalidOperationException("The value must be a collection of strings");
        }
        
        if (parameter is not string delimiter) delimiter = "";

        return string.Join(delimiter, collection);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
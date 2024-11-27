using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace TimeTraveler.Converters;

public class DoubleToDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double radius)
        {
            return radius * 2; // 转换为直径
        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
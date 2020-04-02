using System;
using System.Windows.Data;

namespace Airlines.Converters
{
    // ковертирует true в false и наоборот
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
            => value is bool boolValue ? !boolValue : throw new ArgumentException();
        
        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
            => throw new NotSupportedException();
    }
}

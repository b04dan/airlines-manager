using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Airlines.Converters
{
    // конвертирует громкость звука(double значение, от 0 до 1) в bool
    class VolumeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is double doubleValue ? doubleValue <= 0 : DependencyProperty.UnsetValue;
        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool boolValue ? boolValue ? 0 : 0.5 : DependencyProperty.UnsetValue;
    }
}

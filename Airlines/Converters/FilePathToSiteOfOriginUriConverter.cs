using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Airlines.Converters
{
    // конвертирует переданное название файла в siteoforigin URI
    class FilePathToSiteOfOriginUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string fileName)) return null;

            var builder = new StringBuilder("pack://siteoforigin:,,,/");

            // в виде параметра может передаваться дополнительный путь
            if (parameter is string path)
                builder.Append(path);

            return new Uri($"{builder}/{fileName}", UriKind.Absolute);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}

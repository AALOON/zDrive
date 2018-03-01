using System;
using System.Windows.Data;

namespace zDrive.Converters
{
    [ValueConversion(typeof(double), typeof(bool))]
    public class BadPercentToBoolConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (double)value > 90D)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

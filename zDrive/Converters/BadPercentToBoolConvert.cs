using System;
using System.Globalization;
using System.Windows.Data;

namespace zDrive.Converters
{
    [ValueConversion(typeof(double), typeof(bool))]
    public class BadPercentToBoolConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (double)value > 90D)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}

using System;
using System.Windows;
using System.Windows.Data;

namespace zDrive.Converters
{
    [ValueConversion(typeof(System.IO.DriveType), typeof(string))]
    public class FlagToPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var driveType = Enum.GetName(typeof(System.IO.DriveType), value);
                if (!string.IsNullOrEmpty(driveType) && Application.Current.Resources.Contains(driveType))
                {
                    return Application.Current.Resources[driveType];
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

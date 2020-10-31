using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace zDrive.Converters
{
    [ValueConversion(typeof(DriveType), typeof(string))]
    public class FlagToPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var driveType = Enum.GetName(typeof(DriveType), value);
                if (!string.IsNullOrEmpty(driveType) && Application.Current.Resources.Contains(driveType))
                    return Application.Current.Resources[driveType];
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
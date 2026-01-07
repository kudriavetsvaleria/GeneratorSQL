using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace GeneratorSQL.Helpers.Converters
{
    /// <summary>
    /// Converter for language button background color
    /// </summary>
    public class LocaleToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentLocale = value as string;
            var targetLocale = parameter as string;
            
            return currentLocale == targetLocale 
                ? new SolidColorBrush(Color.FromRgb(33, 150, 243)) // Blue for selected
                : new SolidColorBrush(Colors.Transparent); // Transparent for unselected
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter for language button text color
    /// </summary>
    public class LocaleToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentLocale = value as string;
            var targetLocale = parameter as string;
            
            return currentLocale == targetLocale 
                ? new SolidColorBrush(Colors.White) // White for selected
                : new SolidColorBrush(Color.FromRgb(100, 100, 100)); // Gray for unselected
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converter for language button font weight
    /// </summary>
    public class LocaleToFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentLocale = value as string;
            var targetLocale = parameter as string;
            
            return currentLocale == targetLocale 
                ? FontWeights.Bold 
                : FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

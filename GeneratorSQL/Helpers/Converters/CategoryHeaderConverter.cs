using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using GeneratorSQL.Services;

namespace GeneratorSQL.Helpers.Converters
{
    public class CategoryHeaderConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length > 0 && values[0] != null)
            {
                // values[0] is likely the FieldCategory enum, so we need .ToString()
                var categoryName = values[0].ToString();
                
                // values[1] is the culture code, used just to trigger the update
                
                var resourceKey = $"Category_{categoryName}";
                var localized = LocalizationManager.Instance[resourceKey];
                
                // Return localized string if found, otherwise return original name
                return !string.IsNullOrEmpty(localized) ? localized : categoryName;
            }
            
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

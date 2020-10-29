using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MIS.Infomat.Converters
{
    internal class BooleanToVisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value is Boolean isVisible && isVisible)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

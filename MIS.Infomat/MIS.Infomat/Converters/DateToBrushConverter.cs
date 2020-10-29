using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MIS.Infomat.Converters
{
    internal class DateToBrushConverter : IMultiValueConverter
    {
        public Object Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
        {
            DateTime date;
            try
            {
                date = (DateTime)values[0];
            }
            catch
            {
                date = new DateTime();
            }

            Boolean isEnabled;
            try
            {
                isEnabled = (Boolean)values[1];
            }
            catch
            {
                isEnabled = true;
            }

            if (date != new DateTime() && isEnabled)
            {
                return date.Month % 2 == 0 ? Brushes.Blue : Brushes.Green;
            }

            return Brushes.DarkGray;
        }

        public Object[] ConvertBack(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
        {
            return new DependencyProperty[0];
        }
    }
}

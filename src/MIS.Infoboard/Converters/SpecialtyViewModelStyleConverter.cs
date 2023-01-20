using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;
using MIS.Application.ViewModels;
using MIS.Infoboard.Constants;

namespace MIS.Infoboard.Converters;

public class SpecialtyViewModelStyleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SpecialtyViewModel specialty)
        {
            return BindingNotification.UnsetValue;
        }

        if (parameter is not string option || string.IsNullOrEmpty(option))
        {
            return BindingNotification.UnsetValue;
        }
        
        var count = (int)specialty.Resources.Average(r => r.Count);
        var key = count switch
        {
            > 50 => StyleConstants.Success,
            > 25 => StyleConstants.Warning,
            > 0 => StyleConstants.Danger,
            0 => StyleConstants.Secondary,
            _ => StyleConstants.Default
        };

        return $"{key}{option}";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        BindingNotification.UnsetValue;
}

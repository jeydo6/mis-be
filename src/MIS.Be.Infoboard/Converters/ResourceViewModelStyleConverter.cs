using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using MIS.Be.Application.ViewModels;
using MIS.Be.Infoboard.Constants;

namespace MIS.Be.Infoboard.Converters;

public class ResourceViewModelStyleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ResourceViewModel resource)
        {
            return BindingNotification.UnsetValue;
        }

        if (parameter is not string option || string.IsNullOrEmpty(option))
        {
            return BindingNotification.UnsetValue;
        }

        var count = resource.Count;
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

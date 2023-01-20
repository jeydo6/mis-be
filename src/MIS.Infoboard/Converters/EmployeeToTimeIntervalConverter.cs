using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using MIS.Application.ViewModels;

namespace MIS.Infoboard.Converters;

public class EmployeeToTimeIntervalConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is EmployeeViewModel employee ?
            $"{employee.BeginDateTime:H:mm} - {employee.EndDateTime:H:mm}" :
            "нет приёма";

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        BindingNotification.UnsetValue;
}

using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Microsoft.Extensions.DependencyInjection;
using MIS.Application.ViewModels;
using MIS.Domain.Providers;

namespace MIS.Infoboard.Converters;

public class DateItemsToTimeIntervalConverter : IValueConverter
{
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public DateItemsToTimeIntervalConverter()
    {
        var serviceProvider = Avalonia.Application.Current.GetServiceProvider();

        _dateTimeProvider = serviceProvider.GetRequiredService<IDateTimeProvider>();
    }
    
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not DateItemViewModel[] dates)
        {
            return "нет приёма";
        }

        var date = dates.FirstOrDefault(di => di.Date.Date == _dateTimeProvider.Now.Date);
        return date is not null ?
            $"{date.BeginDateTime:H:mm} - {date.EndDateTime:H:mm}" :
            "нет приёма";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        BindingNotification.UnsetValue;
}

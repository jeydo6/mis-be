using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace MIS.Infoboard.Properties;

internal sealed class BindableStyle : AvaloniaObject
{
    static BindableStyle()
    {
        ClassesProperty.Changed.Subscribe(
            Observer.Create<AvaloniaPropertyChangedEventArgs<string>>(x =>
                HandleClassesChanged(x.Sender, x.NewValue.GetValueOrDefault<string>()))
        );
    }

    internal static readonly AttachedProperty<string> ClassesProperty =
        AvaloniaProperty.RegisterAttached<BindableStyle, IStyledElement, string>(
            "Classes", default!, false, BindingMode.OneTime);
    
    private static void HandleClassesChanged(IAvaloniaObject element, string? classes)
    {
        if (element is IStyledElement styled)
            styled.Classes = Classes.Parse(classes ?? "");
    }

    public static void SetClasses(AvaloniaObject element, string value) =>
        element.SetValue(ClassesProperty, value);

    public static string GetClasses(AvaloniaObject element) =>
        element.GetValue(ClassesProperty);
}

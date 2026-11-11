using System.Globalization;

namespace LifeManagementApp.Converters;

public class PriorityToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int priority)
        {
            return priority switch
            {
                5 => Colors.Crimson,
                4 => Colors.DarkOrange,
                3 => Colors.DodgerBlue,
                _ => Colors.SeaGreen
            };
        }

        return Colors.Gray;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
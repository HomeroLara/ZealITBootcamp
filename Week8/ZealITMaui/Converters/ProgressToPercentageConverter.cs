using System.Globalization;

namespace ZealITMaui.Converters;

public class ProgressToPercentageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int progress)
        {
            return progress / 100.0;
        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return 0;
    }
}
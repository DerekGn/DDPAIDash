using System;
using Windows.UI.Xaml.Data;

namespace DDPAIDash.Converter
{
    internal class IntToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (double) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (int) value;
        }
    }
}

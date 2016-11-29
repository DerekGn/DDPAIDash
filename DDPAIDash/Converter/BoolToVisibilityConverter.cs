using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace DDPAIDash.Converter
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility result = Visibility.Collapsed;
            
            if (System.Convert.ToBoolean(value))
            {
                result = Visibility.Visible;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool result = false;

            if((Visibility)value == Visibility.Visible)
            {
                result = true;
            }

            return result;
        }
    }

}

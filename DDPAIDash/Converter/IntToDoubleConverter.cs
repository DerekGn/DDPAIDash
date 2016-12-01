using System;
using Windows.UI.Xaml.Data;

namespace DDPAIDash.Converter
{
    internal class IntToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double result = 0;
            
            try
            {
                result = System.Convert.ToDouble(value);
            }
            catch (OverflowException)
            {
                      
            }

            return System.Convert.ToDouble(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            int result = 0;

            try
            {
                result = System.Convert.ToInt32(value);
            }
            catch (OverflowException)
            {
#warning TODO
            }

            return result;
        }
    }
}

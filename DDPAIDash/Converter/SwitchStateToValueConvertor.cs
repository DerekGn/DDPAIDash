using System;
using Windows.UI.Xaml.Data;
using DDPAIDash.Core.Types;

namespace DDPAIDash.Converter
{
    internal class SwitchStateToValueConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool result = false;

            if ((SwitchState) value == SwitchState.On)
            {
                result = true;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            SwitchState result = SwitchState.Off;

            if (System.Convert.ToBoolean(value))
            {
                result = SwitchState.On;
            }

            return result;
        }
    }
}

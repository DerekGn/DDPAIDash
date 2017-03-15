
using System;
using System.Globalization;

namespace DDPAIDash.ViewModels
{
    public abstract class EventBase : DeviceContent
    {
        protected static DateTime FormatEventName(string name)
        {
            var result = name.Substring(2, 14);

            return DateTime.ParseExact(result, "yyyyMMddHHmmss", CultureInfo.CurrentUICulture);
        }
    }
}

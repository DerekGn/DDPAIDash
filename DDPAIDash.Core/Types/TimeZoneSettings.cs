/**
* MIT License
*
* Copyright (c) 2016 Derek Goslin < http://corememorydump.blogspot.ie/ >
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System;
using System.Globalization;
using Newtonsoft.Json;

namespace DDPAIDash.Core.Types
{
    public class TimeZoneSettings
    {
        private static readonly Lazy<TimeZoneSettings> InstanceLazy =
            new Lazy<TimeZoneSettings>(() => new TimeZoneSettings());

        private TimeZoneSettings()
        {
            Date = DateTime.Now.ToString("yyyyMMddHHmmss");
            DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;
            Language = CultureInfo.CurrentCulture.Name;
            Imei = "012345678912345";
            TimeZone = 0;
        }

        public static TimeZoneSettings Instance => InstanceLazy.Value;

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("format")]
        public string DateFormat { get; set; }

        [JsonProperty("lang")]
        public string Language { get; set; }

        [JsonProperty("imei")]
        public string Imei { get; set; }

        [JsonProperty("time_zone")]
        public int TimeZone { get; set; }
    }
}
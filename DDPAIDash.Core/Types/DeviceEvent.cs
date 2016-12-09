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
using Newtonsoft.Json;
using DDPAIDash.Core.Json.Converters;

namespace DDPAIDash.Core
{
    public class DeviceEvent
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("imgname")]
        public string ImageName { get; set; }

        [JsonProperty("bvideoname")]
        public string BVideoName { get; set; }

        [JsonProperty("svideoname")]
        public string SVideoName { get; set; }

        [JsonProperty("bstarttime")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? BStartTime { get; set; }

        [JsonProperty("bendtime")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? BEndTime { get; set; }

        [JsonProperty("sstarttime")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? SStartTime { get; set; }

        [JsonProperty("sendtime")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? SEndTime { get; set; }

        [JsonProperty("eventtype")]
        public int Type { get; set; }

        [JsonProperty("imgsize")]
        public int? ImageSize { get; set; }

        [JsonProperty("bvideosize")]
        public int? BVideoSize { get; set; }

        [JsonProperty("svideosize")]
        public int? SVideoSize { get; set; }
    }
}
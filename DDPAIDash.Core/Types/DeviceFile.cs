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
using System.IO;
using Newtonsoft.Json;
using DDPAIDash.Core.Json.Converters;
using Windows.UI.Xaml.Media.Imaging;

namespace DDPAIDash.Core.Types
{
    public class DeviceFile
    {
        private BitmapImage _image;
        private Stream _stream;

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("starttime")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? StartTime { get; set; }

        [JsonProperty("endtime")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime? EndTime { get; set; }

        [JsonProperty("event")]
        public int Event { get; set; }

        [JsonProperty("matchval")]
        public int MatchValue { get; set; }

        [JsonProperty("parentfile")]
        public string ParentFile { get; set; }

        [JsonIgnore]
        public Stream Stream
        {
            get { return _stream; }
            set
            {
                _stream = value;
                _image = new BitmapImage();
                _image.SetSource(_stream.AsRandomAccessStream());
            }
        }

        [JsonIgnore]
        public BitmapImage Image
        {
            get
            {
                return _image;
            }
        }
    }
}
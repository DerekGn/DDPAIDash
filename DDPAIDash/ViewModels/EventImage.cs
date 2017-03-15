

using System;
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
using DDPAIDash.Core.Types;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Globalization;

namespace DDPAIDash.ViewModels
{
    public class EventImage : EventBase
    {
        private DeviceEvent _event;
        private BitmapImage _image;

        public EventImage(DeviceEvent @event)
        {
            _event = @event;
        }

        public override ImageSource Image
        {
            get
            {
                if (_image == null)
                {
                    _image = CreateBitmapFromStream(_event.ImageThumbnailStream);
                }

                return _image;
            }
        }

        public override string Name
        {
            get
            {
                return FormatEventName(_event.ImageName).ToString(CultureInfo.CurrentUICulture);
            }
        }

        public override string SourceName
        {
            get
            {
                return _event.ImageName;
            }
        }
    }
}
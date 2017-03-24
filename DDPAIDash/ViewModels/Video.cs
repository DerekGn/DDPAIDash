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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using DDPAIDash.Core.Types;

namespace DDPAIDash.ViewModels
{
    internal class Video : DeviceContent
    {
        private readonly DeviceVideo _video;
        private BitmapImage _image;

        public Video(DeviceVideo video)
        {
            _video = video;
        }

        public override string Name => ParseVideoName(_video.Name).ToString(CultureInfo.CurrentUICulture);

        public override ImageSource Image => _image ?? (_image = CreateBitmapFromStream(_video.ImageThumbnailStream));

        public override string SourceName => _video.Name;

        private static DateTime ParseVideoName(string name)
        {
            var result = name.Substring(0, 14);

            return DateTime.ParseExact(result, "yyyyMMddHHmmss", CultureInfo.CurrentUICulture);
        }
    }
}
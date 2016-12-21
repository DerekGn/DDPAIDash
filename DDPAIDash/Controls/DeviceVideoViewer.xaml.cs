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

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DDPAIDash.Controls
{
    public sealed partial class DeviceVideoViewer
    {
        ImageSource _source;
        string _text;

        public DeviceVideoViewer()
        {
            this.InitializeComponent();
        }

        public void ShowPlaceholder(ImageSource source, string text)
        {
            _source = source;
            _text = text;

            NameTextBlock.Opacity = 0;
            Image.Opacity = 0;
        }
        public void ShowName()
        {
            NameTextBlock.Text = _text;
            NameTextBlock.Opacity = 1;
        }
        public void ShowImage()
        {
            Image.Source = _source;
            Image.Opacity = 1;
        }
        public void ClearData()
        {
            _source = null;
            _text = string.Empty;

            NameTextBlock.ClearValue(TextBlock.TextProperty);
            Image.ClearValue(Image.SourceProperty);
        }
    }
}

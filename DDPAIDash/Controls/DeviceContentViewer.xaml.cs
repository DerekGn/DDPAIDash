
using DDPAIDash.ViewModels;
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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DDPAIDash.Controls
{
    public sealed partial class DeviceVideoViewer
    {
        public DeviceVideoViewer()
        {
            this.InitializeComponent();
        }

        public event EventHandler<DeviceContent> DeviceContentTapped;

        public event EventHandler<DeviceContent> DeviceContentHolding;

        private void ImageList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RaiseEvent(DeviceContentTapped, (ListView) sender);
        }

        private void ImageList_Holding(object sender, HoldingRoutedEventArgs e)
        {
            RaiseEvent(DeviceContentHolding, (ListView) sender);
        }

        private void RaiseEvent(EventHandler<DeviceContent> eventToRaise, ListView listView)
        {
            var temp = eventToRaise;
            if (temp != null)
            {
                eventToRaise(this, (DeviceContent) listView.SelectedItem);
            }
        }
    }
}

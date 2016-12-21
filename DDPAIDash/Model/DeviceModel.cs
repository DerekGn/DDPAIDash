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
using System.Collections.ObjectModel;
using DDPAIDash.Core;
using DDPAIDash.Core.Events;
using DDPAIDash.Core.Types;

namespace DDPAIDash.Model
{
    public class DeviceModel
    {
        private static readonly Lazy<DeviceModel> DeviceModelInstance = new Lazy<DeviceModel>();

        public DeviceModel()
        {
            EventImages = new ObservableCollection<EventImage>();
            EventVideos = new ObservableCollection<EventVideo>();
            Videos = new ObservableCollection<Video>();
            DeviceInstance = new Device();

            DeviceInstance.VideoDeleted += DeviceInstanceVideoDeleted;
            DeviceInstance.StateChanged += DeviceInstanceStateChanged;
            DeviceInstance.EventDeleted += DeviceInstanceEventDeleted;
            DeviceInstance.EventAdded += DeviceInstanceEventAdded;
            DeviceInstance.VideoAdded += DeviceInstanceVideoAdded;
        }

        public static DeviceModel Instance => DeviceModelInstance.Value;

        public IDevice DeviceInstance { get; }

        public ObservableCollection<Video> Videos { get; }

        public ObservableCollection<EventImage> EventImages { get; }

        public ObservableCollection<EventVideo> EventVideos { get; }

        private void DeviceInstanceVideoAdded(object sender, VideoAddedEventArgs e)
        {
            Videos.Add(new Video(e.Video));
        }

        private void DeviceInstanceEventAdded(object sender, EventAddedEventArgs e)
        {
            AddDeviceEvent(e.Event);
        }

        private void DeviceInstanceStateChanged(object sender, StateChangedEventArgs e)
        {
        }

        private void DeviceInstanceVideosChanged(object sender, VideoAddedEventArgs e)
        {
        }

        private void DeviceInstanceEventDeleted(object sender, EventDeletedEventArgs e)
        {
            AddDeviceEvent(e.Event);
        }

        private void DeviceInstanceVideoDeleted(object sender, VideoDeletedEventArgs e)
        {
        }

        private void AddDeviceEvent(DeviceEvent deviceEvent)
        {
            if (!string.IsNullOrWhiteSpace(deviceEvent.BVideoName))
            {
                EventVideos.Add(new EventVideo(deviceEvent));
            }

            if (!string.IsNullOrWhiteSpace(deviceEvent.ImageName))
            {
                EventImages.Add(new EventImage(deviceEvent));
            }
        }
    }
}
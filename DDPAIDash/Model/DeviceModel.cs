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
using DDPAIDash.Core.Types;


namespace DDPAIDash.Model
{
    public class DeviceModel
    {
        private static Lazy<DeviceModel> _instance = new Lazy<DeviceModel>();
        
        public DeviceModel()
        {
            DeviceEvents = new ObservableCollection<DeviceEvent>();
            DeviceFiles = new ObservableCollection<DeviceFile>();
            DeviceInstance = new Device();
        }

        public static DeviceModel Instance { get { return _instance.Value; } }

        public IDevice DeviceInstance { get; }

        public ObservableCollection<DeviceFile> DeviceFiles { get; private set; }

        public ObservableCollection<DeviceEvent> DeviceEvents { get; private set; }

        public bool IsDeviceConnected => DeviceInstance.State == DeviceState.Connected;

        public void LoadFilesAndEvents()
        {
            foreach (var deviceFile in DeviceInstance.GetFiles().Files)
            {
                DeviceFiles.Add(deviceFile);
            }

            foreach (var deviceEvent in DeviceInstance.GetEvents().Events)
            {
                DeviceEvents.Add(deviceEvent);
            }
        }
    }
}

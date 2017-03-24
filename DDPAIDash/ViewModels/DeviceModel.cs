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
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using DDPAIDash.Core;
using DDPAIDash.Core.Events;
using DDPAIDash.Core.Types;
using System.Threading.Tasks;
using System.IO;

namespace DDPAIDash.ViewModels
{
    internal class DeviceModel : INotifyPropertyChanged
    {
        private static readonly Lazy<DeviceModel> DeviceModelInstance = new Lazy<DeviceModel>();
        private readonly CoreDispatcher _dispatcher;
        private readonly IDevice _deviceInstance;
        private int _syncCount;
        private int _syncRemain;

        public DeviceModel()
        {
            _deviceInstance = new Device();
            Videos = new ObservableCollection<Video>();
            EventImages = new ObservableCollection<EventImage>();
            EventVideos = new ObservableCollection<EventVideo>();

            _deviceInstance.VideoDeleted += DeviceInstanceVideoDeleted;
            _deviceInstance.StateChanged += DeviceInstanceStateChanged;
            _deviceInstance.EventDeleted += DeviceInstanceEventDeleted;
            _deviceInstance.EventAdded += DeviceInstanceEventAdded;
            _deviceInstance.VideoAdded += DeviceInstanceVideoAdded;
            _deviceInstance.SyncProgress += DeviceInstanceSyncProgress;

            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            }
        }
        
        public static DeviceModel Instance => DeviceModelInstance.Value;
                
        public int SyncCount
        {
            get
            {
                return _syncCount;
            }
            private set
            {
                _syncCount = value;
                OnPropertyChanged(nameof(SyncCount));
            }
        }

        public int SyncRemain
        {
            get
            {
                return _syncRemain;
            }
            private set
            {
                _syncRemain = value;
                OnPropertyChanged(nameof(SyncRemain));
            }
        }

        public DeviceState DeviceState => _deviceInstance.State;

        public ObservableCollection<Video> Videos { get; }

        public ObservableCollection<EventImage> EventImages { get; }

        public ObservableCollection<EventVideo> EventVideos { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<bool> ConnectAsync(UserInfo userInfo)
        {
            return await _deviceInstance.ConnectAsync(userInfo);
        }

        public async Task<Stream> StreamFileAsync(string sourceName)
        {
            return await _deviceInstance.StreamFileAsync(sourceName);
        }

        public async Task<bool> FormatDeviceAsync()
        {
            EventImages.Clear();
            EventVideos.Clear();
            Videos.Clear();

            return await _deviceInstance.FormatAsync();
        }

        private void DeviceInstanceVideoAdded(object sender, VideoAddedEventArgs e)
        {
            ExecuteOnDispatcher(() =>
            {
                Videos.Insert(0, new Video(e.Video));
            });
        }

        private void DeviceInstanceVideoDeleted(object sender, VideoDeletedEventArgs e)
        {
            var video = Videos.FirstOrDefault(v => v.Name == e.Name);

            ExecuteOnDispatcher(() =>
            {
                if (video != null)
                {
                    Videos.Remove(video);
                }
            });
        }

        private void DeviceInstanceEventAdded(object sender, EventAddedEventArgs e)
        {
            AddDeviceEvent(e.Event);
        }

        private void DeviceInstanceStateChanged(object sender, StateChangedEventArgs e)
        {
            OnPropertyChanged(nameof(DeviceState));
        }

        private void DeviceInstanceEventDeleted(object sender, EventDeletedEventArgs e)
        {
            AddDeviceEvent(e.Event);
        }
        
        private void DeviceInstanceSyncProgress(object sender, SyncProgressEventArgs e)
        {
            SyncCount = e.Total;
            SyncRemain = e.Remaining;
        }

        private void AddDeviceEvent(DeviceEvent deviceEvent)
        {
            ExecuteOnDispatcher(() =>
            {
                if (!string.IsNullOrWhiteSpace(deviceEvent.BVideoName))
                {
                    EventVideos.Insert(0, new EventVideo(deviceEvent));
                }

                if (!string.IsNullOrWhiteSpace(deviceEvent.ImageName))
                {
                    EventImages.Insert(0, new EventImage(deviceEvent));
                }
            });
        }

        private async void ExecuteOnDispatcher(Action action)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 action();
             });
        }

        private void OnPropertyChanged(string propertyName)
        {
            ExecuteOnDispatcher(() => 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName))
            );
        }
    }
}
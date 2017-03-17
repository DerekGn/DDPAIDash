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

namespace DDPAIDash.ViewModels
{
    public class DeviceModel : INotifyPropertyChanged
    {
        private static readonly Lazy<DeviceModel> DeviceModelInstance = new Lazy<DeviceModel>();
        private readonly CoreDispatcher Dispatcher;
        private bool _settingsEnabled;
        private bool _connectEnabled;
        private bool _formatEnabled;
        private bool _paringEnabled;
        private bool _liveEnabled;

        public DeviceModel()
        {
            DeviceInstance = new Device();
            Videos = new ObservableCollection<Video>();
            EventImages = new ObservableCollection<EventImage>();
            EventVideos = new ObservableCollection<EventVideo>();

            DeviceInstance.VideoDeleted += DeviceInstanceVideoDeleted;
            DeviceInstance.StateChanged += DeviceInstanceStateChanged;
            DeviceInstance.EventDeleted += DeviceInstanceEventDeleted;
            DeviceInstance.EventAdded += DeviceInstanceEventAdded;
            DeviceInstance.VideoAdded += DeviceInstanceVideoAdded;

            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            }

            ConnectEnabled = true;
        }

        public async Task<bool> FormatDeviceAsync()
        {
            EventImages.Clear();
            EventVideos.Clear();
            Videos.Clear();

            return await DeviceInstance.FormatAsync();
        }

        public static DeviceModel Instance => DeviceModelInstance.Value;

        public IDevice DeviceInstance { get; private set; }

        public bool ConnectEnabled
        {
            get
            {
                return _connectEnabled;
            }
            private set
            {
                _connectEnabled = value;
                OnPropertyChanged(nameof(ConnectEnabled));
            }
        }

        public bool FormatEnabled
        {
            get
            {
                return _formatEnabled;
            }
            private set
            {
                _formatEnabled = value;
                OnPropertyChanged(nameof(FormatEnabled));
            }
        }

        public bool SettingsEnabled
        {
            get
            {
                return _settingsEnabled;
            }
            private set
            {
                _settingsEnabled = value;
                OnPropertyChanged(nameof(SettingsEnabled));
            }
        }

        public bool PairingEnabled {
            get
            {
                return _paringEnabled;
            }
            private set
            {
                _paringEnabled = value;
                OnPropertyChanged(nameof(PairingEnabled));
            }
        }

        public bool LiveEnabled
        {
            get
            {
                return _liveEnabled;
            }
            private set
            {
                _liveEnabled = value;
                OnPropertyChanged(nameof(LiveEnabled));
            }
        }

        public ObservableCollection<Video> Videos { get; }

        public ObservableCollection<EventImage> EventImages { get; }

        public ObservableCollection<EventVideo> EventVideos { get; }

        public event PropertyChangedEventHandler PropertyChanged;

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
            if (e.State == DeviceState.Connected)
            {
                LiveEnabled = FormatEnabled = SettingsEnabled = PairingEnabled = true;
                ConnectEnabled = false;
            }

            if(e.State == DeviceState.Formatting)
            {
                SettingsEnabled = PairingEnabled = false;
                FormatEnabled = false;
            }

            if(e.State == DeviceState.PoweredDown)
            {
                //TODO: Handle power down
                LiveEnabled = FormatEnabled = SettingsEnabled = PairingEnabled = false;
                ConnectEnabled = true;
            }
        }

        private void DeviceInstanceEventDeleted(object sender, EventDeletedEventArgs e)
        {
            AddDeviceEvent(e.Event);
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
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
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
using System;

namespace DDPAIDash.Core
{
    internal class DeviceStateChangedEventArgs : EventArgs
    {
        public DeviceState DeviceState { get; set; }
    }
}
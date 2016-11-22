using System;

namespace DDPAIDash.Core
{
    internal interface IDevice
    {
        DeviceInfo Info { get; set; }

        GSensorMode Level { get; set; }

        int CycleRecordSpace { get; }

        int SpeakerLevel { get; set; }

        int AntiFog { get; set; }

        bool IsNeedUpdate { get; set; }

        int EventAfterTime { get; set; }

        int EventBeforeTime { get; set; }

        int DisplayMode { get; set; }

        SwitchState WdrSwitch { get; set; }

        string DefaultUser { get; }

        SwitchState LdcSwitch { get; set; }

        SwitchState MicSwitch { get; set; }

        ImageQuality Quality { get; set; }

        SwitchState OsdSwitch { get; set; }

        SwitchState OsdSpeedSwitch { get; set; }

        SwitchState StartSoundSwitch { get; set; }

        SwitchState ParkingModeSwitch { get; set; }

        SwitchState TimeLapseSwitch { get; set; }

        string SessionId { get; set; }

        UserInfo User { get; set; }

        StorageInfo Storage { get; }

        DeviceState DeviceState { get; }

        void Connect(UserInfo userInfo, TimeZoneSettings timeZoneSettings);

        void Disconnect();
        
        event EventHandler<DeviceStateChangedEventArgs> DeviceStateChanged;
    }
}

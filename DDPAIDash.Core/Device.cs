using System;
using DDPAIDash.Core.Constants;
using DDPAIDash.Core.Transports;
using Newtonsoft.Json;
using Windows.Networking;

namespace DDPAIDash.Core
{
    internal class Device : IDevice
    {
        //{"keys":[{"key":"wdr_enable"},{"key":"gsensor_mode"},{"key":"cycle_record_space"},{"key":"speaker_turn"},{"key":"default_user"},{"key":"ldc_switch"},{"key":"anti_fog"},{"key":"is_need_update"},{"key":"event_after_time"},{"key":"event_before_time"},{"key":"mic_switch"},{"key":"image_quality"},{"key":"display_mode"},{"key":"osd_switch"},{"key":"osd_speedswitch"},{"key":"start_sound_switch"},{"key":"delay_poweroff_time"},{"key":"edog_switch"},{"key":"parking_mode_switch"},{"key":"timelapse_rec_switch"}]}

        //{"errcode":0,"data":"{\"int_params\":[{\"key\":\"gsensor_mode\",\"value\":2},
        //{\"key\":\"cycle_record_space\",\"value\":1048576},
        //{\"key\":\"speaker_turn\",\"value\":34},
        //{\"key\":\"anti_fog\",\"value\":0},
        //{\"key\":\"is_need_update\",\"value\":0},
        //{\"key\":\"event_after_time\",\"value\":0},
        //{\"key\":\"event_before_time\",\"value\":0},
        //{\"key\":\"display_mode\",\"value\":0}],
        //\"string_params\":[{\"key\":\"wdr_enable\",\"value\":\"off\"},
        //{\"key\":\"default_user\",\"value\":\"012345678912345\"},{\"key\":\"ldc_switch\",\"value\":\"on\"},
        //{\"key\":\"mic_switch\",\"value\":\"off\"},{\"key\":\"image_quality\",\"value\":\"low\"},
        //{\"key\":\"osd_switch\",\"value\":\"on\"},{\"key\":\"osd_speedswitch\",\"value\":\"na\"},
        //{\"key\":\"start_sound_switch\",\"value\":\"on\"},{\"key\":\"parking_mode_switch\",\"value\":\"on\"},
        //{\"key\":\"timelapse_rec_switch\",\"value\":\"on\"}]}"}

        private ITransport _transport;

        public Device() : this(new HttpTransport())
        { }

        public Device(ITransport transport)
        {
            _transport = transport;
        }

        public int AntiFog
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int CycleRecordSpace
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DefaultUser
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DeviceState DeviceState
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int DisplayMode
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int EventAfterTime
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int EventBeforeTime
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public DeviceInfo Info
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsNeedUpdate
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public SwitchState LdcSwitch
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public GSensorMode Level
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public SwitchState MicSwitch
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public SwitchState OsdSpeedSwitch
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public SwitchState OsdSwitch
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public SwitchState ParkingModeSwitch
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ImageQuality Quality
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string SessionId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int SpeakerLevel
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public SwitchState StartSoundSwitch
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public StorageInfo Storage
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public SwitchState TimeLapseSwitch
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public UserInfo User
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public SwitchState WdrSwitch
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<DeviceStateChangedEventArgs> DeviceStateChanged;

        public void Connect(UserInfo userInfo, TimeZoneSettings timeZoneSettings)
        {
            try
            {
                _transport.Connect(new HostName("192.168.0.1"), 80);

                ResponseMessage response = _transport.Execute(ApiConstants.RequestSession);

                //_transport.SessionId = session;

                //response = _transport.Execute(ApiConstants.RequestCertificate, JsonConvert.SerializeObject(userInfo));

                //response = _transport.Execute(ApiConstants.SyncDate, JsonConvert.SerializeObject(timeZoneSettings));

                //response = _transport.Execute(ApiConstants.GetBaseInfo);

                //response = _transport.Execute(ApiConstants.AvCapReq);

                //response = _transport.Execute(ApiConstants.AvCapSet, JsonConvert.SerializeObject(new StreamSettings(30, 0)));

                //StartMailBoxTask();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Disconnect()
        {
            try
            {
#warning logout

                _transport.Disconnect();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
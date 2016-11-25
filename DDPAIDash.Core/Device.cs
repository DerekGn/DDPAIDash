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
using System.Collections.Generic;
using DDPAIDash.Core.Constants;
using DDPAIDash.Core.Logging;
using DDPAIDash.Core.Transports;
using DDPAIDash.Core.Types;
using Newtonsoft.Json;

namespace DDPAIDash.Core
{
    internal class Device : IDevice
    {
        private readonly ILogger _logger;

        private readonly ITransport _transport;
        private int _antiFog;
        private GSensorMode _gsmode;
        private SwitchState _ldc;
        private SwitchState _mic;
        private SwitchState _osd;
        private SwitchState _osdSpeed;

        private SwitchState _parkingMode;
        private ImageQuality _quality;
        private int _speakerLevel;
        private SwitchState _startSound;
        private SwitchState _timeLapse;
        private SwitchState _wdr;

        public Device() : this(new HttpTransport(), new Logger())
        {
        }

        public Device(ITransport transport, ILogger logger)
        {
            _transport = transport;
            _logger = logger;
        }

        public DeviceInfo Info { get; private set; }

        public UserInfo User { get; private set; }

        public StorageInfo Storage
        {
            get { throw new NotImplementedException(); }
        }

        public DeviceState DeviceState
        {
            get;
            private set;
        }

        public DeviceCapabilities Capabilities { get; private set; }

        public string SessionId { get; private set; }

        public GSensorMode GsMode
        {
            get { return _gsmode; }
            set
            {
                _gsmode = value;
                SetStringValue("gsensor_mode", value.ToString());
            }
        }

        public int CycleRecordSpace
        {
            get { return _speakerLevel; }
            set
            {
                _speakerLevel = value;
                SetIntValue("cycle_record_space", value);
            }
        }

        public int SpeakerLevel
        {
            get { return _speakerLevel; }
            set
            {
                _speakerLevel = value;
                SetIntValue("speaker_turn", value);
            }
        }

        public int AntiFog
        {
            get { return _antiFog; }
            set
            {
                _antiFog = value;
                SetIntValue("anti_fog", value);
            }
        }

        public int EventAfterTime
        {
            get { return _antiFog; }
            set
            {
                _antiFog = value;
                SetIntValue("event_after_time", value);
            }
        }

        public int EventBeforeTime
        {
            get { return _antiFog; }
            set
            {
                _antiFog = value;
                SetIntValue("event_before_time", value);
            }
        }

        public int DisplayMode
        {
            get { return _antiFog; }
            set
            {
                _antiFog = value;
                SetIntValue("display_mode", value);
            }
        }

        public SwitchState Wdr
        {
            get { return _wdr; }
            set
            {
                _wdr = value;
                SetStringValue("wdr_enable", value.ToString());
            }
        }

        public SwitchState Ldc
        {
            get { return _ldc; }
            set
            {
                _ldc = value;
                SetStringValue("ldc_switch", value.ToString());
            }
        }

        public SwitchState Mic
        {
            get { return _mic; }
            set
            {
                _mic = value;
                SetStringValue("mic_switch", value.ToString());
            }
        }

        public ImageQuality Quality
        {
            get { return _quality; }
            set
            {
                _quality = value;
                SetStringValue("mic_switch", value.ToString());
            }
        }

        public SwitchState Osd
        {
            get { return _osd; }
            set
            {
                _osd = value;
                SetStringValue("osd_switch", value.ToString());
            }
        }

        public SwitchState OsdSpeed
        {
            get { return _osdSpeed; }
            set
            {
                _osdSpeed = value;
                SetStringValue("osd_speedswitch", value.ToString());
            }
        }

        public SwitchState StartSound
        {
            get { return _startSound; }
            set
            {
                _startSound = value;
                SetStringValue("start_sound_switch", value.ToString());
            }
        }

        public SwitchState ParkingMode
        {
            get { return _parkingMode; }
            set
            {
                _parkingMode = value;
                SetStringValue("parking_mode_switch", value.ToString());
            }
        }

        public SwitchState TimeLapse
        {
            get { return _timeLapse; }
            set
            {
                _timeLapse = value;
                SetStringValue("timelapse_rec_switch", value.ToString());
            }
        }
        
        public event EventHandler<DeviceStateChangedEventArgs> DeviceStateChanged;

        public void Connect(UserInfo userInfo)
        {
            _transport.Connect("193.168.0.1", 80);

            var connectActions = new List<Func<bool>>
            {
                () =>
                {
                    return ExecuteRequest(ApiConstants.RequestSession, apiCommand => _transport.Execute(apiCommand),
                        response =>
                        {
                            SessionId =
                                _transport.SessionId =
                                    JsonConvert.DeserializeAnonymousType(response.Data, new {acsessionid = string.Empty})
                                        .acsessionid;
                        });
                },
                () =>
                {
                    return ExecuteRequest(ApiConstants.RequestCertificate,
                        apiCommand => _transport.Execute(apiCommand, JsonConvert.SerializeObject(userInfo)), null);
                },
                () =>
                {
                    return ExecuteRequest(ApiConstants.SyncDate,
                        apiCommand =>
                            _transport.Execute(apiCommand, JsonConvert.SerializeObject(TimeZoneSettings.Instance)), null);
                },
                () =>
                {
                    return ExecuteRequest(ApiConstants.GetBaseInfo, apiCommand => _transport.Execute(apiCommand),
                        response => { Info = JsonConvert.DeserializeObject<DeviceInfo>(response.Data); });
                },
                () =>
                {
                    return ExecuteRequest(ApiConstants.AvCapReq, apiCommand => _transport.Execute(apiCommand),
                        response => { Capabilities = JsonConvert.DeserializeObject<DeviceCapabilities>(response.Data); });
                },
                () =>
                {
                    return ExecuteRequest(ApiConstants.AvCapSet,
                        apiCommand =>
                            _transport.Execute(apiCommand, JsonConvert.SerializeObject(new StreamSettings(30, 0))), null);
                }
            };

            foreach (var action in connectActions)
            {
                if (!action())
                    break;
            }

            User = userInfo;

            //StartMailBoxTask();
        }

        public void Disconnect()
        {
            try
            {
#warning logout

                _transport.Disconnect();
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        private bool ExecuteRequest(string apiCommand, Func<string, ResponseMessage> transportAction,
            Action<ResponseMessage> responseAction)
        {
            var result = true;

            try
            {
                var response = transportAction(apiCommand);

                if (response.ErrorCode == 0)
                {
                    _logger.Error($"[{apiCommand}] Execution Succeded. Data: [{response.Data}]");
                    responseAction?.Invoke(response);
                }
                else
                {
                    _logger.Error($"[{apiCommand}] Execution Failed. Data: [{response.Data}]");
                    result = false;
                }
            }
            catch (Exception exception)
            {
                _logger.Fatal($"[{apiCommand}] Execution Failed", exception);
                result = false;
            }

            return result;
        }

        private void SetStringValue(string key, string state)
        {
            var parameter = new Parameters {StringParameters = {new StringParameter {Key = key, Value = state}}};
            ExecuteRequest(ApiConstants.GeneralSave,
                apiCommand => _transport.Execute(apiCommand, JsonConvert.SerializeObject(parameter)), null);
        }

        private void SetIntValue(string key, int state)
        {
            var parameter = new Parameters {IntParameters = {new IntParameter {Key = key, Value = state}}};
            ExecuteRequest(ApiConstants.GeneralSave,
                apiCommand => _transport.Execute(apiCommand, JsonConvert.SerializeObject(parameter)), null);
        }

        #region IDisposable Support

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Device() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
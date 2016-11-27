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
using System.Threading.Tasks;
using System.Threading;

namespace DDPAIDash.Core
{
    internal class Device : IDevice
    {
        private static CancellationTokenSource cts = new CancellationTokenSource();

        private readonly ITransport _transport;
        private readonly ILogger _logger;
        
        private int? _antiFog;
        private int? _cycleRecordSpace;
        private string _defaultUser;
        private int? _delayPoweroffTime;
        private int? _displayMode;
        private int? _eventAfterTime;
        private int? _eventBeforeTime;
        private SwitchState? _edogSwitch;
        private bool? _isNeedUpdate;
        private GSensorMode? _gsmode;
        private SwitchState? _ldc;
        private SwitchState? _mic;
        private SwitchState? _osd;
        private SwitchState? _osdSpeed;
        private SwitchState? _parkingMode;
        private ImageQuality? _quality;
        private int? _speakerLevel;
        private SwitchState? _startSound;
        private SwitchState? _timeLapse;
        private SwitchState? _wdr;

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

        public StorageInfo Storage { get; private set; }

        public DeviceState DeviceState { get; private set; }

        public DeviceCapabilities Capabilities { get; private set; }

        public string SessionId { get; private set; }

        public string DefaultUser
        {
            get { return _defaultUser; }
            set
            {
                _defaultUser = value;
                SetStringValue("default_user", value.ToString());
            }
        }
        
        public GSensorMode? GsMode
        {
            get { return _gsmode; }
            set
            {
                _gsmode = value;
                if (_gsmode.HasValue)
                    SetStringValue("gsensor_mode", value.Value.ToString());
            }
        }

        public int? CycleRecordSpace
        {
            get { return _cycleRecordSpace; }
            set
            {
                _cycleRecordSpace = value;
                if (_cycleRecordSpace.HasValue)
                    SetIntValue("cycle_record_space", value.Value);
            }
        }

        public int? SpeakerLevel
        {
            get { return _speakerLevel; }
            set
            {
                _speakerLevel = value;
                if(_speakerLevel.HasValue)
                    SetIntValue("speaker_turn", value.Value);
            }
        }

        public int? AntiFog
        {
            get { return _antiFog; }
            set
            {
                _antiFog = value;
                if (_antiFog.HasValue)
                    SetIntValue("anti_fog", value.Value);
            }
        }

        public int? EventAfterTime
        {
            get { return _eventAfterTime; }
            set
            {
                _eventAfterTime = value;
                if (_eventAfterTime.HasValue)
                    SetIntValue("event_after_time", value.Value);
            }
        }

        public int? EventBeforeTime
        {
            get { return _eventBeforeTime; }
            set
            {
                _eventBeforeTime = value;
                if (_eventBeforeTime.HasValue)
                    SetIntValue("event_before_time", value.Value);
            }
        }

        public int? DisplayMode
        {
            get { return _displayMode; }
            set
            {
                _displayMode = value;
                if (_displayMode.HasValue)
                    SetIntValue("display_mode", value.Value);
            }
        }

        public SwitchState? EDogSwitch
        {
            get { return _edogSwitch; }
            set
            {
                _edogSwitch = value;
                if (_edogSwitch.HasValue)
                    SetStringValue("edog_switch", value.Value.ToString());
            }
        }

        public SwitchState? Wdr
        {
            get { return _wdr; }
            set
            {
                _wdr = value;
                if (_wdr.HasValue)
                    SetStringValue("wdr_enable", value.Value.ToString());
            }
        }

        public SwitchState? Ldc
        {
            get { return _ldc; }
            set
            {
                _ldc = value;
                if (_ldc.HasValue)
                    SetStringValue("ldc_switch", value.Value.ToString());
            }
        }

        public SwitchState? Mic
        {
            get { return _mic; }
            set
            {
                _mic = value;
                if (_mic.HasValue)
                    SetStringValue("mic_switch", value.Value.ToString());
            }
        }

        public ImageQuality? Quality
        {
            get { return _quality; }
            set
            {
                _quality = value;
                if (_quality.HasValue)
                    SetStringValue("image_quality", value.Value.ToString());
            }
        }

        public SwitchState? Osd
        {
            get { return _osd; }
            set
            {
                _osd = value;
                if (_osd.HasValue)
                    SetStringValue("osd_switch", value.Value.ToString());
            }
        }

        public SwitchState? OsdSpeed
        {
            get { return _osdSpeed; }
            set
            {
                _osdSpeed = value;
                if (_osdSpeed.HasValue)
                    SetStringValue("osd_speedswitch", value.Value.ToString());
            }
        }

        public SwitchState? StartSound
        {
            get { return _startSound; }
            set
            {
                _startSound = value;
                if (_startSound.HasValue)
                    SetStringValue("start_sound_switch", value.Value.ToString());
            }
        }

        public SwitchState? ParkingMode
        {
            get { return _parkingMode; }
            set
            {
                _parkingMode = value;
                if (_parkingMode.HasValue)
                    SetStringValue("parking_mode_switch", value.Value.ToString());
            }
        }

        public SwitchState? TimeLapse
        {
            get { return _timeLapse; }
            set
            {
                _timeLapse = value;
                if (_timeLapse.HasValue)
                    SetStringValue("timelapse_rec_switch", value.Value.ToString());
            }
        }

        public int? DelayPoweroffTime
        {
            get { return _delayPoweroffTime; }
            set
            {
                _delayPoweroffTime = value;
                if (_displayMode.HasValue)
                    SetIntValue("delay_poweroff_time", value.Value);
            }
        }
        
        public bool? IsNeedUpdate
        {
            get { return _isNeedUpdate; }
            set
            {
                _isNeedUpdate = value;
                SetStringValue("is_need_update", value.ToString());
            }
        }

        public event EventHandler<DeviceStateChangedEventArgs> DeviceStateChanged;

        public bool Connect(UserInfo userInfo)
        {
            bool result = false;

            _transport.Connect("193.168.0.1", 80);

            result = PerformConnect(userInfo);

            if(result)
            {
                User = userInfo;
 
                _mailboxTask = Task.Factory.StartNew(() => PollMailbox(cts.Token), cts.Token);
            }

            return result;
        }

        public void Disconnect()
        {
            try
            {
#warning logout
                ExecuteRequest(ApiConstants.GetMailboxData,
                    apiCommand => _transport.Execute(apiCommand), (response) => {
                        _logger.Error(response.Data);
                    });

                _transport.Disconnect();
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        private void PollMailbox(CancellationToken cancellationToken)
        {
            do
            {
                Task.Delay(5000).Wait();

                // OK
                ExecuteRequest(ApiConstants.GetMailboxData,
                    apiCommand => _transport.Execute(apiCommand), (response) => {
                        _logger.Error(response.Data);
                    });

                //ExecuteRequest("API_GetTestdate",
                //    apiCommand => _transport.Execute(apiCommand), (response) => {
                //        _logger.Error(response.Data);
                //    });

                ////OK
                //ExecuteRequest(ApiConstants.PlayModeQuery,
                //        apiCommand => _transport.Execute(apiCommand), (response) => {
                //            _logger.Error(response.Data);
                //        });
                ////ok
                //ExecuteRequest(ApiConstants.PlaybackListReq,
                //        apiCommand => _transport.Execute(apiCommand), (response) => {
                //            _logger.Error(response.Data);
                //        });

                //ExecuteRequest("API_GetModuleState",
                //        apiCommand => _transport.Execute(apiCommand), (response) =>
                //        {
                //            _logger.Error(response.Data);
                //        });
                
            } while (!cancellationToken.IsCancellationRequested);
        }

        private bool PerformConnect(UserInfo userInfo)
        {
            bool result = false;

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
                },
                () =>
                {
                    return ExecuteRequest(ApiConstants.GeneralQuery,
                        apiCommand =>
                            _transport.Execute(apiCommand, JsonConvert.SerializeObject(QueryParameters.Instance)), response => { LoadSettings(response); });
                },
                () =>
                {
                    return ExecuteRequest(ApiConstants.GetStorageInfo,
                        apiCommand => _transport.Execute(apiCommand), response => {  Storage =  JsonConvert.DeserializeObject<StorageInfo>(response.Data); });
                }
            };

            foreach (var action in connectActions)
            {
                result = action();

                if (!result)
                    break;
            }


            return result;
        }
        
        private void LoadSettings(ResponseMessage response)
        {
            Parameters parameters = JsonConvert.DeserializeObject<Parameters>(response.Data);

            foreach (var intParameter in parameters.IntParameters)
            {
                LoadSetting(intParameter);
            }

            foreach (var stringParameter in parameters.StringParameters)
            {
                LoadSetting(stringParameter);
            }
        }

        private void LoadSetting<T>(Parameter<T> parameter)
        {
            switch ((QueryParameterKeys)Enum.Parse(typeof(QueryParameterKeys), parameter.Key))
            {
                case QueryParameterKeys.wdr_enable:
                    _wdr = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case QueryParameterKeys.gsensor_mode:
                    _gsmode = (GSensorMode)Enum.Parse(typeof(GSensorMode), parameter.Value.ToString(), true);
                    break;
                case QueryParameterKeys.cycle_record_space:
                    _cycleRecordSpace = int.Parse(parameter.Value.ToString());
                    break;
                case QueryParameterKeys.speaker_turn:
                    _speakerLevel = int.Parse(parameter.Value.ToString());
                    break;
                case QueryParameterKeys.default_user:
                    _defaultUser = parameter.Value.ToString();
                    break;
                case QueryParameterKeys.ldc_switch:
                    _ldc = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case QueryParameterKeys.anti_fog:
                    _antiFog = int.Parse(parameter.Value.ToString());
                    break;
                case QueryParameterKeys.is_need_update:
                    _isNeedUpdate = int.Parse(parameter.Value.ToString()) == 1;
                    break;
                case QueryParameterKeys.event_after_time:
                    _eventAfterTime = int.Parse(parameter.Value.ToString());
                    break;
                case QueryParameterKeys.event_before_time:
                    _eventBeforeTime = int.Parse(parameter.Value.ToString());
                    break;
                case QueryParameterKeys.mic_switch:
                    _mic = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case QueryParameterKeys.image_quality:
                    _quality = (ImageQuality)Enum.Parse(typeof(ImageQuality), parameter.Value.ToString(), true);
                    break;
                case QueryParameterKeys.display_mode:
                    _displayMode = int.Parse(parameter.Value.ToString());
                    break;
                case QueryParameterKeys.osd_switch:
                    _osd = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case QueryParameterKeys.osd_speedswitch:
                    _osdSpeed = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case QueryParameterKeys.start_sound_switch:
                    _startSound = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case QueryParameterKeys.delay_poweroff_time:
                    _delayPoweroffTime = int.Parse(parameter.Value.ToString());
                    break;
                case QueryParameterKeys.edog_switch:
                    _edogSwitch = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case QueryParameterKeys.parking_mode_switch:
                    _parkingMode = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case QueryParameterKeys.timelapse_rec_switch:
                    _timeLapse = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                default:
                    break;
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
            var parameter = new Parameters { StringParameters = { new Parameter<string> { Key = key, Value = state } } };
            ExecuteRequest(ApiConstants.GeneralSave,
                apiCommand => _transport.Execute(apiCommand, JsonConvert.SerializeObject(parameter)), null);
        }

        private void SetIntValue(string key, int state)
        {
            var parameter = new Parameters { IntParameters = { new Parameter<int> { Key = key, Value = state } } };
            ExecuteRequest(ApiConstants.GeneralSave,
                apiCommand => _transport.Execute(apiCommand, JsonConvert.SerializeObject(parameter)), null);
        }

        #region IDisposable Support

        private bool _disposedValue;
        private object _mailboxTask;

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
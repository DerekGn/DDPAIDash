﻿/**
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
using System.Threading;
using System.Threading.Tasks;
using DDPAIDash.Core.Constants;
using DDPAIDash.Core.Logging;
using DDPAIDash.Core.Transports;
using DDPAIDash.Core.Types;
using Newtonsoft.Json;

namespace DDPAIDash.Core
{
    public class Device : IDevice
    {
        private static readonly CancellationTokenSource Cts;

        private readonly ILogger _logger;

        private readonly ITransport _transport;
        private int? _antiFog;
        private int? _cycleRecordSpace;
        private string _defaultUser;
        private int? _delayPoweroffTime;
        private DisplayMode? _displayMode;
        private SwitchState? _edogSwitch;
        private int? _eventAfterTime;
        private int? _eventBeforeTime;
        private GSensorMode? _gsmode;
        private SwitchState? _hmirror;
        private bool? _isNeedUpdate;
        private SwitchState? _ldc;

        private Task _mailboxTask;
        private SwitchState? _mic;
        private SwitchState? _osd;
        private SwitchState? _osdSpeed;
        private SwitchState? _parkingMode;
        private ImageQuality? _quality;
        private int? _speakerLevel;
        private SwitchState? _startSound;
        private SwitchState? _timeLapse;
        private SwitchState? _wdr;

        static Device()
        {
            Cts = new CancellationTokenSource();
        }

        public Device() : this(new HttpTransport(), EtwLogger.Instance)
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

        public DeviceState State { get; private set; }

        public DeviceCapabilities Capabilities { get; private set; }

        public string SessionId { get; private set; }

        public string DefaultUser
        {
            get { return _defaultUser; }
            set
            {
                GuardPropertySet(nameof(DefaultUser), value,
                    () =>
                    {
                        _defaultUser = value;
                        SetStringValue(PropertyKeys.default_user.ToString(), value);
                    });
            }
        }

        public GSensorMode? GsMode
        {
            get { return _gsmode; }
            set
            {
                GuardPropertySet(nameof(GsMode), value,
                    () =>
                    {
                        _gsmode = value;
                        SetStringValue(PropertyKeys.gsensor_mode.ToString(), value.Value.ToString());
                    });
            }
        }

        public int? CycleRecordSpace
        {
            get { return _cycleRecordSpace; }
            set
            {
                GuardPropertySet(nameof(CycleRecordSpace), value,
                    () =>
                    {
                        _cycleRecordSpace = value;
                        SetIntValue(PropertyKeys.cycle_record_space.ToString(), value.Value);
                    });
            }
        }

        public int? SpeakerLevel
        {
            get { return _speakerLevel; }
            set
            {
                GuardPropertySet(nameof(SpeakerLevel), value,
                    () =>
                    {
                        _speakerLevel = value;
                        SetIntValue(PropertyKeys.speaker_turn.ToString(), value.Value);
                    });
            }
        }

        public int? AntiFog
        {
            get { return _antiFog; }
            set
            {
                GuardPropertySet(nameof(AntiFog), value,
                    () =>
                    {
                        _antiFog = value;
                        SetIntValue(PropertyKeys.anti_fog.ToString(), value.Value);
                    });
            }
        }

        public int? EventAfterTime
        {
            get { return _eventAfterTime; }
            set
            {
                GuardPropertySet(nameof(EventAfterTime), value,
                    () =>
                    {
                        _eventAfterTime = value;
                        SetIntValue(PropertyKeys.event_after_time.ToString(), value.Value);
                    });
            }
        }

        public int? EventBeforeTime
        {
            get { return _eventBeforeTime; }
            set
            {
                GuardPropertySet(nameof(EventBeforeTime), value,
                    () =>
                    {
                        _eventBeforeTime = value;
                        SetIntValue(PropertyKeys.event_before_time.ToString(), value.Value);
                    });
            }
        }

        public DisplayMode? DisplayMode
        {
            get { return _displayMode; }
            set
            {
                GuardPropertySet(nameof(DisplayMode), value,
                    () =>
                    {
                        _displayMode = value;
                        SetIntValue(PropertyKeys.display_mode.ToString(), (int) value.Value);
                    });
            }
        }

        public SwitchState? EDog
        {
            get { return _edogSwitch; }
            set
            {
                GuardPropertySet(nameof(EDog), value,
                    () =>
                    {
                        _edogSwitch = value;
                        SetStringValue(PropertyKeys.edog_switch.ToString(), value.Value.ToString());
                    });
            }
        }

        public SwitchState? Wdr
        {
            get { return _wdr; }
            set
            {
                GuardPropertySet(nameof(Wdr), value,
                    () =>
                    {
                        _wdr = value;
                        SetStringValue(PropertyKeys.wdr_enable.ToString(), value.Value.ToString());
                    });
            }
        }

        public SwitchState? Ldc
        {
            get { return _ldc; }
            set
            {
                GuardPropertySet(nameof(Ldc), value,
                    () =>
                    {
                        _ldc = value;
                        SetStringValue(PropertyKeys.ldc_switch.ToString(), value.Value.ToString());
                    });
            }
        }

        public SwitchState? Mic
        {
            get { return _mic; }
            set
            {
                GuardPropertySet(nameof(Mic), value,
                    () =>
                    {
                        _mic = value;
                        SetStringValue(PropertyKeys.mic_switch.ToString(), value.Value.ToString());
                    });
            }
        }

        public ImageQuality? Quality
        {
            get { return _quality; }
            set
            {
                GuardPropertySet(nameof(Ldc), value,
                    () =>
                    {
                        _quality = value;
                        SetStringValue(PropertyKeys.image_quality.ToString(), value.Value.ToString());
                    });
            }
        }

        public SwitchState? Osd
        {
            get { return _osd; }
            set
            {
                GuardPropertySet(nameof(Osd), value,
                    () =>
                    {
                        _osd = value;
                        SetStringValue(PropertyKeys.osd_switch.ToString(), value.Value.ToString());
                    });
            }
        }

        public SwitchState? OsdSpeed
        {
            get { return _osdSpeed; }
            set
            {
                GuardPropertySet(nameof(OsdSpeed), value,
                    () =>
                    {
                        _osdSpeed = value;
                        SetStringValue(PropertyKeys.osd_speedswitch.ToString(), value.Value.ToString());
                    });
            }
        }

        public SwitchState? StartSound
        {
            get { return _startSound; }
            set
            {
                GuardPropertySet(nameof(StartSound), value,
                    () =>
                    {
                        _startSound = value;
                        SetStringValue(PropertyKeys.start_sound_switch.ToString(), value.Value.ToString());
                    });
            }
        }

        public SwitchState? ParkingMode
        {
            get { return _parkingMode; }
            set
            {
                GuardPropertySet(nameof(ParkingMode), value,
                    () =>
                    {
                        _parkingMode = value;
                        SetStringValue(PropertyKeys.parking_mode_switch.ToString(), value.Value.ToString());
                    });
            }
        }

        public SwitchState? TimeLapse
        {
            get { return _timeLapse; }
            set
            {
                GuardPropertySet(nameof(TimeLapse), value,
                    () =>
                    {
                        _timeLapse = value;
                        SetStringValue(PropertyKeys.timelapse_rec_switch.ToString(), value.Value.ToString());
                    });
            }
        }

        public SwitchState? HMirror
        {
            get { return _hmirror; }
            set
            {
                GuardPropertySet(nameof(HMirror), value,
                    () =>
                    {
                        _hmirror = value;
                        SetStringValue(PropertyKeys.horizontal_mirror.ToString(), value.Value.ToString());
                    });
            }
        }

        public int? DelayPoweroffTime
        {
            get { return _delayPoweroffTime; }
            set
            {
                GuardPropertySet(nameof(DelayPoweroffTime), value,
                    () =>
                    {
                        _delayPoweroffTime = value;
                        SetIntValue(PropertyKeys.delay_poweroff_time.ToString(), value.Value);
                    });
            }
        }

        public bool? IsNeedUpdate
        {
            get { return _isNeedUpdate; }
            set
            {
                GuardPropertySet(nameof(IsNeedUpdate), value,
                    () =>
                    {
                        _isNeedUpdate = value;
                        SetStringValue(PropertyKeys.is_need_update.ToString(), value.ToString());
                    });
            }
        }

        public event EventHandler<StateChangedEventArgs> StateChanged;

        public event EventHandler<NewFilesCreatedEventArgs> NewFilesCreated;

        public bool Connect(UserInfo userInfo)
        {
            bool result;

            _transport.Connect("193.168.0.1", 80);

            result = PerformConnect(userInfo);

            if (result)
            {
                User = userInfo;

                _mailboxTask = Task.Factory.StartNew(() => PollMailbox(Cts.Token), Cts.Token);
            }

            State = DeviceState.Connected;

            OnStateChanged();

            return result;
        }

        public void Disconnect()
        {
            try
            {
#warning logout
                ExecuteRequest(ApiConstants.GetMailboxData,
                    apiCommand => _transport.Execute(apiCommand), response => { _logger.Error(response.Data); });

                _transport.Disconnect();
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        protected void OnStateChanged()
        {
            StateChanged?.Invoke(this, new StateChangedEventArgs {DeviceState = State});
        }

        private void PollMailbox(CancellationToken cancellationToken)
        {
            do
            {
                Task.Delay(5000, cancellationToken).Wait(cancellationToken);

                // OK
                ExecuteRequest(ApiConstants.GetMailboxData,
                    apiCommand => _transport.Execute(apiCommand), response => { _logger.Error(response.Data); });

                //ExecuteRequest("API_GetTestdate",
                //    apiCommand => _transport.Execute(apiCommand), (response) =>
                //    {
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
            var result = false;

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
                            _transport.Execute(apiCommand, JsonConvert.SerializeObject(QueryParameters.Instance)),
                        LoadSettings);
                },
                () =>
                {
                    return ExecuteRequest(ApiConstants.GetStorageInfo,
                        apiCommand => _transport.Execute(apiCommand),
                        response => { Storage = JsonConvert.DeserializeObject<StorageInfo>(response.Data); });
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
            var parameters = JsonConvert.DeserializeObject<Parameters>(response.Data);

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
            switch ((PropertyKeys) Enum.Parse(typeof(PropertyKeys), parameter.Key))
            {
                case PropertyKeys.wdr_enable:
                    _wdr = (SwitchState) Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.gsensor_mode:
                    _gsmode = (GSensorMode) Enum.Parse(typeof(GSensorMode), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.cycle_record_space:
                    _cycleRecordSpace = int.Parse(parameter.Value.ToString());
                    break;
                case PropertyKeys.speaker_turn:
                    _speakerLevel = int.Parse(parameter.Value.ToString());
                    break;
                case PropertyKeys.default_user:
                    _defaultUser = parameter.Value.ToString();
                    break;
                case PropertyKeys.ldc_switch:
                    _ldc = (SwitchState) Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.anti_fog:
                    _antiFog = int.Parse(parameter.Value.ToString());
                    break;
                case PropertyKeys.is_need_update:
                    _isNeedUpdate = int.Parse(parameter.Value.ToString()) == 1;
                    break;
                case PropertyKeys.event_after_time:
                    _eventAfterTime = int.Parse(parameter.Value.ToString());
                    break;
                case PropertyKeys.event_before_time:
                    _eventBeforeTime = int.Parse(parameter.Value.ToString());
                    break;
                case PropertyKeys.mic_switch:
                    _mic = (SwitchState) Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.image_quality:
                    _quality = (ImageQuality) Enum.Parse(typeof(ImageQuality), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.display_mode:
                    _displayMode = (DisplayMode) int.Parse(parameter.Value.ToString());
                    break;
                case PropertyKeys.osd_switch:
                    _osd = (SwitchState) Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.osd_speedswitch:
                    _osdSpeed = (SwitchState) Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.start_sound_switch:
                    _startSound = (SwitchState) Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.delay_poweroff_time:
                    _delayPoweroffTime = int.Parse(parameter.Value.ToString());
                    break;
                case PropertyKeys.edog_switch:
                    _edogSwitch = (SwitchState) Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.parking_mode_switch:
                    _parkingMode = (SwitchState) Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.timelapse_rec_switch:
                    _timeLapse = (SwitchState) Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.horizontal_mirror:
                    _hmirror = (SwitchState) Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
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
                _logger.Critical($"[{apiCommand}] Execution Failed", exception);
                result = false;
            }

            return result;
        }

        private void SetStringValue(string key, string state)
        {
            var parameter = new Parameters {StringParameters = {new Parameter<string> {Key = key, Value = state}}};
            ExecuteRequest(ApiConstants.GeneralSave,
                apiCommand => _transport.Execute(apiCommand, JsonConvert.SerializeObject(parameter)), null);
        }

        private void SetIntValue(string key, int state)
        {
            var parameter = new Parameters {IntParameters = {new Parameter<int> {Key = key, Value = state}}};
            ExecuteRequest(ApiConstants.GeneralSave,
                apiCommand => _transport.Execute(apiCommand, JsonConvert.SerializeObject(parameter)), null);
        }

        private void GuardPropertySet(string propertyName, string value, Action assignValue)
        {
            if (value == null)
            {
                throw new ArgumentNullException(propertyName);
            }

            assignValue();
        }

        private void GuardPropertySet<T>(string propertyName, T? value, Action assignValue) where T : struct
        {
            if (!value.HasValue)
            {
                throw new ArgumentNullException(propertyName);
            }

            assignValue();
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

        public IList<string> GetFiles()
        {
            throw new NotImplementedException();
        }

        public IStreamDescriptor StreamFile(string filename)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
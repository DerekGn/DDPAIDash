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
using System.Threading;
using System.Threading.Tasks;
using DDPAIDash.Core.Cache;
using DDPAIDash.Core.Constants;
using DDPAIDash.Core.Events;
using DDPAIDash.Core.Logging;
using DDPAIDash.Core.Transports;
using DDPAIDash.Core.Types;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace DDPAIDash.Core
{
    public class Device : IDevice
    {
        private const int PollingInterval = 5000;

        private static readonly CancellationTokenSource Cts;

        private readonly IImageCache _imageCache;
        private readonly ITransport _transport;
        private readonly ILogger _logger;

        private DeviceState _state;

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

        public Device() : this(new HttpTransport(), new ImageCache(), EtwLogger.Instance)
        {
        }

        public Device(ITransport transport, IImageCache imageCache, ILogger logger)
        {
            _imageCache = imageCache;
            _transport = transport;
            _logger = logger;

            _imageCache.FlushAsync(new TimeSpan(30, 0, 0, 0));
        }

        public DeviceInfo Info { get; private set; }

        public UserInfo User { get; private set; }

        public StorageInfo Storage { get; private set; }

        public DeviceState State
        {
            get
            {
                return _state;
            }
            private set
            {
                _state = value;
                OnStateChanged(_state);
            }
        }

        public DeviceCapabilities Capabilities { get; private set; }

        public string SessionId { get; private set; }

        public string DefaultUser
        {
            get { return _defaultUser; }
            set
            {
                GuardPropertySet(nameof(DefaultUser), value,
                    (v) =>
                    {
                        _defaultUser = v;
                        SetStringValueAsync(PropertyKeys.default_user.ToString(), value);
                    });
            }
        }

        public GSensorMode? GsMode
        {
            get { return _gsmode; }
            set
            {
                GuardPropertySet(nameof(GsMode), value,
                    (v) =>
                    {
                        _gsmode = v;
                        SetStringValueAsync(PropertyKeys.gsensor_mode.ToString(), v.ToString());
                    });
            }
        }

        public int? CycleRecordSpace
        {
            get { return _cycleRecordSpace; }
            set
            {
                GuardPropertySet(nameof(CycleRecordSpace), value,
                    (v) =>
                    {
                        _cycleRecordSpace = v;
                        SetIntValueAsync(PropertyKeys.cycle_record_space.ToString(), v);
                    });
            }
        }

        public int? SpeakerLevel
        {
            get { return _speakerLevel; }
            set
            {
                GuardPropertySet(nameof(SpeakerLevel), value,
                    (v) =>
                    {
                        _speakerLevel = v;
                        SetIntValueAsync(PropertyKeys.speaker_turn.ToString(), v);
                    });
            }
        }

        public int? AntiFog
        {
            get { return _antiFog; }
            set
            {
                GuardPropertySet(nameof(AntiFog), value,
                    (v) =>
                    {
                        _antiFog = v;
                        SetIntValueAsync(PropertyKeys.anti_fog.ToString(), v);
                    });
            }
        }

        public int? EventAfterTime
        {
            get { return _eventAfterTime; }
            set
            {
                GuardPropertySet(nameof(EventAfterTime), value,
                    (v) =>
                    {
                        _eventAfterTime = v;
                        SetIntValueAsync(PropertyKeys.event_after_time.ToString(), v);
                    });
            }
        }

        public int? EventBeforeTime
        {
            get { return _eventBeforeTime; }
            set
            {
                GuardPropertySet(nameof(EventBeforeTime), value,
                    (v) =>
                    {
                        _eventBeforeTime = v;
                        SetIntValueAsync(PropertyKeys.event_before_time.ToString(), v);
                    });
            }
        }

        public DisplayMode? DisplayMode
        {
            get { return _displayMode; }
            set
            {
                GuardPropertySet(nameof(DisplayMode), value,
                    (v) =>
                    {
                        _displayMode = v;
                        SetIntValueAsync(PropertyKeys.display_mode.ToString(), (int)v);
                    });
            }
        }

        public SwitchState? EDog
        {
            get { return _edogSwitch; }
            set
            {
                GuardPropertySet(nameof(EDog), value,
                    (v) =>
                    {
                        _edogSwitch = v;
                        SetStringValueAsync(PropertyKeys.edog_switch.ToString(), v.ToString());
                    });
            }
        }

        public SwitchState? Wdr
        {
            get { return _wdr; }
            set
            {
                GuardPropertySet(nameof(Wdr), value,
                    (v) =>
                    {
                        _wdr = v;
                        SetStringValueAsync(PropertyKeys.wdr_enable.ToString(), v.ToString());
                    });
            }
        }

        public SwitchState? Ldc
        {
            get { return _ldc; }
            set
            {
                GuardPropertySet(nameof(Ldc), value,
                    (v) =>
                    {
                        _ldc = v;
                        SetStringValueAsync(PropertyKeys.ldc_switch.ToString(), v.ToString());
                    });
            }
        }

        public SwitchState? Mic
        {
            get { return _mic; }
            set
            {
                GuardPropertySet(nameof(Mic), value,
                    (v) =>
                    {
                        _mic = v;
                        SetStringValueAsync(PropertyKeys.mic_switch.ToString(), v.ToString());
                    });
            }
        }

        public ImageQuality? Quality
        {
            get { return _quality; }
            set
            {
                GuardPropertySet(nameof(Ldc), value,
                    (v) =>
                    {
                        _quality = v;
                        SetStringValueAsync(PropertyKeys.image_quality.ToString(), v.ToString());
                    });
            }
        }

        public SwitchState? Osd
        {
            get { return _osd; }
            set
            {
                GuardPropertySet(nameof(Osd), value,
                    (v) =>
                    {
                        _osd = v;
                        SetStringValueAsync(PropertyKeys.osd_switch.ToString(), v.ToString());
                    });
            }
        }

        public SwitchState? OsdSpeed
        {
            get { return _osdSpeed; }
            set
            {
                GuardPropertySet(nameof(OsdSpeed), value,
                    (v) =>
                    {
                        _osdSpeed = value;
                        SetStringValueAsync(PropertyKeys.osd_speedswitch.ToString(), v.ToString());
                    });
            }
        }

        public SwitchState? StartSound
        {
            get { return _startSound; }
            set
            {
                GuardPropertySet(nameof(StartSound), value,
                    (v) =>
                    {
                        _startSound = v;
                        SetStringValueAsync(PropertyKeys.start_sound_switch.ToString(), v.ToString());
                    });
            }
        }

        public SwitchState? ParkingMode
        {
            get { return _parkingMode; }
            set
            {
                GuardPropertySet(nameof(ParkingMode), value,
                    (v) =>
                    {
                        _parkingMode = v;
                        SetStringValueAsync(PropertyKeys.parking_mode_switch.ToString(), v.ToString());
                    });
            }
        }

        public SwitchState? TimeLapse
        {
            get { return _timeLapse; }
            set
            {
                GuardPropertySet(nameof(TimeLapse), value,
                    (v) =>
                    {
                        _timeLapse = v;
                        SetStringValueAsync(PropertyKeys.timelapse_rec_switch.ToString(), v.ToString());
                    });
            }
        }

        public SwitchState? HMirror
        {
            get { return _hmirror; }
            set
            {
                GuardPropertySet(nameof(HMirror), value,
                    (v) =>
                    {
                        _hmirror = v;
                        SetStringValueAsync(PropertyKeys.horizontal_mirror.ToString(), v.ToString());
                    });
            }
        }

        public int? DelayPoweroffTime
        {
            get { return _delayPoweroffTime; }
            set
            {
                GuardPropertySet(nameof(DelayPoweroffTime), value,
                    (v) =>
                    {
                        _delayPoweroffTime = v;
                        SetIntValueAsync(PropertyKeys.delay_poweroff_time.ToString(), v);
                    });
            }
        }

        public bool? IsNeedUpdate
        {
            get { return _isNeedUpdate; }
            set
            {
                GuardPropertySet(nameof(IsNeedUpdate), value,
                    (v) =>
                    {
                        _isNeedUpdate = v;
                        SetStringValueAsync(PropertyKeys.is_need_update.ToString(), v.ToString());
                    });
            }
        }

        public Uri BaseAddress => _transport.BaseAddress;

        public event EventHandler<SyncProgressEventArgs> SyncProgress;

        public event EventHandler<StateChangedEventArgs> StateChanged;

        public event EventHandler<VideoDeletedEventArgs> VideoDeleted;

        public event EventHandler<EventDeletedEventArgs> EventDeleted;

        public event EventHandler<EventAddedEventArgs> EventAdded;

        public event EventHandler<VideoAddedEventArgs> VideoAdded;

        public async Task<bool> ConnectAsync(UserInfo userInfo)
        {
            bool result;

            if (State == DeviceState.PoweredDown)
            {
                State = DeviceState.Connecting;

                _transport.Open("193.168.0.1", 80);

                result = await PerformConnect(userInfo);

                State = DeviceState.Connected;

                if (result)
                {
                    User = userInfo;

                    await LoadDeviceContent();
                    
                    _mailboxTask = Task.Factory.StartNew(() => PollMailboxAsync(Cts.Token));
                }
            }
            else
            {
                result = true;
            }

            return result;
        }

        public void Disconnect()
        {
            if ((State == DeviceState.Connected) || (State == DeviceState.Formatting))
            {
                _transport.Close();
            }
        }

        public async Task<bool> FormatAsync()
        {
            bool result = false;

            if (State == DeviceState.Connected)
            {
                result = await ExecuteRequestAsync(ApiConstants.MmcFormat, apiCommand => _transport.ExecuteAsync(apiCommand), responseMessage => 
                {
                    State = DeviceState.Formatting;
                });
            }

            return result;
        }

        public async Task<int> PairDeviceButtonAsync()
        {
            int result = 0;

            if (State == DeviceState.Connected)
            {
                await ExecuteRequestAsync(ApiConstants.ButtonMatch, apiCommand => _transport.ExecuteAsync(apiCommand), responseMessage =>
                {
                    var pairingTimer = JsonConvert.DeserializeObject<PairingTimer>(responseMessage.Data);
                    result = pairingTimer.WaitTime;
                    State = DeviceState.Pairing;
                });
            }

            return result;
        }

        public async Task<Stream> StreamFileAsync(string fileName)
        {
            return await _transport.GetFileAsync(fileName);
        }

        private  async Task LoadDeviceContent()
        {
            List<DeviceVideo> deviceVideos = await GetDeviceVideoList();
            List<DeviceEvent> deviceEvents = await GetDeviceEventsList();
            int remaining = deviceVideos.Count + deviceEvents.Count;
            int total = remaining;

            OnSyncProgress(new SyncProgressEventArgs(total, remaining));

            _logger.Verbose("Loading Device Videos");

            foreach (var deviceVideo in deviceVideos)
            {
                deviceVideo.ImageThumbnailStream = await LoadDeviceVideoThumbnailAsync(deviceVideo.Name);

                if (deviceVideo.ImageThumbnailStream != null)
                    OnVideoAdded(new VideoAddedEventArgs(deviceVideo));

                OnSyncProgress(new SyncProgressEventArgs(total, --remaining));
            }

            _logger.Verbose("Loading Device Event Files");

            foreach (var deviceEvent in deviceEvents)
            {
                await LoadDeviceEventThumbnail(deviceEvent);

                if (deviceEvent.ImageThumbnailStream != null && deviceEvent.VideoThumbnailStream != null)
                    OnEventAdded(new EventAddedEventArgs(deviceEvent));

                OnSyncProgress(new SyncProgressEventArgs(total, --remaining));
            }
        }

        private async Task<List<DeviceEvent>> GetDeviceEventsList()
        {
            DeviceEventList deviceEventList = null;

            await ExecuteRequestAsync(ApiConstants.EventListReq,
                apiCommand => _transport.ExecuteAsync(apiCommand), response =>
                {
                    deviceEventList = JsonConvert.DeserializeObject<DeviceEventList>(response.Data);

                    deviceEventList = deviceEventList ?? new DeviceEventList();
                });

            return deviceEventList.Events.FindAll(e => !string.IsNullOrWhiteSpace(e.ImageName));
        }

        private async Task<List<DeviceVideo>> GetDeviceVideoList()
        {
            DeviceVideoList deviceVideoList = null;

            await ExecuteRequestAsync(ApiConstants.PlaybackListReq,
                apiCommand => _transport.ExecuteAsync(apiCommand), response =>
                {
                    deviceVideoList = JsonConvert.DeserializeObject<DeviceVideoList>(response.Data);

                    deviceVideoList = deviceVideoList ?? new DeviceVideoList();
                });

            return deviceVideoList.Videos;
        }
        
        private async Task GetDeviceVideosAndProcess(Func<DeviceVideoList, Task> processingAction)
        {
            DeviceVideoList deviceVideoList = null;

            await ExecuteRequestAsync(ApiConstants.PlaybackListReq,
                apiCommand => _transport.ExecuteAsync(apiCommand), response =>
                {
                    deviceVideoList = JsonConvert.DeserializeObject<DeviceVideoList>(response.Data);

                    deviceVideoList = deviceVideoList ?? new DeviceVideoList();
                });

            await processingAction(deviceVideoList);
        }

        private async Task LoadDeviceEventThumbnail(DeviceEvent deviceEvent) 
        {
            var imageFileName = deviceEvent.ImageName.Replace("_L", "_T").Replace("_X", "_T");

            if (!await _imageCache.ContainsAsync(imageFileName))
            {
                _logger.Verbose($"Cache does not contain [{imageFileName}] retrieving from device");

                using (var imageStream = await _transport.GetFileAsync(imageFileName))
                {
                    await _imageCache.CacheAsync(imageFileName, imageStream);
                }
            }

            deviceEvent.ImageThumbnailStream = await _imageCache.GetThumbnailStreamAsync(imageFileName);
            deviceEvent.VideoThumbnailStream = await _imageCache.GetThumbnailStreamAsync(imageFileName);
        }

        private async void PollMailboxAsync(CancellationToken cancellationToken)
        {
            _logger.Info("Mailbox polling task started");

            try
            {
                do
                {
                    Task.Delay(PollingInterval, cancellationToken).Wait(cancellationToken);

                    await ExecuteRequestAsync(ApiConstants.GetMailboxData,
                        apiCommand => _transport.ExecuteAsync(apiCommand),
                        response => { HandleMailBoxResponseAsync(response.Data); });

                } while (!cancellationToken.IsCancellationRequested);
            }
            catch (Exception ex)
            {
                _logger.Critical("Unhandled Exception Occured in PollMailboxAsync", ex);
            }

            _logger.Info("Mailbox polling task completed");
        }

        private async void HandleMailBoxResponseAsync(string data)
        {
            DeviceVideoList currentDeviceVideos = null;
            var mailboxMessages = JsonConvert.DeserializeObject<MailBoxMessageList>(data);

            if (mailboxMessages.Messages.FindIndex(m => (m.Key == MailBoxMessageKeys.MSG_PlaybackListUpdate) || (m.Key == MailBoxMessageKeys.MSG_DeleteEvent) || (m.Key == MailBoxMessageKeys.MSG_EventOccured) || (m.Key == MailBoxMessageKeys.MSG_MMCWarning)) >= 0)
            {
                await UpdateStorageInfoAsync();
            }

            if (mailboxMessages.Messages.FindIndex(m => (m.Key == MailBoxMessageKeys.MSG_PlaybackListUpdate)) >= 0)
            {
                await GetDeviceVideosAndProcess(deviceVideos =>
                 {
                     currentDeviceVideos = deviceVideos;

                     return Task.FromResult(deviceVideos);
                 });
            }

            foreach (var message in mailboxMessages.Messages)
            {
                switch (message.Key)
                {
                    case MailBoxMessageKeys.Unknown:
                        _logger.Error($"Key: {message.Key} Data: {message.Data}");
                        break;
                    case MailBoxMessageKeys.MSG_PowerDown:
                        Cts.Cancel();
                        State = DeviceState.PoweredDown;
                        break;
                    case MailBoxMessageKeys.MSG_DeleteEvent:
                        _logger.Info($"Key: {message.Key} Data: {message.Data}");
                        HandleEventDeleted();
                        break;
                    case MailBoxMessageKeys.MSG_EventOccured:
                        HandleEventOccured(JsonConvert.DeserializeObject<DeviceEvent>(message.Data));
                        break;
                    case MailBoxMessageKeys.MSG_PlaybackListUpdate:
                        HandlePlaybackListUpdate(currentDeviceVideos, JsonConvert.DeserializeObject<VideosListUpdate>(message.Data));
                        break;
                    case MailBoxMessageKeys.MSG_PlaybackLiveSwitch:
                        _logger.Info($"Key: {message.Key} Data: {message.Data}");
                        break;
                    case MailBoxMessageKeys.MSG_MMCWarning:
                        HandleMmcWarning();
                        break;
                    case MailBoxMessageKeys.MSG_ButtonMatch:
                        _logger.Info($"Key: {message.Key} Data: {message.Data}");
                        break;
                    case MailBoxMessageKeys.MSG_RecordSizeWarning:
                        _logger.Info($"Key: {message.Key} Data: {message.Data}");
                        break;
                }
            }
        }

        private void HandleMmcWarning()
        {
            State = DeviceState.Connected;
        }

        private void HandleEventDeleted()
        {
#warning todo
            OnEventDeleted(new EventDeletedEventArgs(null));
        }

        private async void HandleEventOccured(DeviceEvent deviceEvent)
        {
            // Skip the event that has no video
            if (!string.IsNullOrEmpty(deviceEvent.BVideoName))
            {
                await LoadDeviceEventThumbnail(deviceEvent);

                OnEventAdded(new EventAddedEventArgs(deviceEvent));
            }
        }

        private async void HandlePlaybackListUpdate(DeviceVideoList deviceVideoList, VideosListUpdate videosListUpdate)
        {
            if (videosListUpdate.Action == PlaybackAction.Add)
            {
                var deviceVideo = deviceVideoList.Videos.Find(df => df.Name == videosListUpdate.Name);

                if (deviceVideo != null)
                {
                    deviceVideo.ImageThumbnailStream = await LoadDeviceVideoThumbnailAsync(deviceVideo.Name);

                    if(deviceVideo.ImageThumbnailStream != null)
                        OnVideoAdded(new VideoAddedEventArgs(deviceVideo));
                }
                else
                {
                    _logger.Error($"Video {videosListUpdate.Name} in current device video list");
                }
            }
            else if (videosListUpdate.Action == PlaybackAction.Delete)
            {
                OnVideoDeleted(new VideoDeletedEventArgs(videosListUpdate.Name));
            }
        }

        private async Task<bool> PerformConnect(UserInfo userInfo)
        {
            var result = false;

            var connectActions = new List<Func<Task<bool>>>
            {
                async () =>
                {
                    return await ExecuteRequestAsync(ApiConstants.RequestSession, apiCommand => _transport.ExecuteAsync(apiCommand),
                        response =>
                        {
                            SessionId =
                                _transport.SessionId =
                                    JsonConvert.DeserializeAnonymousType(response.Data, new {acsessionid = string.Empty})
                                        .acsessionid;
                        });
                },
                async () =>
                {
                    return await ExecuteRequestAsync(ApiConstants.RequestCertificate,
                        apiCommand => _transport.ExecuteAsync(apiCommand, JsonConvert.SerializeObject(userInfo)), null);
                },
                async () =>
                {
                    return await ExecuteRequestAsync(ApiConstants.SyncDate,
                        apiCommand =>
                            _transport.ExecuteAsync(apiCommand, JsonConvert.SerializeObject(TimeZoneSettings.Instance)), null);
                },
                async () =>
                {
                    return await ExecuteRequestAsync(ApiConstants.GetBaseInfo, apiCommand => _transport.ExecuteAsync(apiCommand),
                        response => { Info = JsonConvert.DeserializeObject<DeviceInfo>(response.Data); });
                },
                async () =>
                {
                    return await ExecuteRequestAsync(ApiConstants.AvCapReq, apiCommand => _transport.ExecuteAsync(apiCommand),
                        response => { Capabilities = JsonConvert.DeserializeObject<DeviceCapabilities>(response.Data); });
                },
                async () =>
                {
                    return await ExecuteRequestAsync(ApiConstants.AvCapSet,
                        apiCommand =>
                            _transport.ExecuteAsync(apiCommand, JsonConvert.SerializeObject(new StreamSettings(30, 0))), null);
                },
                async () =>
                {
                    return await ExecuteRequestAsync(ApiConstants.GeneralQuery,
                        apiCommand =>
                            _transport.ExecuteAsync(apiCommand, JsonConvert.SerializeObject(QueryParameters.Instance)),
                        LoadSettings);
                },
                async () => await UpdateStorageInfoAsync()
            };

            foreach (var action in connectActions)
            {
                result = await action();

                if (!result)
                    break;
            }

            return result;
        }

        private async Task<bool> UpdateStorageInfoAsync()
        {
            return await ExecuteRequestAsync(ApiConstants.GetStorageInfo,
                apiCommand => _transport.ExecuteAsync(apiCommand),
                response => { Storage = JsonConvert.DeserializeObject<StorageInfo>(response.Data); });
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
            switch ((PropertyKeys)Enum.Parse(typeof(PropertyKeys), parameter.Key))
            {
                case PropertyKeys.wdr_enable:
                    _wdr = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.gsensor_mode:
                    _gsmode = (GSensorMode)Enum.Parse(typeof(GSensorMode), parameter.Value.ToString(), true);
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
                    _ldc = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
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
                    _mic = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.image_quality:
                    _quality = (ImageQuality)Enum.Parse(typeof(ImageQuality), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.display_mode:
                    _displayMode = (DisplayMode)int.Parse(parameter.Value.ToString());
                    break;
                case PropertyKeys.osd_switch:
                    _osd = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.osd_speedswitch:
                    _osdSpeed = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.start_sound_switch:
                    _startSound = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.delay_poweroff_time:
                    _delayPoweroffTime = int.Parse(parameter.Value.ToString());
                    break;
                case PropertyKeys.edog_switch:
                    _edogSwitch = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.parking_mode_switch:
                    _parkingMode = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.timelapse_rec_switch:
                    _timeLapse = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
                case PropertyKeys.horizontal_mirror:
                    _hmirror = (SwitchState)Enum.Parse(typeof(SwitchState), parameter.Value.ToString(), true);
                    break;
            }
        }

        private async Task<bool> ExecuteRequestAsync(string apiCommand, Func<string, Task<ResponseMessage>> transportAction,
            Action<ResponseMessage> responseAction)
        {
            var result = true;

            try
            {
                var response = await transportAction(apiCommand);

                if (response.ErrorCode == 0)
                {
                    _logger.Verbose($"[{apiCommand}] Execution Succeded. Data: [{response.Data}]");
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

        private async void SetStringValueAsync(string key, string state)
        {
            var parameter = new Parameters { StringParameters = { new Parameter<string> { Key = key, Value = state.ToLower() } } };
            await ExecuteRequestAsync(ApiConstants.GeneralSave,
                apiCommand => _transport.ExecuteAsync(apiCommand, JsonConvert.SerializeObject(parameter)), null);
        }

        private async void SetIntValueAsync(string key, int state)
        {
            var parameter = new Parameters { IntParameters = { new Parameter<int> { Key = key, Value = state } } };
            await ExecuteRequestAsync(ApiConstants.GeneralSave,
                apiCommand => _transport.ExecuteAsync(apiCommand, JsonConvert.SerializeObject(parameter)), null);
        }

        private static void GuardPropertySet(string propertyName, string value, Action<string> assignValue)
        {
            if (value == null)
            {
                throw new ArgumentNullException(propertyName);
            }

            assignValue(value);
        }

        private static void GuardPropertySet<T>(string propertyName, T? value, Action<T> assignValue) where T : struct
        {
            if (!value.HasValue)
            {
                throw new ArgumentNullException(propertyName);
            }

            assignValue(value.Value);
        }
        
        private async Task<Stream> LoadDeviceVideoThumbnailAsync(string deviceVideoName)
        {
            var baseFileName = deviceVideoName.Substring(0, 14);

            if (!await _imageCache.ContainsAsync(baseFileName))
            {
                _logger.Verbose($"Cache does not contain [{baseFileName}] retrieving from device");

                using (var tarStream = await _transport.GetFileAsync(string.Concat(baseFileName, ".tar")))
                {
                    await _imageCache.CacheAsync(tarStream);
                }
            }

            return await _imageCache.GetThumbnailStreamAsync(baseFileName);
        }

        protected void OnStateChanged(DeviceState state)
        {
            var temp = StateChanged;

            temp?.Invoke(this, new StateChangedEventArgs(State));
        }

        protected void OnVideoAdded(VideoAddedEventArgs addedEventArgs)
        {
            var temp = VideoAdded;

            temp?.Invoke(this, addedEventArgs);
        }

        protected void OnVideoDeleted(VideoDeletedEventArgs deletedEventArgs)
        {
            var temp = VideoDeleted;

            temp?.Invoke(this, deletedEventArgs);
        }

        protected void OnEventDeleted(EventDeletedEventArgs eventDeletedEventArgs)
        {
            var temp = EventDeleted;

            temp?.Invoke(this, eventDeletedEventArgs);
        }

        protected void OnEventAdded(EventAddedEventArgs eventAddedEventArgs)
        {
            var temp = EventAdded;

            temp?.Invoke(this, eventAddedEventArgs);
        }

        protected void OnSyncProgress(SyncProgressEventArgs syncProgressEventArgs)
        {
            var temp = SyncProgress;

            temp?.Invoke(this, syncProgressEventArgs);
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
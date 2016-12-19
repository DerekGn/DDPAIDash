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
using DDPAIDash.Core.Types;
using DDPAIDash.Core.Events;

namespace DDPAIDash.Core
{
    public interface IDevice : IDisposable
    {
        DeviceInfo Info { get; }

        UserInfo User { get; }

        StorageInfo Storage { get; }

        DeviceState State { get; }

        DeviceCapabilities Capabilities { get; }

        string SessionId { get; }

        GSensorMode? GsMode { get; set; }

        int? CycleRecordSpace { get; set; }

        string DefaultUser { get; set; }

        int? SpeakerLevel { get; set; }

        int? AntiFog { get; set; }

        int? EventAfterTime { get; set; }

        int? EventBeforeTime { get; set; }

        DisplayMode? DisplayMode { get; set; }

        int? DelayPoweroffTime { get; set; }

        bool? IsNeedUpdate { get; set; }

        SwitchState? HMirror { get; set; }

        SwitchState? EDog { get; set; }

        SwitchState? Wdr { get; set; }

        SwitchState? Ldc { get; set; }

        SwitchState? Mic { get; set; }

        ImageQuality? Quality { get; set; }

        SwitchState? Osd { get; set; }

        SwitchState? OsdSpeed { get; set; }

        SwitchState? StartSound { get; set; }

        SwitchState? ParkingMode { get; set; }

        SwitchState? TimeLapse { get; set; }

        Uri BaseAddress { get; }

        /// <summary>
        /// Connect to the <see cref="IDevice"/> instance
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        bool Connect(UserInfo userInfo);

        /// <summary>
        /// Disconnect from the <see cref="IDevice"/> instance
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Get a list of files on the device
        /// </summary>
        /// <returns>An instance of <see cref="DeviceFileList"/> that details all files on the devices ssd</returns>
        DeviceFileList GetFiles();
        
        DeviceEventList GetEvents();
        
        /// <summary>
        /// Stream a file from the <see cref="IDevice"/> instance
        /// </summary>
        /// <returns>An instance of a <see cref="IStreamDescriptor"/></returns>
        IStreamDescriptor StreamFile(string filename);
        
        /// <summary>
        /// Raised when the <see cref="IDevice"/> state changes
        /// </summary>
        event EventHandler<StateChangedEventArgs> StateChanged;

        /// <summary>
        /// Raised when the <see cref="IDevice"/> udates its file list
        /// </summary>
        event EventHandler<FilesChangedEventArgs> FilesChanged;

        /// <summary>
        /// Raised when a new event occurs <see cref="IDevice"/>
        /// </summary>
        event EventHandler<EventOccuredEventArgs> EventOccured;
    }
}
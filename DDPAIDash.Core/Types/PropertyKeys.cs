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

using System.Diagnostics.CodeAnalysis;

namespace DDPAIDash.Core.Types
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal enum PropertyKeys
    {
        wdr_enable,
        gsensor_mode,
        cycle_record_space,
        speaker_turn,
        default_user,
        ldc_switch,
        anti_fog,
        is_need_update,
        event_after_time,
        event_before_time,
        mic_switch,
        image_quality,
        display_mode,
        osd_switch,
        osd_speedswitch,
        start_sound_switch,
        delay_poweroff_time,
        edog_switch,
        parking_mode_switch,
        timelapse_rec_switch,
        horizontal_mirror,
        parking_power_mgr,
        button_match,
        record_split_time,
        vertical_mirror
    }
}

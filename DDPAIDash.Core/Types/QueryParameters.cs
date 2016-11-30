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
using Newtonsoft.Json;

namespace DDPAIDash.Core.Types
{
    internal class QueryParameters
    {
        private QueryParameters()
        {
            Keys = new List<QueryParameter>()
            {
                new QueryParameter() { Key = PropertyKeys.wdr_enable.ToString() },
                new QueryParameter() { Key = PropertyKeys.gsensor_mode.ToString() },
                new QueryParameter() { Key = PropertyKeys.cycle_record_space.ToString() },
                new QueryParameter() { Key = PropertyKeys.speaker_turn.ToString() },
                new QueryParameter() { Key = PropertyKeys.default_user.ToString() },
                new QueryParameter() { Key = PropertyKeys.ldc_switch.ToString() },
                new QueryParameter() { Key = PropertyKeys.anti_fog.ToString() },
                new QueryParameter() { Key = PropertyKeys.is_need_update.ToString() },
                new QueryParameter() { Key = PropertyKeys.event_after_time.ToString() },
                new QueryParameter() { Key = PropertyKeys.event_before_time.ToString() },
                new QueryParameter() { Key = PropertyKeys.mic_switch.ToString() },
                new QueryParameter() { Key = PropertyKeys.image_quality.ToString() },
                new QueryParameter() { Key = PropertyKeys.display_mode.ToString() },
                new QueryParameter() { Key = PropertyKeys.osd_switch.ToString() },
                new QueryParameter() { Key = PropertyKeys.osd_speedswitch.ToString() },
                new QueryParameter() { Key = PropertyKeys.start_sound_switch.ToString() },
                new QueryParameter() { Key = PropertyKeys.delay_poweroff_time.ToString() },
                new QueryParameter() { Key = PropertyKeys.edog_switch.ToString() },
                new QueryParameter() { Key = PropertyKeys.parking_mode_switch.ToString() },
                new QueryParameter() { Key = PropertyKeys.timelapse_rec_switch.ToString() },
                new QueryParameter() { Key = PropertyKeys.horizontal_mirror.ToString() }
            };
        }

        private static readonly Lazy<QueryParameters> InstanceLazy =
            new Lazy<QueryParameters>(() => new QueryParameters());

        public static QueryParameters Instance => InstanceLazy.Value;

        [JsonProperty("keys")]
        public List<QueryParameter> Keys { get; set; }
    }
}

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
                new QueryParameter() { Key = "wdr_enable" },
                new QueryParameter() { Key = "gsensor_mode" },
                new QueryParameter() { Key = "cycle_record_space" },
                new QueryParameter() { Key = "speaker_turn" },
                new QueryParameter() { Key = "default_user" },
                new QueryParameter() { Key = "ldc_switch" },
                new QueryParameter() { Key = "anti_fog" },
                new QueryParameter() { Key = "is_need_update" },
                new QueryParameter() { Key = "event_after_time" },
                new QueryParameter() { Key = "event_before_time" },
                new QueryParameter() { Key = "mic_switch" },
                new QueryParameter() { Key = "image_quality" },
                new QueryParameter() { Key = "display_mode" },
                new QueryParameter() { Key = "osd_switch" },
                new QueryParameter() { Key = "osd_speedswitch" },
                new QueryParameter() { Key = "start_sound_switch" },
                new QueryParameter() { Key = "delay_poweroff_time" },
                new QueryParameter() { Key = "edog_switch" },
                new QueryParameter() { Key = "parking_mode_switch" },
                new QueryParameter() { Key = "timelapse_rec_switch" },


            };
        }

        private static readonly Lazy<QueryParameters> InstanceLazy =
            new Lazy<QueryParameters>(() => new QueryParameters());

        [JsonProperty("keys")]
        public List<QueryParameter> Keys { get; set; }
    }
}

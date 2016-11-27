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

using Newtonsoft.Json;

namespace DDPAIDash.Core.Types
{
    public class DeviceInfo
    {
        [JsonProperty("nickname")]
        public string Nickname { get; private set; }

        [JsonProperty("ordernum")]
        public string OrderNumber { get; private set; }

        [JsonProperty("model")]
        public string Model { get; private set; }

        [JsonProperty("version")]
        public string Version { get; private set; }

        [JsonProperty("uuid")]
        public string Uuid { get; private set; }

        [JsonProperty("macaddr")]
        public string MacAddr { get; private set; }

        [JsonProperty("chipsn")]
        public string ChipSerialNumber { get; private set; }

        [JsonProperty("legalret")]
        public int LegalRet { get; private set; }

        [JsonProperty("btnver")]
        public int ButtonVersion { get; private set; }

        [JsonProperty("totalruntime")]
        public int TotalRuntime { get; private set; }

        [JsonProperty("sdcapacity")]
        public int SdCapacity { get; private set; }

        [JsonProperty("sdspare")]
        public int SdSpare { get; private set; }

        [JsonProperty("hbbitrate")]
        public int HbBitrate { get; private set; }

        [JsonProperty("hsbitrate")]
        public int HsBitrate { get; private set; }

        [JsonProperty("mbbitrate")]
        public int MbBitrate { get; private set; }

        [JsonProperty("msbitrate")]
        public int MsBitrate { get; private set; }

        [JsonProperty("lbbitrate")]
        public int LbBitrate { get; private set; }

        [JsonProperty("lsbitrate")]
        public int LsBitrate { get; private set; }

        [JsonProperty("default_user")]
        public string DefaultUser { get; private set; }

        [JsonProperty("is_neeed_update")]
        public bool IsNeeedUpdate { get; private set; }

        [JsonProperty("edog_model")]
        public string EdogModel { get; private set; }

        [JsonProperty("edog_version")]
        public string EDogVersion { get; private set; }

        [JsonProperty("edog_status")]
        public bool EDogStatus { get; private set; }
    }
}
namespace DDPAIDash.Core
{
    internal class DeviceInfo
    {
        //{"errcode":0,"data":"{\"nickname\":\"vYou_DDPai_MINI9\",\"ordernum\":\"0410Mini\",\"model\":\"DDPai Mini\",\"version\":\"v3.2.1.5\",\"uuid\":\"04112016-1601-0000-0018-000000068454\",\"macaddr\":\"00:e0:01:00:e6:47\",
        //\"chipsn\":\"\",\"legalret\":1,\"btnver\":3,\"totalruntime\":122240,\"sdcapacity\":31248384,\"sdspare\":9789184,\"hbbitrate\":8192,\"hsbitrate\":1024,\"mbbitrate\":7168,\"msbitrate\":1536,\"lbbitrate\":4096,\"lsbitrate\":1024,\"default_user\":\"012345678912345\",\"is_neeed_update\":0,\"edog_model\":\"\",\"edog_version\":\"\",\"edog_status\":2}"}
        public string Nickname { get; private set; }

        public string OrderNumber { get; private set; }

        public string Model { get; private set; }

        public string Version { get; private set; }

        public string Uuid { get; private set; }

        public string MacAddr { get; private set; }

        public string ChipSerialNumber { get; private set; }

        public int ButtonVersion { get; private set; }

        public int TotalRuntime { get; private set; }
    }
}
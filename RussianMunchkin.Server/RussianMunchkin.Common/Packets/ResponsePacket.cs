using MessagePack;

namespace RussianMunchkin.Common.Packets
{
    [MessagePackObject]
    public class ResponsePacket: Packet
    {
        [Key(0)]
        public bool IsSuccess { get; set; }
        [Key(1)]
        public string Log { get; set; }

        public ResponsePacket(bool isSuccess, string log = "")
        {
            IsSuccess = isSuccess;
            Log = log;
        }
    }
}
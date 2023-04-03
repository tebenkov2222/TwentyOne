using MessagePack;

namespace RussianMunchkin.Common.Packets
{
    [MessagePackObject]
    public class RequestConnectToRoomPacket: Packet
    {
        [Key(0)]
        public string Uid { get; set; }
        [Key(1)]
        public string Password { get; set; }
    }
}
using MessagePack;

namespace RussianMunchkin.Common.Packets
{
    [MessagePackObject]
    public class ExitFromRoomPacket: Packet
    {
        [Key(0)]
        public int PlayerId { get; set; }
    }
}
using MessagePack;

namespace RussianMunchkin.Common.Packets
{
    [MessagePackObject]
    public class CreateRoomPacket: Packet
    {
        [Key(0)]
        public bool IsPrivate { get; set; }
        [Key(1)]
        public int MaxCountPlayers{ get; set; }
    }
}
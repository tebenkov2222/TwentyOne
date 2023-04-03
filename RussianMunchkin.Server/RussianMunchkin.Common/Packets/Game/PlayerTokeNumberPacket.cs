using MessagePack;

namespace RussianMunchkin.Common.Packets.Game
{
    [MessagePackObject]
    public class PlayerTokeNumberPacket: Packet
    {
        [Key(0)]
        public int PlayerId { get; set; }
    }
}
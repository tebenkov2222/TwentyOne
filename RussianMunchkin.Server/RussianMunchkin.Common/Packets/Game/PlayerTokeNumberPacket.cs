using MessagePack;

namespace RussianMunchkin.Common.Packets.Game
{
    [MessagePackObject]
    public class PlayerTokeNumberPacket: Packet
    {
        [Key(0)]
        public string PlayerLogin { get; set; }
    }
}
using MessagePack;

namespace RussianMunchkin.Common.Packets.Game
{
    [MessagePackObject]
    public class PlayerReadyGamePacket: Packet
    {
        [Key(0)]
        public string PlayerLogin { get; set; }
    }
}
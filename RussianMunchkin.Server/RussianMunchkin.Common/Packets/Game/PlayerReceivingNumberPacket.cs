using MessagePack;

namespace RussianMunchkin.Common.Packets.Game
{
    [MessagePackObject]
    public class PlayerReceivingNumberPacket: Packet
    {
        [Key(0)]
        public int Number { get; set; }
    }
}
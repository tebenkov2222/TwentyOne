using MessagePack;

namespace RussianMunchkin.Common.Packets
{
    [MessagePackObject]
    public class ChangeStatusStartGamePacket: Packet
    {
        [Key(0)]
        public bool IsReady { get; set; }
    }
}
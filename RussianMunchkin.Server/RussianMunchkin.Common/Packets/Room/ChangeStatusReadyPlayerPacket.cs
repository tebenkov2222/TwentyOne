using MessagePack;

namespace RussianMunchkin.Common.Packets
{
    [MessagePackObject]
    public class ChangeStatusReadyPlayerPacket: Packet
    {
        [Key(0)]
        public string PlayerLogin { get; set; }
        [Key(1)]
        public bool IsReady { get; set; }
    }
}
using MessagePack;

namespace RussianMunchkin.Common.Packets
{
    [MessagePackObject]
    public class ChangeStatusGamePacket: Packet
    {
        [Key(0)]
        public bool IsStartGame { get; set; }   
    }
}
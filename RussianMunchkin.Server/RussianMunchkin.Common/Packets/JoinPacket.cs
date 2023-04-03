using MessagePack;

namespace RussianMunchkin.Common.Packets
{
    [MessagePackObject]
    public class JoinPacket: Packet
    {
        [Key(0)]
        public string Username;
    }
}
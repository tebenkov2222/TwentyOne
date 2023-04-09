using MessagePack;

namespace RussianMunchkin.Common.Packets.Auth
{
    [MessagePackObject]
    public class CheckLoginPacket : Packet
    {
        [Key(0)] public string Login { get; set; }
    }
}
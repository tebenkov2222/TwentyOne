using MessagePack;

namespace RussianMunchkin.Common.Packets.Auth
{
    [MessagePackObject]
    public class AuthorizationPacket : Packet
    {
        [Key(0)] public string Login { get; set; }
        [Key(1)] public string Password { get; set; }
    }
}
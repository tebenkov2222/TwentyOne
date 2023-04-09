using MessagePack;

namespace RussianMunchkin.Common.Packets.Auth
{
    [MessagePackObject]
    public class AuthorizationResultPacket : Packet
    {
        [Key(0)]public int UserId { get; set; }
        [Key(1)]public string Login { get; set; }
        [Key(2)]public string Token { get; set; }
    }
}
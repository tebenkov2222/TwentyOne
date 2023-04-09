using RussianMunchkin.Common.Packets.Auth;
using ServerFramework;

namespace RussianMunchkin.Common.Packets
{
    [MessagePack.Union(1000, typeof(AuthorizationPacket))]
    [MessagePack.Union(1001, typeof(AuthorizationResultPacket))]
    [MessagePack.Union(1002, typeof(RegistrationPacket))]
    [MessagePack.Union(1003, typeof(CheckLoginPacket))]
    public abstract partial class Packet
    {
    }
}
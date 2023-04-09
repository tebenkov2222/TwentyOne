using RussianMunchkin.Common.Packets.Auth;
using RussianMunchkin.Common.Packets.Game;
using ServerFramework;

namespace RussianMunchkin.Common.Packets
{
    [MessagePack.Union(2003, typeof(PlayerReadyGamePacket))]
    [MessagePack.Union(2004, typeof(PlayerReceivingNumberPacket))]
    [MessagePack.Union(2005, typeof(PlayerTokeNumberPacket))]
    [MessagePack.Union(2006, typeof(RequestGetNumberPacket))]
    [MessagePack.Union(2007, typeof(ShowResultsPacket))]
    [MessagePack.Union(2008, typeof(StartSessionPacket))]
    [MessagePack.Union(2009, typeof(RestartSessionPacket))]
    public abstract partial class Packet
    {
    }
}
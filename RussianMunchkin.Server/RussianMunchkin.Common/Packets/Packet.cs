using RussianMunchkin.Common.Models;
using RussianMunchkin.Common.Packets.Game;
using ServerFramework;

namespace RussianMunchkin.Common.Packets
{
    [MessagePack.Union(0, typeof(ResponsePacket))]

    public abstract partial class Packet: IPacket
    {
        
    }
}
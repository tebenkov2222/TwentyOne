using RussianMunchkin.Common.Packets;
using RussianMunchkin.Server.Core.Player.Interfaces;
using RussianMunchkin.Server.Core.Player.Server;

namespace RussianMunchkin.Server.Core.PacketsHandler.Decorator
{
    public class PacketHandler<TPlayer>: IPacketsHandler<TPlayer> where TPlayer: INetPlayer
    {
        public void Handle(TPlayer player, Packet packet)
        {
            
        }
    }
}
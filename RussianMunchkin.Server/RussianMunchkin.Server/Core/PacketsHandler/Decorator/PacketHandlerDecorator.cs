using RussianMunchkin.Common.Packets;
using RussianMunchkin.Server.Core.Player.Interfaces;
using RussianMunchkin.Server.Core.Player.Server;

namespace RussianMunchkin.Server.Core.PacketsHandler.Decorator
{
    public abstract class PacketHandlerDecorator<TPlayer>: IPacketsHandler<TPlayer> where TPlayer: INetPlayer
    {
        private IPacketsHandler<TPlayer> _previewHandler;

        protected PacketHandlerDecorator(IPacketsHandler<TPlayer> previewHandler)
        {
            _previewHandler = previewHandler;
        }

        public void Handle(TPlayer player, Packet packet)
        {
            if (!TryHandle(player, packet))
            {
                _previewHandler.Handle(player,packet);
            }
        }

        protected abstract bool TryHandle(TPlayer player, Packet packet);
    }
}
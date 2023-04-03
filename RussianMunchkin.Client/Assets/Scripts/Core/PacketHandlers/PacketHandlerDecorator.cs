using Core;
using Core.PacketHandlers;
using RussianMunchkin.Common.Packets;
using ServerFramework;

namespace PacketHandlers.Core
{
    public abstract class PacketHandlerDecorator: IPacketsHandler
    {
        private IPacketsHandler _previewHandler;
        protected readonly NetPeer NetPeer;

        protected PacketHandlerDecorator(IPacketsHandler previewHandler, NetPeer netPeer)
        {
            _previewHandler = previewHandler;
            NetPeer = netPeer;
        }

        public void Handle(Packet packet)
        {
            if (!TryHandle(packet))
            {
                _previewHandler.Handle(packet);
            }
        }

        protected abstract bool TryHandle(Packet packet);
    }
}
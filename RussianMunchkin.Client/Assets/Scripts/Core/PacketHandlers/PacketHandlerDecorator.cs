using Core;
using Core.PacketHandlers;
using RussianMunchkin.Common.Packets;
using ServerFramework;

namespace PacketHandlers.Core
{
    public abstract class PacketHandlerDecorator: IPacketsHandler
    {
        private IPacketsHandler _previewHandler;
        protected readonly Peer _peer;

        protected PacketHandlerDecorator(IPacketsHandler previewHandler, Peer peer)
        {
            _previewHandler = previewHandler;
            _peer = peer;
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
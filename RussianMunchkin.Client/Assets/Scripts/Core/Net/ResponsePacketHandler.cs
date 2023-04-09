using Core.PacketHandlers;
using PacketHandlers.Core;
using RussianMunchkin.Common.Packets;

namespace Core.Net
{
    public class ResponsePacketHandler: PacketHandlerDecorator
    {
        public ResponsePacketHandler(IPacketsHandler previewHandler, Peer peer) : base(previewHandler, peer)
        {
        }

        protected override bool TryHandle(Packet packet)
        {
            if (packet is not ResponsePacket responsePacket) return false;
            _peer.ResponseHandle(responsePacket);
            return true;
        }
    }
}
using Core.PacketHandlers;
using PacketHandlers.Core;
using RussianMunchkin.Common.Packets;

namespace Core.Net
{
    public class ResponsePacketHandler: PacketHandlerDecorator
    {
        public ResponsePacketHandler(IPacketsHandler previewHandler, NetPeer netPeer) : base(previewHandler, netPeer)
        {
        }

        protected override bool TryHandle(Packet packet)
        {
            if (packet is not ResponsePacket responsePacket) return false;
            NetPeer.ResponseHandle(responsePacket);
            return true;
        }
    }
}
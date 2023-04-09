using RussianMunchkin.Common.Packets;
using RussianMunchkin.Server.Core.PacketsHandler.Decorator;
using RussianMunchkin.Server.Core.Player;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server;

namespace RussianMunchkin.Server.Server;

public class ResponsePacketHandler: PacketHandlerDecorator<ServerPlayerToController>
{
    public ResponsePacketHandler(IPacketsHandler<ServerPlayerToController> previewHandler) : base(previewHandler)
    {
    }

    protected override bool TryHandle(ServerPlayerToController player, Packet packet)
    {
        switch (packet)
        {
            case ResponsePacket responsePacket:
                player.Peer.ResponseHandle(responsePacket);
                break;
            default: return false;
        }

        return true;
    }
}
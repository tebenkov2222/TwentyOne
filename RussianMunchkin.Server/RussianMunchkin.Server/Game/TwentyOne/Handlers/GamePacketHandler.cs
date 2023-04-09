using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Packets.Game;
using RussianMunchkin.Server.Core.PacketsHandler.Decorator;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server;

namespace RussianMunchkin.Server.Game.TwentyOne.Handlers
{
    public class GamePacketHandler: PacketHandlerDecorator<ServerPlayerToController>
    {
        public GamePacketHandler(IPacketsHandler<ServerPlayerToController> previewHandler) : base(previewHandler)
        {
        }

        protected override bool TryHandle(ServerPlayerToController player, Packet packet)
        {
            switch (packet)
            {
                case RequestGetNumberPacket:
                    
                    player.GameController.TakeNumber();
                    break;
                case PlayerReadyGamePacket:
                    
                    player.GameController.PlayerReadyToShow();
                    break;
                case RestartSessionPacket:
                    
                    player.GameController.ReadyToRestart();
                    break;
                default: return false;
            }
            player.Peer.SendResponse(true);
            return true;
        }
    }
}
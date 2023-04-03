using System;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Server.Core.PacketsHandler.Decorator;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server;

namespace RussianMunchkin.Server.Handlers
{
    public class AuthUsernamePacketHandler: PacketHandlerDecorator<ServerPlayerToController> 
    {
        public AuthUsernamePacketHandler(IPacketsHandler<ServerPlayerToController> previewHandler) : base(previewHandler)
        {
        }

        protected override  bool TryHandle(ServerPlayerToController player, Packet packet)
        {
            switch (packet)
            {
                case JoinPacket joinPacket:
                    player.AuthController.SetUsername(joinPacket.Username);
                    player.NetPeer.SendResponse(true, player.PlayerModel.PlayerId.ToString());
                    Console.WriteLine($"Join player to room. Id = {player.NetModel.NetId}, Username = {player.AuthController.AuthModel.Username}");
                    break;
                default: return false;
            }
            return true;
        }
    }
}
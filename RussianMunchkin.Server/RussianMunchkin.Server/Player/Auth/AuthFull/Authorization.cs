using System;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Packets.Auth;
using RussianMunchkin.Server.DataBase;
using RussianMunchkin.Server.DataBase.Models;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server;
using RussianMunchkin.Server.Mapper;

namespace RussianMunchkin.Server.Player.Auth.AuthFull
{
    public class Authorization
    {
        private readonly DataBaseController _dataBaseController;
        private readonly AuthController _authController;

        public Authorization(DataBaseController dataBaseController, AuthController authController)
        {
            _dataBaseController = dataBaseController;
            _authController = authController;
        }

        public async void AuthorizationClient(ServerPlayerToController player, AuthorizationPacket authorizationPacket)
        {
            var userModelFromClint = MapperInstance.Mapper.Map<AuthorizationModel>(authorizationPacket);
            var userModelFromDb = await _dataBaseController.Select.GetAuthorizationModel(userModelFromClint);
            if (userModelFromDb == null)
            {
                player.Peer.SendResponse(false, "incorrect login");
                return;
            }

            if (!_authController.GetAccessLogIn(authorizationPacket.Login))
            {
                player.Peer.SendResponse(false, "access denied");
                return;
            }
            if (!BCrypt.Net.BCrypt.Verify(userModelFromClint.Password, userModelFromDb.Password))
            {
                player.Peer.SendResponse(false, "incorrect password");
                return;
            }
            player.Peer.SendResponse(true);
            Console.WriteLine("player.AuthController");

            var packet = MapperInstance.Mapper.Map<AuthorizationResultPacket>(userModelFromDb);
            _authController.LogIn(player, MapperInstance.Mapper.Map<PlayerAuthFullModel>(packet));
            await player.Peer.SendPacket(packet);
        }

        public bool TryHandle(ServerPlayerToController player, Packet packet)
        {
            switch (packet)
            {
                case AuthorizationPacket authorizationPackage:
                    AuthorizationClient(player, authorizationPackage);
                    break;
                default: return false;
            }

            return true;
        }
    }
}
using System;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Packets.Auth;
using RussianMunchkin.Server.DataBase;
using RussianMunchkin.Server.DataBase.Models;
using RussianMunchkin.Server.Game.TwentyOne.Player.Server;
using RussianMunchkin.Server.Mapper;

namespace RussianMunchkin.Server.Player.Auth.AuthFull
{
    public class Registration
    {
        private readonly DataBaseController _dataBaseController;
        private readonly AuthController _authController;

        public Registration(DataBaseController dataBaseController, AuthController authController)
        {
            _dataBaseController = dataBaseController;
            _authController = authController;
        }

        private async void RegistrationClient(ServerPlayerToController player, RegistrationPacket registrationPacket)
        {
            var registrationModelFromClient = HashPassword(MapperInstance.Mapper.Map<RegistrationModel>(registrationPacket));

            AuthorizationResultPacket authorizationResultPacket;
            try
            {
                var linesAdded = await _dataBaseController.Insert.DistributionModels(registrationModelFromClient);
                if (linesAdded != 1) player.Peer.SendResponse(false, "registration failed");
            }
            catch (Exception e)
            {
                player.Peer.SendResponse(false, "login is busy");
            }
            finally
            {
                var registrationModelFromDb =
                    await _dataBaseController.Select.GetRegistrationModel(registrationModelFromClient);
                authorizationResultPacket = MapperInstance.Mapper.Map<AuthorizationResultPacket>(registrationModelFromDb);
            
                _authController.LogIn(player,MapperInstance.Mapper.Map<PlayerAuthFullModel>(authorizationResultPacket));

                player.Peer.SendResponse(true);
                await player.Peer.SendPacket(authorizationResultPacket);
            }
        }

        private async void CheckLogin(ServerPlayerToController player, CheckLoginPacket checkLoginPacket)
        {
            var checkLoginModel = MapperInstance.Mapper.Map<CheckLoginModel>(checkLoginPacket);
            var checkLoginModelFromServer = await _dataBaseController.Select.CheckLogin(checkLoginModel);
            player.Peer.SendResponse(checkLoginModelFromServer.LoginExists);
        }

        private RegistrationModel HashPassword(RegistrationModel registrationModel)
        {
            registrationModel.Password = BCrypt.Net.BCrypt.HashPassword(registrationModel.Password, 10);
            return registrationModel;
        }

        public bool TryHandle(ServerPlayerToController player, Packet packet)
        {
            switch (packet)
            {
                case  RegistrationPacket registrationPacket:
                    RegistrationClient(player, registrationPacket);
                    break;
                case  CheckLoginPacket checkLoginPacket:
                    CheckLogin(player, checkLoginPacket);
                    break;
                default: return false;
            }

            return true;
        }
    }
}
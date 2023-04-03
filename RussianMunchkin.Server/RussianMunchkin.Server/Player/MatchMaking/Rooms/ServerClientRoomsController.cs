using System;
using RussianMunchkin.Common.Models;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Server.Core.Player.Models;
using RussianMunchkin.Server.Game.Player;
using RussianMunchkin.Server.Game.Player.Static;
using RussianMunchkin.Server.Game.TwentyOne.Player;
using RussianMunchkin.Server.Server;

namespace RussianMunchkin.Server.Player.MatchMaking.Rooms
{
    public class ServerClientRoomsController: IClientRoomsController
    {
        private readonly NetPeer _netPeer;
        private readonly PlayerModel _playerModel;
        private readonly ClientRoomsModel _roomsModel;

        public ClientRoomsModel RoomsModel => _roomsModel;

        public ServerClientRoomsController(NetPeer netPeer, PlayerModel playerModel, ClientRoomsModel roomsModel)
        {
            _netPeer = netPeer;
            _playerModel = playerModel;
            _roomsModel = roomsModel;
        }

        public async void EnterToRoom(RoomInfoModel roomInfoModel)
        {

            Console.WriteLine($"Send Enter to room");

            var res = await _netPeer.SendPacket(new ConnectToRoomPacket(){ RoomInfoModel = roomInfoModel});
            Console.WriteLine($"Player entered to room with result {res}");
            //if (res) PlayerEnteredToRoom(this);
            //todo: implement handle result
        }

        public async void PlayerEnteredToRoom(PlayerControllerBase playerOld)
        {
            var res = await _netPeer.SendPacket(
                new ChangeConnectionToRoomPacket()
                {
                    PlayerInfoModel = CommonModelCreatorStatic.GetPlayerInfoByPlayer(playerOld), 
                    IsConnected = true,
                });
            //todo: implement handle result

        }

        public async void PlayerLeftFromRoom(PlayerControllerBase player)
        {
            var res = await _netPeer.SendPacket(
                new ChangeConnectionToRoomPacket()
                {
                    PlayerInfoModel = CommonModelCreatorStatic.GetPlayerInfoByPlayer(player), 
                    IsConnected = false
                });
            //todo: implement handle result
        }

        
        public async void ExitFromRoom()
        {
            _roomsModel.IsReadyStartGame = false;
            var res = await _netPeer.SendPacket(new ExitFromRoomPacket(){ PlayerId = _playerModel.PlayerId});
            //todo: implement handle result
        }

        public async void ChangeAdmin(PlayerControllerBase player)
        {
            var res = await _netPeer.SendPacket(
                new ChangeAdminRoomPacket()
                {
                    PlayerInfoModel = CommonModelCreatorStatic.GetPlayerInfoByPlayer(player)
                }
            );
            //todo: implement handle result

        }

        public void ChangeStatusReady(bool isReady)
        {
            _roomsModel.IsReadyStartGame = isReady;
        }

        public async void ChangeStatusReadyPlayer(int playerId, bool isReady)
        {
            var res = await _netPeer.SendPacket(
                new ChangeStatusReadyPlayerPacket()
                {
                    PlayerId = playerId,
                    IsReady = isReady
                }
            );
            //todo: implement handle result

        }

        public async void ChangeStatusStartGame(bool isReady)
        {
            var res = await _netPeer.SendPacket(new ChangeStatusStartGamePacket(){ IsReady = isReady});
            //todo: implement handle result

        }
    }
}
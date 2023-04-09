using System;
using RussianMunchkin.Common.Models;
using RussianMunchkin.Common.Packets;
using RussianMunchkin.Common.Packets.Room;
using RussianMunchkin.Server.Core.Player;
using RussianMunchkin.Server.Core.Player.Models;
using RussianMunchkin.Server.Game.TwentyOne.Player;
using RussianMunchkin.Server.Mapper;
using RussianMunchkin.Server.Server;

namespace RussianMunchkin.Server.Player.MatchMaking.Rooms
{
    public class ServerPlayerRoomsController: IPlayerRoomsController
    {
        private readonly Peer _peer;
        private readonly PlayerModel _playerModel;
        private readonly PlayerRoomModel _playerRoomModel;

        public PlayerRoomModel PlayerRoomModel => _playerRoomModel;

        public ServerPlayerRoomsController(Peer peer, PlayerModel playerModel, PlayerRoomModel playerRoomModel)
        {
            _peer = peer;
            _playerModel = playerModel;
            _playerRoomModel = playerRoomModel;
        }

        public async void EnterToRoom(RoomInfoModel roomInfoModel)
        {
            var res = await _peer.SendPacket(new ConnectToRoomPacket(){ RoomInfoModel = roomInfoModel});
            
            Console.WriteLine($"Player entered to room with result {res}");
            //if (res) PlayerEnteredToRoom(this);
            //todo: implement handle result
        }

        public async void PlayerEnteredToRoom(PlayerControllerBase player)
        {
            var res = await _peer.SendPacket(
                new ChangeConnectionToRoomPacket()
                {
                    PlayerInfoModel = MapperInstance.Mapper.Map<PlayerInfoModel>(player),
                    IsConnected = true,
                });
            //todo: implement handle result

        }

        public async void PlayerLeftFromRoom(PlayerControllerBase player)
        {
            var res = await _peer.SendPacket(
                new ChangeConnectionToRoomPacket()
                {
                    PlayerInfoModel = MapperInstance.Mapper.Map<PlayerInfoModel>(player),
                    IsConnected = false
                });
            //todo: implement handle result
        }

        
        public async void ExitFromRoom()
        {
            _playerRoomModel.IsReadyStartGame = false;
            var res = await _peer.SendPacket(new ExitFromRoomPacket(){ PlayerLogin = _playerModel.Login});
            //todo: implement handle result
        }

        public async void ChangeAdmin(PlayerControllerBase player)
        {
            var res = await _peer.SendPacket(
                new ChangeAdminRoomPacket()
                {
                    PlayerInfoModel = MapperInstance.Mapper.Map<PlayerInfoModel>(player),
                }
            );
            //todo: implement handle result

        }

        public void ChangeStatusReady(bool isReady)
        {
            _playerRoomModel.IsReadyStartGame = isReady;
        }

        public async void ChangeStatusReadyPlayer(string playerLogin, bool isReady)
        {
            var res = await _peer.SendPacket(
                new ChangeStatusReadyPlayerPacket()
                {
                    PlayerLogin = playerLogin,
                    IsReady = isReady
                }
            );
            //todo: implement handle result

        }

        public async void ChangeStatusStartGame(bool isReady)
        {
            var res = await _peer.SendPacket(new ChangeStatusStartGamePacket(){ IsReady = isReady});
            //todo: implement handle result

        }
    }
}
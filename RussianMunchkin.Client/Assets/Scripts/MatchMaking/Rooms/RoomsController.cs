using System.Collections.Generic;
using Core;
using MatchMaking.Rooms;
using MatchMaking.Rooms.View;
using Models;
using RussianMunchkin.Common.Models;
using RussianMunchkin.Common.Packets;
using UnityEngine;

namespace Controllers
{
    public class RoomsController: ControllerBase
    {
        private readonly IRoomsView _view;
        private readonly PlayerModel _playerModel;
        private readonly RoomModel _roomModel;
        private PlayerInfoModel _playerInfoModel;
        

        public RoomsController(Peer peer, IRoomsView view, PlayerModel playerModel, RoomModel roomModel) : base(peer)
        {
            _view = view;
            _view.ControllerInit(playerModel);
            _playerModel = playerModel;
            _roomModel = roomModel;
            _roomModel.Players = new List<PlayerInfoModel>();
        }

        public override void Enable()
        {
            _view.CreateRoom+=RoomsViewOnCreateRoom;
            _view.GetPublicRooms+=ViewOnGetPublicRooms;
            _view.ConnectToRoom+=ViewOnConnectToRoom;
            _view.ChangeReady+=ViewOnChangeReady;
            _view.StartGame+=ViewOnStartGame;
            _view.LeftRoom+=ViewOnLeaveRoom;
        }

        public override void Disable()
        {
            _view.CreateRoom-=RoomsViewOnCreateRoom;
            _view.GetPublicRooms-=ViewOnGetPublicRooms;
            _view.ConnectToRoom-=ViewOnConnectToRoom;
            _view.ChangeReady-=ViewOnChangeReady;
            _view.StartGame-=ViewOnStartGame;
            _view.LeftRoom-=ViewOnLeaveRoom;
        }

        private async void ViewOnGetPublicRooms()
        {
            var res = await _peer.SendPacket(new GetListPublicRooms());
            if (res)
            {
                _view.ShowListPublicRooms();
            }
        }

        private async void ViewOnLeaveRoom()
        {
            var res = await _peer.SendPacket(new ExitFromRoomPacket(){PlayerLogin = _playerModel.Login});
            //todo implement handle result
        }

        private async void ViewOnStartGame()
        {
            var res = await _peer.SendPacket(new ChangeStatusGamePacket());
            //todo implement handle result
        }

        private async void ViewOnChangeReady(bool isReady)
        {
            var res = await _peer.SendPacket(new ChangeStatusReadyPlayerPacket(){PlayerLogin = _playerModel.Login, IsReady = isReady});
        }

        private async void ViewOnConnectToRoom(string uid, string password)
        {
            var res = await _peer.SendPacket(
                new RequestConnectToRoomPacket()
                {
                    Uid = uid, 
                    Password = password
                });
            if (!res)
            {
                if(res.Log == "incorrect pass0") Debug.Log("Password Incorect");
                Debug.LogError($"Not connected to room. Log = {res.Log}");
            }
            //todo implement handle request
        }

        public void ConnectToRoom(ConnectToRoomPacket connectToRoomPacket)
        {
            _roomModel.Players = new List<PlayerInfoModel>();
            _view.ConnectedToRoom(connectToRoomPacket.RoomInfoModel);
        }

        public void EnterPlayerToRoom(PlayerInfoModel playerInfoModel)
        {
            _roomModel.Players.Add(playerInfoModel);
            _view.EnterPlayerToRoom(playerInfoModel);
        }

        public void LeftPlayerFromRoom(PlayerInfoModel playerInfoModel)
        {
            _roomModel.Players.Remove(playerInfoModel);
            _view.LeftPlayerFromRoom(playerInfoModel);
        }
        private async void RoomsViewOnCreateRoom(bool isPrivate, int countPlayers)
        {
            var res = await _peer.SendPacket(new CreateRoomPacket()
                { IsPrivate = isPrivate, MaxCountPlayers = countPlayers });
            //todo create logic connect to room
        }

        public void ChangeAdmin(PlayerInfoModel playerInfoModel)
        {
            _view.ChangeAdmin(playerInfoModel); 
        }

        public void ChangeStatusReady(string playerLogin, bool isReady)
        {
            _view.ChangeStatusReady(playerLogin, isReady);
        }

        public void ChangeStatusStartGame(bool isReady)
        {
            _view.ChangeStatusStartGame(isReady);
        }

        public void LeaveRoom()
        {
            _view.LeaveRoom();
        }

        public void ShowListPublicRooms(List<RoomInfoModel> roomInfoModels)
        {
            _view.ShowListPublicRooms(roomInfoModels);
        }
    }
}
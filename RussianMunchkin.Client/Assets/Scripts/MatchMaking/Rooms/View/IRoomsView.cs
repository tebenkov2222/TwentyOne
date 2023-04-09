using System;
using System.Collections.Generic;
using Models;
using RussianMunchkin.Common.Models;

namespace MatchMaking.Rooms.View
{
    public delegate void CreateRoomHandler(bool isPrivate, int countPlayers);
    public delegate void ConnectToRoomHandler(string uid, string password);
    public interface IRoomsView
    {
        public event CreateRoomHandler CreateRoom;
        public event ConnectToRoomHandler ConnectToRoom;
        public event Action GetPublicRooms;
        public event Action LeftRoom;
        public event Action<bool> ChangeReady;
        public event Action StartGame;

        public void ConnectedToRoom(RoomInfoModel roomInfoModel);
        public void LeaveRoom();
        public void EnterPlayerToRoom(PlayerInfoModel playerInfoModel);
        public void LeftPlayerFromRoom(PlayerInfoModel playerInfoModel);
        public void ChangeAdmin(PlayerInfoModel playerInfoModel);
        public void ChangeStatusReady(string playerLogin, bool isReady);
        public void ChangeStatusStartGame(bool isReady);
        public void ShowListPublicRooms();
        public void ShowListPublicRooms(List<RoomInfoModel> roomInfoModels);
        public void ControllerInit(PlayerModel playerModel);
    }
}
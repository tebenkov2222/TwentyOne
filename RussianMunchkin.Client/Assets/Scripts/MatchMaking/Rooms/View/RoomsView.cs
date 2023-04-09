using System;
using System.Collections.Generic;
using MatchMaking.Rooms.View.Windows;
using MatchMaking.Rooms.View.Windows.ListPublicRooms;
using MatchMaking.Rooms.View.Windows.Lobby;
using Models;
using RussianMunchkin.Common.Models;
using UnityEngine;
using View.Views.Window;

namespace MatchMaking.Rooms.View
{
    public class RoomsView: WindowBase, IRoomsView
    {
        [SerializeField] private RoomsGeneralPanelView _roomsGeneralPanelView;
        [SerializeField] private CreateRoomWindow createRoomWindow;
        [SerializeField] private ConnectToRoomWindow _connectToRoomWindow;
        [SerializeField] private LobbyWindow lobbyWindow;
        [SerializeField] private ListPublicRoomsWindow _listPublicRoomsWindow;
        private PlayerModel _playerModel;
        private RoomInfoModel _roomInfoModel;
        public event CreateRoomHandler CreateRoom;
        public event ConnectToRoomHandler ConnectToRoom;
        public event Action GetPublicRooms;
        public event Action LeftRoom;
        public event Action<bool> ChangeReady;
        public event Action StartGame;
        public event Action RoomLeaved;
        //public event Action GameStarted;

        public void Init()
        {
            Hide();
        }

        public void ControllerInit(PlayerModel model)
        {
            _playerModel = model;
        }

        private void OnEnable()
        {
            _listPublicRoomsWindow.WindowClosed+=ListPublicRoomsWindowOnWindowClosed;
            _listPublicRoomsWindow.ConnectToRoom+=ListPublicRoomsWindowOnConnectToRoom;
            
            _roomsGeneralPanelView.GetPublicRoomsClicked+=RoomsClickedGeneralPanelViewOnGetPublicRoomsClicked;
            _roomsGeneralPanelView.CreateRoomClicked+=RoomsClickedGeneralPanelViewOnCreateRoomClicked;
            _roomsGeneralPanelView.ConnectToRoomClicked+=ToRoomsClickedGeneralPanelViewOnConnectToRoomClicked;
            
            createRoomWindow.WindowClosed+=RoomItemWindowClosed;
            createRoomWindow.CreateRoom+=CreateRoomWindowOnCreateRoom;
            
            _connectToRoomWindow.WindowClosed+=RoomItemWindowClosed;
            _connectToRoomWindow.ConnectToRoom+=ConnectToRoomWindowOnConnectToRoom;
            
            lobbyWindow.ChangeReadyStatus+=LobbyWindowOnChangeReadyStatus;
            lobbyWindow.GameStarted+=LobbyWindowOnGameStarted;
            lobbyWindow.ExitFromRoom+=LobbyWindowOnExitFromRoom;

        }

        private void OnDisable()
        {
            _listPublicRoomsWindow.WindowClosed-=ListPublicRoomsWindowOnWindowClosed;
            _listPublicRoomsWindow.ConnectToRoom-=ListPublicRoomsWindowOnConnectToRoom;

            _roomsGeneralPanelView.GetPublicRoomsClicked-=RoomsClickedGeneralPanelViewOnGetPublicRoomsClicked;
            _roomsGeneralPanelView.CreateRoomClicked-=RoomsClickedGeneralPanelViewOnCreateRoomClicked;
            _roomsGeneralPanelView.ConnectToRoomClicked+=ToRoomsClickedGeneralPanelViewOnConnectToRoomClicked;
            
            createRoomWindow.WindowClosed-=RoomItemWindowClosed;
            createRoomWindow.CreateRoom-=CreateRoomWindowOnCreateRoom;
            
            _connectToRoomWindow.WindowClosed-=RoomItemWindowClosed;
            _connectToRoomWindow.ConnectToRoom-=ConnectToRoomWindowOnConnectToRoom;
            
            lobbyWindow.ChangeReadyStatus-=LobbyWindowOnChangeReadyStatus;
            lobbyWindow.GameStarted-=LobbyWindowOnGameStarted;
            lobbyWindow.ExitFromRoom-=LobbyWindowOnExitFromRoom;
        }

        public void ConnectedToRoom(RoomInfoModel roomInfoModel)
        {
            _roomInfoModel = roomInfoModel;
            createRoomWindow.Hide();
            _connectToRoomWindow.Hide();
            _listPublicRoomsWindow.Hide();
            lobbyWindow.Show();
            lobbyWindow.ConnectedToRoom(roomInfoModel);
        }

        public void LeaveRoom()
        {
            lobbyWindow.Hide();
            _roomsGeneralPanelView.Show();
            RoomLeaved?.Invoke();
        }

        public void SendRequestLeaveRoom()
        {
            LeftRoom?.Invoke();
        }

        private void ListPublicRoomsWindowOnConnectToRoom(RoomInfoModel model)
        {
            ConnectToRoom?.Invoke(model.Uid, model.Password);
        }

        private void ListPublicRoomsWindowOnWindowClosed()
        {
            _roomsGeneralPanelView.Show();
        }

        private void RoomsClickedGeneralPanelViewOnGetPublicRoomsClicked()
        {
            GetPublicRooms?.Invoke();
        }

        private void LobbyWindowOnExitFromRoom()
        {
            SendRequestLeaveRoom();
        }

        private void LobbyWindowOnGameStarted()
        {
            StartGame?.Invoke();
        }

        private void LobbyWindowOnChangeReadyStatus(bool isReady)
        {
            ChangeReady?.Invoke(isReady);
        }

        private void ToRoomsClickedGeneralPanelViewOnConnectToRoomClicked()
        {
            _connectToRoomWindow.Show();
            _roomsGeneralPanelView.Hide();
        }

        private void RoomsClickedGeneralPanelViewOnCreateRoomClicked()
        {
            createRoomWindow.Show();
            _roomsGeneralPanelView.Hide();
        }

        private void ConnectToRoomWindowOnConnectToRoom(string uid, string password)
        {
            ConnectToRoom?.Invoke(uid, password);
        }

        private void CreateRoomWindowOnCreateRoom(bool isPrivate, int countPlayers)
        {
            CreateRoom?.Invoke(isPrivate, countPlayers);
        }

        private void RoomItemWindowClosed()
        {
            _roomsGeneralPanelView.Show();
        }

        public override void Hide()
        {
            createRoomWindow.Hide();
            _roomsGeneralPanelView.Hide();
            lobbyWindow.Hide();
            _connectToRoomWindow.Hide();
            _listPublicRoomsWindow.Hide();
        }
        public void ShowGeneralPanel()
        {
            _roomsGeneralPanelView.Show();
        }


        public void EnterPlayerToRoom(PlayerInfoModel playerInfoModel)
        {
            lobbyWindow.EnterPlayerToRoom(playerInfoModel);
            if(_playerModel.Login == playerInfoModel.Login ) SetMeIsAdmin(playerInfoModel.Login  == _roomInfoModel.AdminPlayer.Login);

        }

        public void LeftPlayerFromRoom(PlayerInfoModel playerInfoModel)
        {
            lobbyWindow.LeftPlayerFromRoom(playerInfoModel);
        }
        public void ChangeAdmin(PlayerInfoModel playerInfoModel)
        {
            lobbyWindow.SetAdmin(playerInfoModel);
            SetMeIsAdmin(_playerModel.Login == playerInfoModel.Login);
        }

        public void ChangeStatusReady(string playerLogin, bool isReady)
        {
            lobbyWindow.ChangeStatusReady(playerLogin, isReady);
        }

        public void ChangeStatusStartGame(bool isReady)
        {
            lobbyWindow.ChangeStatusStartGame(isReady);
        }

        public void SetMeIsAdmin(bool isAdmin)
        {
            lobbyWindow.SetMeIsAdmin(isAdmin);
        }

        public void ShowListPublicRooms()
        {
            Hide();
            _listPublicRoomsWindow.Show();
            _listPublicRoomsWindow.SetActiveUpdateAnimation(true);
        }

        public void ShowListPublicRooms(List<RoomInfoModel> roomInfoModels)
        {
            _listPublicRoomsWindow.UpdateList(roomInfoModels);
        }
    }
}
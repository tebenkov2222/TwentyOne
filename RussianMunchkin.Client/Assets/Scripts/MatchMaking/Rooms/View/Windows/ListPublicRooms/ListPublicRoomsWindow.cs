using System;
using System.Collections.Generic;
using RussianMunchkin.Common.Models;
using UnityEngine;
using UnityEngine.UI;
using View.Views.Window;

namespace MatchMaking.Rooms.View.Windows.ListPublicRooms
{
    public class ListPublicRoomsWindow: WindowBase
    {
        [SerializeField] private ListPublicRoomsItem _prefab;
        [SerializeField] private ListPublicRoomsItemsGroupView _listPublicRoomsItemsGroupView;
        
        [SerializeField] private GameObject _updateAnimationGo;
        [SerializeField] private Button _closeButton;
        public event Action<RoomInfoModel> ConnectToRoom;

        private void OnEnable()
        {
            RemoveAllRooms();
            _closeButton.onClick.AddListener(CloseButtonOnClick);
            _listPublicRoomsItemsGroupView.ConnectToRoom += RoomItemOnConnectToRoom;
        }

        private void OnDisable()
        {
            _listPublicRoomsItemsGroupView.ConnectToRoom -= RoomItemOnConnectToRoom;
            _closeButton.onClick.RemoveListener(CloseButtonOnClick);
        }

        private void CloseButtonOnClick()
        {
            CloseWindow();
        }

        public void UpdateList(List<RoomInfoModel> rooms)
        {
            SetActiveUpdateAnimation(false);
            RemoveAllRooms();
            foreach (var room in rooms)
            {
                CreateRoomItem(room);
            }
        }

        public void SetActiveUpdateAnimation(bool value)
        {
            _updateAnimationGo.SetActive(value);
        }
        private void RemoveAllRooms()
        {
            _listPublicRoomsItemsGroupView.RemoveAll();
        }

        private void CreateRoomItem(RoomInfoModel roomInfoModel)
        {
            var roomItem = _listPublicRoomsItemsGroupView.Add(_prefab);
            roomItem.ShowRoom(roomInfoModel);
        }

        private void RoomItemOnConnectToRoom(RoomInfoModel model)
        {
            ConnectToRoom?.Invoke(model);
        }
    }
}
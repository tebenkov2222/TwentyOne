using System;
using UnityEngine;
using UnityEngine.UI;
using View.Views.Window;

namespace MatchMaking.Rooms.View.Windows
{
    public class RoomsGeneralPanelView: WindowBase
    {
        [SerializeField] private Button _createRoom;
        [SerializeField] private Button _connectToRoom;
        [SerializeField] private Button _getPublicRooms;
        public event Action CreateRoomClicked; 
        public event Action GetPublicRoomsClicked; 
        public event Action ConnectToRoomClicked; 
        private void OnEnable()
        {
            _createRoom.onClick.AddListener(CreateRoomOnClicked);
            _connectToRoom.onClick.AddListener(ConnectToRoomOnClicked);
            _getPublicRooms.onClick.AddListener(GetPublishRoomsOnClick);
        }

        private void OnDisable()
        {
            _connectToRoom.onClick.RemoveListener(ConnectToRoomOnClicked);
            _createRoom.onClick.RemoveListener(CreateRoomOnClicked);
            _getPublicRooms.onClick.RemoveListener(GetPublishRoomsOnClick);

        }

        private void GetPublishRoomsOnClick()
        {
            GetPublicRoomsClicked?.Invoke();
        }

        private void CreateRoomOnClicked()
        {
            CreateRoomClicked?.Invoke();
        }

        private void ConnectToRoomOnClicked()
        {
            ConnectToRoomClicked?.Invoke();
        }
    }
}
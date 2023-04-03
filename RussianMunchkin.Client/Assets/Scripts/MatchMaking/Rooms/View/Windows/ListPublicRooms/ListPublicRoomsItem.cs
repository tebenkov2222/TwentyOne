using System;
using RussianMunchkin.Common.Models;
using UnityEngine;
using UnityEngine.UI;

namespace MatchMaking.Rooms.View.Windows.ListPublicRooms
{
    public class ListPublicRoomsItem: MonoBehaviour
    {
        [SerializeField] private Text _uidText;
        [SerializeField] private Text _adminUsername;
        [SerializeField] private Text _currentCountPlayer;
        [SerializeField] private Button _connectToRoomButton;
        public event Action<RoomInfoModel> ConnectToRoom; 
        private RoomInfoModel _roomInfoModel;

        private void OnEnable()
        {
            _connectToRoomButton.onClick.AddListener(ConnectToRoomButtonOnClick);
        }

        private void OnDisable()
        {
            _connectToRoomButton.onClick.RemoveListener(ConnectToRoomButtonOnClick);
        }

        private void ConnectToRoomButtonOnClick()
        {
            ConnectToRoom?.Invoke(_roomInfoModel);
        }

        public void ShowRoom(RoomInfoModel roomInfoModel)
        {
            _roomInfoModel = roomInfoModel;
            _uidText.text = roomInfoModel.Uid;
            _adminUsername.text = roomInfoModel.AdminPlayer.Username;
            _currentCountPlayer.text = $"{roomInfoModel.CurrentCountPlayers}/{roomInfoModel.MaxCountPlayers}";
        }
    }
}
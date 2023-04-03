using UnityEngine;
using UnityEngine.UI;
using View.Views.Window;

namespace MatchMaking.Rooms.View.Windows
{
    public class CreateRoomWindow: WindowBase
    {
        [SerializeField] private Toggle _isPrivateToggle;
        [SerializeField] private Dropdown _countPlayersDropdown;
        [SerializeField] private Button _createRoomButton;
        [SerializeField] private Button _closeWindowButton;
        
        public event CreateRoomHandler CreateRoom;
        private void OnEnable()
        {
            _createRoomButton.onClick.AddListener(ButtonCreateRoomOnClick);
            _closeWindowButton.onClick.AddListener(CloseWindowButtonOnClick);
        }

        private void CloseWindowButtonOnClick()
        {
            CloseWindow();
        }

        private void ButtonCreateRoomOnClick()
        {
            var value = _countPlayersDropdown.value + 2; // todo убрать костыль
            CreateRoom?.Invoke(_isPrivateToggle.isOn, value);
        }

        private void OnDisable()
        {
            _createRoomButton.onClick.RemoveListener(ButtonCreateRoomOnClick);
            _closeWindowButton.onClick.RemoveListener(CloseWindowButtonOnClick);

        }
    }
}
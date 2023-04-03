using UnityEngine;
using UnityEngine.UI;
using View.Views.Window;

namespace MatchMaking.Rooms.View.Windows
{
    public class ConnectToRoomWindow: WindowBase
    {
        private const int _lenghtUid = 7;
        private const int _minPasswordLenght = 3;
        
        [SerializeField] private InputField _uidRoomInputField;
        [SerializeField] private InputField _passwordRoomInputField;
        [SerializeField] private Button _connectButton;
        [SerializeField] private Button _closeButton;
        public event ConnectToRoomHandler ConnectToRoom;
        private bool _isUidCorrect;
        private bool _isPasswordCorrect;
        private void OnEnable()
        {
            _connectButton.onClick.AddListener(ConnectButtonOnCLick);
            _closeButton.onClick.AddListener(CloseButtonOnCLick);
            
            _uidRoomInputField.onValueChanged.AddListener(UidRoomInputFieldOnChange);
            _passwordRoomInputField.onValueChanged.AddListener(PasswordRoomInputFieldOnChange);
        }

        private void UidRoomInputFieldOnChange(string uid)
        {
            _isUidCorrect = uid.Length == _lenghtUid;
            CheckInteractableButton();
        }
        private void PasswordRoomInputFieldOnChange(string password)
        {
            _isPasswordCorrect = password.Length > _minPasswordLenght;
            CheckInteractableButton();
        }

        private void CheckInteractableButton()
        {
            _connectButton.interactable = _isUidCorrect && _isPasswordCorrect;
        }
        private void ConnectButtonOnCLick()
        {
            ConnectToRoom?.Invoke(_uidRoomInputField.text, _passwordRoomInputField.text);
        }
        private void CloseButtonOnCLick()
        {
            CloseWindow();
        }
        private void OnDisable()
        {
            _connectButton.onClick.RemoveListener(ConnectButtonOnCLick);
            _closeButton.onClick.RemoveListener(CloseButtonOnCLick);

            _uidRoomInputField.onValueChanged.RemoveListener(UidRoomInputFieldOnChange);
            _passwordRoomInputField.onValueChanged.RemoveListener(PasswordRoomInputFieldOnChange);
        }
    }
}